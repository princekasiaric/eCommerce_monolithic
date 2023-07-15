using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construmart.Core.ProcessorContracts.FileStorage
{
    public class DTOs
    {
        public record FileUploadResponse(string FileName, string Url, string SecureUrl, string Extension);
    }
}