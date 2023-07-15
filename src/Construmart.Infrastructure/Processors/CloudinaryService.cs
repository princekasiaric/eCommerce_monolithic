using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Construmart.Core.Commons;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.ProcessorContracts.FileStorage;
using static Construmart.Core.ProcessorContracts.FileStorage.DTOs;

namespace Construmart.Infrastructure.Processors
{
    public class CloudinaryService : IFileStorageService
    {
        private readonly Account _account;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            _account = new Account(Env.CloudinaryCloud, Env.CloudinaryKey, Env.CloudinarySecret);
            _cloudinary = new Cloudinary(_account);
        }

        public async Task<(bool, string, FileUploadResponse)> UploadFileAsync(string base64string, FileTypes fileType, string folderpath)
        {
            if (fileType == FileTypes.Image)
            {
                return await UploadImage(base64string, folderpath);
            }
            return (false, "Invalid file type", null);
        }

        private async Task<(bool, string, FileUploadResponse)> UploadImage(string base64string, string folderPath)
        {
            var dateStringFormat = "ddMMyyyyhhmmssffff";
            var bytes = Convert.FromBase64String(base64string);
            var fileName = $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}_{DateTime.Now.ToString(dateStringFormat)}";
            var stream = new MemoryStream(bytes);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = fileName,
                Folder = folderPath,
                Format = MediaTypeNames.Image.Jpeg.Split("/").Last()
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (((int)uploadResult.StatusCode >= 200 && (int)uploadResult.StatusCode < 300) && uploadResult.Error == null)
            {
                return (true, null, new FileUploadResponse(fileName, uploadResult.Url.ToString(), uploadResult.SecureUrl.ToString(), uploadParams.Format));
            }
            return (false, "Failed to upload image", null);
        }

        // private static bool IsFileValid(string extension, FileTypes fileType)
        // {
        //     if (string.IsNullOrEmpty(extension))
        //     {
        //         return false;
        //     }
        //     if (fileType == FileTypes.Image)
        //     {
        //         return extension.ToLower() switch
        //         {
        //             ".jpg" or ".jpeg" or ".png" => true,
        //             _ => false,
        //         };
        //     }
        //     if (fileType == FileTypes.Document)
        //     {
        //         return extension.ToLower() switch
        //         {
        //             ".pdf" or ".docx" or ".doc" or ".csv" or ".xlsx" => true,
        //             _ => false,
        //         };
        //     }
        //     if (fileType == FileTypes.ImageAndDocument)
        //     {
        //         return extension.ToLower() switch
        //         {
        //             ".pdf" or ".docx" or ".doc" or ".csv" or ".xlsx" or ".jpg" or ".jpeg" or ".png" => true,
        //             _ => false,
        //         };
        //     }
        //     return false;
        // }
    }
}
