using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Construmart.Core.Domain.Enumerations;
using static Construmart.Core.ProcessorContracts.FileStorage.DTOs;

namespace Construmart.Core.ProcessorContracts.FileStorage
{
    public interface IFileStorageService
    {
        Task<(bool isSuccessful, string msg, FileUploadResponse data)> UploadFileAsync(string base64string, FileTypes fileType, string folderpath);
    }
}