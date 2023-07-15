using System;
using System.Collections.Generic;
using Construmart.Core.Domain.Models;

namespace Construmart.Core.ProcessorContracts.Identity.DTOs
{
    // Requests
    public record AuthenticateUserRequest(string Email, string Password);
    public record CreateUserRequest(
        string Email,
        string Password,
        bool ConfirmEmail = true,
        IEnumerable<string> Roles = null);
    public record ChangePasswordRequest(string Email, string CurrentPassword, string NewPassword);
    public record ResetPasswordRequest(string Email, string Password, string Otp);


    // Responses
    public record AuthenticateUserResponse(string AccessToken, DateTime ValidTo, IEnumerable<string> Roles = default);
    public record UserIdResponse(long ApplicationUserId);
    public record UserResponse(ApplicationUser User);
}