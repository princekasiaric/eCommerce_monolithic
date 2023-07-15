using System.Collections.Generic;

namespace Construmart.Core.ProcessorContracts.Cache.DTOs
{
    public record UserSignupRequest(
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        bool ConfirmEmail,
        string Password,
        string Otp,
        IEnumerable<string> Roles);
}