using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.ProcessorContracts.Notification;
using Construmart.Core.ProcessorContracts.Notification.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Construmart.Core.Domain.Events
{
    public class OtpCreated : Event, INotification
    {
        public OtpCreated(string email, string otp, string message) : base(message)
        {
            Email = Guard.Against.NullOrEmpty(email, nameof(email));
            Otp = Guard.Against.NullOrEmpty(otp, nameof(otp));
        }

        public string Email { get; private set; }
        public string Otp { get; private set; }
    }

    public class OtpGeneratedEventHandler : INotificationHandler<OtpCreated>
    {
        private readonly ILogger<OtpGeneratedEventHandler> _logger;
        private readonly INotificationService _notificationService;

        public OtpGeneratedEventHandler(
            ILogger<OtpGeneratedEventHandler> logger,
            INotificationService notificationService
        )
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _notificationService = Guard.Against.Null(notificationService, nameof(notificationService));
        }

        public async Task Handle(OtpCreated notification, CancellationToken cancellationToken)
        {
            var placeholders = new Dictionary<string, string>{
                    {Constants.NotificationTemplates.SignupOtp.Placeholders.OTP, notification.Otp}
                };
            var body = await _notificationService.PrepareTemplateAsync(Constants.NotificationTemplates.SignupOtp.SIGNUP_OTP_TEMPLATE, placeholders);
            var emailRequest = new EmailRequest(notification.Email, "Customer Signup", HtmlBody: body);
            _notificationService.SendSmtpEmail(emailRequest);
            _logger.LogInformation("mail sent?: ", _notificationService.IsMailSent());
            _logger.LogInformation("OTP_GENERATED =>" + notification.ToString());
        }
    }
}