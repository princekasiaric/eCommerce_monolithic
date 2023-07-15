using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.ValueObjects
{
    public class File : ValueObjectBase
    {
        public string Name { get; private set; }
        public string UploadUrl { get; private set; }
        public string SecureUploadUrl { get; private set; }
        public string Extension { get; private set; }

        private File(string name, string uploadUrl, string secureUploadUrl, string extension)
        {
            Name = name;
            UploadUrl = uploadUrl;
            SecureUploadUrl = secureUploadUrl;
            Extension = extension;
        }

        public static File Create(string uploadUrl, string secureUploadUrl)
        {
            Guard.Against.NullOrWhiteSpace(uploadUrl, nameof(uploadUrl));
            Guard.Against.NullOrWhiteSpace(Path.GetFileNameWithoutExtension(uploadUrl), nameof(uploadUrl), "Failed to get file name");
            Guard.Against.NullOrWhiteSpace(Path.GetExtension(uploadUrl), nameof(uploadUrl), "Failed to get file extension");
            return new File(Path.GetFileNameWithoutExtension(uploadUrl), uploadUrl, secureUploadUrl, Path.GetExtension(uploadUrl));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Extension;
        }
    }
}