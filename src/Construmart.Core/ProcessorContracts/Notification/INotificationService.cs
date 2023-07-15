using System.Collections.Generic;
using System.Threading.Tasks;
using Construmart.Core.ProcessorContracts.Notification.DTOs;

namespace Construmart.Core.ProcessorContracts.Notification
{
    public interface INotificationService
    {
        Task<string> PrepareTemplateAsync(string fileName, IDictionary<string, string> placeholders);
        void SendSmtpEmail(EmailRequest request);
        bool IsMailSent();
    }
}