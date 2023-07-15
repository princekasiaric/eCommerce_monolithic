using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.ValueObjects
{
    public class Otp : ValueObjectBase
    {
        private string _code;

        public string Hash { get; private set; }
        public OtpPurpose Purpose { get; private set; }
        public DateTime? Expiry { get; private set; }
        public bool IsUsed { get; private set; }

        private Otp()
        {

        }

        private Otp(OtpPurpose purpose, IEncryptionUtility encryptionUtility, string code = null, string salt = null)
        {
            Guard.Against.Null(purpose, nameof(purpose));
            if (string.IsNullOrEmpty(code))
            {
                GenerateOtpCode();
            }
            else
            {
                _code = code;
            }
            Guard.Against.NullOrWhiteSpace(_code, nameof(_code));
            Hash = encryptionUtility.HashWithSalt(_code, salt);
            Purpose = purpose ?? throw new ArgumentNullException(nameof(purpose));
            IsUsed = false;
        }

        private Otp(OtpPurpose purpose, IEncryptionUtility hmacUtility, TimeSpan validTill, string code = null, string salt = null)
            : this(purpose, hmacUtility, code, salt)
        {
            Expiry = DateTime.Now + validTill;
        }

        public static Otp Create(
            OtpPurpose purpose,
            IEncryptionUtility utility,
            TimeSpan validTill,
            string code = null,
            string salt = null) => new(purpose, utility, validTill, code, salt);

        private void GenerateOtpCode(int length = 5)
        {
            var random = new Random();
            const string characters = "0123456789";
            _code = new string(Enumerable.Repeat(characters, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GetOtpCode() => _code;

        public (bool, string) Verify(string rawOtp, IEncryptionUtility encryptionUtility)
        {
            if (this == null) return (false, "Otp is invalid");
            if (this.IsUsed) return (false, "Otp has been used");
            var salt = this?.Hash.Split("|").Last();
            var otp = new Otp(this.Purpose, encryptionUtility, rawOtp, salt);
            if (this != otp) return (false, "Otp is invalid");
            if (DateTime.Compare(DateTime.Now, (DateTime)Expiry) > 0)
            {
                return (false, "Otp expired");
            }
            this.IsUsed = true;
            return (true, null);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Hash;
            yield return Purpose.Value;
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}