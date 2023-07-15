using System.Collections.Generic;
using System.IO;

namespace Construmart.Core.ProcessorContracts.Notification.DTOs
{
    public record EmailRequest(
        string ToAddress,
        string Subject,
        string FromAddress = null,
        string FromName = null,
        string HtmlBody = null,
        string StringBody = null,
        IList<Stream> Attachments = null);
}