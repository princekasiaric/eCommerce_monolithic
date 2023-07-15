using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity.DTOs;

namespace Construmart.Core.ProcessorContracts.Identity
{
    public interface IIdentityService
    {
        Task<BaseResponse> AuthenticateAsync(AuthenticateUserRequest request, Clients client);
        Task<BaseResponse> AuthenticateAsync(long userId);
        Task<BaseResponse> UpdatePhoneNumberAsync(long applicationUserId, string phoneNumber);
        Task<BaseResponse> RegisterAsync(CreateUserRequest request, Clients client);
        Task<BaseResponse> ActivateAsync(long applicationUserId, IEnumerable<string> roles);
        Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request, Clients client);
        Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request, Clients client);
        BaseResponse GetUserIdFromClaims(ClaimsPrincipal claimsPrincipal);
        Task<BaseResponse> GetUserFromEmail(string email, Clients client);
        Task<BaseResponse> SendOtp(long userId, OtpPurpose otpPurpose);
        Task<BaseResponse> SendOtp(string email, OtpPurpose otpPurpose, Clients client);
        Task<BaseResponse> VerifyOtp(long userId, string otp);
        Task<BaseResponse> VerifyOtp(string email, string otp, Clients client);
    }
}