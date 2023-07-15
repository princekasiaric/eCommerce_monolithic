using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Events;
using Construmart.Core.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Construmart.Core.Domain.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public ApplicationUser()
        {
            _userRoles = new List<IdentityUserRole<long>>();
        }
        public ApplicationUser(string userName) : base(userName)
        {
        }

        public ApplicationUser(
            string username,
            string normalizedUserName,
            string email,
            bool emailConfirmed,
            bool lockoutEnabled,
            bool isActive,
            string securityStamp = null) : this(username)
        {
            Email = email;
            EmailConfirmed = emailConfirmed;
            LockoutEnabled = lockoutEnabled;
            IsActive = isActive;
            SecurityStamp = securityStamp;
            NormalizedUserName = normalizedUserName;
        }

        public ApplicationUser(
            long id,
            string username,
            string normalizedUserName,
            string email,
            bool emailConfirmed,
            bool lockoutEnabled,
            bool isActive,
            string securityStamp = null)
            :
            this(
            username,
            normalizedUserName,
            email,
            emailConfirmed,
            lockoutEnabled,
            isActive,
            securityStamp
        )
        {
            base.Id = id;
        }

        public bool IsActive { get; private set; }
        public Otp Otp { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime? DateUpdated { get; private set; }

        private readonly List<IdentityUserRole<long>> _userRoles;
        public IReadOnlyCollection<IdentityUserRole<long>> UserRoles => _userRoles;

        public void SavePassword(string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            PasswordHash = passwordHasher.HashPassword(this, password);
        }

        public void AddToRoles(IEnumerable<ApplicationRole> roles)
        {
            Guard.Against.NullOrEmpty(roles, nameof(roles));
            foreach (var role in roles)
            {
                _userRoles.Add(new IdentityUserRole<long>
                {
                    RoleId = role.Id,
                    UserId = Id
                });
            }
        }

        public void UpdatePhoneNumber(string phone) => PhoneNumber = phone;

        public void SaveOtp(IEncryptionUtility utility, OtpPurpose otpPurpose) => Otp = Otp.Create(otpPurpose, utility, TimeSpan.FromMinutes(30));

        public void RegisterLastLogin() => LastLogin = DateTime.UtcNow;

        public void ToggleActiveStatus() => IsActive = !IsActive;

        public void ToggleEmailConfirmed() => EmailConfirmed = !EmailConfirmed;

        public void ToggleLockoutEnabled() => LockoutEnabled = !LockoutEnabled;
    }
}