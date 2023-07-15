using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.Configurations;
using Construmart.Core.ProcessorContracts.Notification;
using Construmart.Core.ProcessorContracts.Notification.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Construmart.Core.Commons.Constants;

namespace Construmart.Infrastructure.Processors
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly EmailConfig _emailConfig;
        private bool _mailSent;

        public NotificationService(
            ILogger<NotificationService> logger,
            IWebHostEnvironment hostingEnvironment,
            IOptions<EmailConfig> emailConfigOptions)
        {
            _mailSent = false;
            _logger = Guard.Against.Null(logger, nameof(logger));
            _hostingEnvironment = Guard.Against.Null(hostingEnvironment, nameof(hostingEnvironment));
            _emailConfig = emailConfigOptions.Value;
        }

        public async Task<string> PrepareTemplateAsync(string fileName, IDictionary<string, string> placeholders)
        {
            Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
            string path = Path.Combine(_hostingEnvironment.WebRootPath, NotificationTemplates.FOLDER, fileName);
            var templateString = await File.ReadAllTextAsync(path, Encoding.UTF8);
            foreach (var item in placeholders)
            {
                templateString = templateString.Replace(item.Key, item.Value);
            }
            return templateString;
        }

        public void SendSmtpEmail(EmailRequest request)
        {
            Guard.Against.NullOrWhiteSpace(Env.EmailFromAddress ?? _emailConfig.EmailFromAddress, nameof(Env.EmailFromAddress));
            Guard.Against.NullOrWhiteSpace(Env.EmailFromName ?? _emailConfig.EmailFromName, nameof(Env.EmailFromName));
            Guard.Against.NullOrWhiteSpace(Env.EmailHost ?? _emailConfig.EmailHost, nameof(Env.EmailHost));
            Guard.Against.NullOrWhiteSpace(Env.EmailPassword ?? _emailConfig.EmailPassword, nameof(Env.EmailPassword));
            Guard.Against.NullOrWhiteSpace(Env.EmailPort ?? _emailConfig.EmailPort, nameof(Env.EmailPort));
            Guard.Against.NullOrWhiteSpace(Env.EmailUserName ?? _emailConfig.EmailUserName, nameof(Env.EmailUserName));
            Guard.Against.InvalidFormat(Env.EmailPort ?? _emailConfig.EmailPort, nameof(Env.EmailPort), Constants.AppRegex.DIGIT);
            Guard.Against.Null(request, nameof(request));
            Guard.Against.NullOrWhiteSpace(request.ToAddress, nameof(request.ToAddress));

            var from = new MailAddress(
                request.FromAddress ?? Env.EmailFromAddress ?? _emailConfig.EmailFromAddress,
                request.FromName ?? Env.EmailFromName ?? _emailConfig.EmailFromName,
                Encoding.UTF8);
            var to = new MailAddress(request.ToAddress);
            using var message = new MailMessage(from, to);
            message.Body = request.HtmlBody ?? request.StringBody;
            message.IsBodyHtml = !string.IsNullOrEmpty(request.HtmlBody);
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = request.Subject;
            message.SubjectEncoding = Encoding.UTF8;
            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var attachment in request.Attachments)
                {
                    message.Attachments.Add(new Attachment(attachment, MediaTypeNames.Application.Octet));
                }
            }
            using var client = new SmtpClient(Env.EmailHost ?? _emailConfig.EmailHost, int.Parse(Env.EmailPort ?? _emailConfig.EmailPort));
            client.UseDefaultCredentials = false;
            var smtpCredentials = new NetworkCredential(Env.EmailUserName ?? _emailConfig.EmailUserName, Env.EmailPassword ?? _emailConfig.EmailPassword);
            client.Credentials = smtpCredentials;
            client.EnableSsl = false;
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            var userToken = Guid.NewGuid().ToString();
            client.Send(message);
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            var token = (string)e.UserState;

            if (e.Cancelled)
            {
                //TODO Decide if excception should be thrown here
                _logger.LogInformation("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                _logger.LogError("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                _logger.LogInformation("Message sent.");
            }
            _mailSent = true;
        }

        public bool IsMailSent() => _mailSent;
    }
}