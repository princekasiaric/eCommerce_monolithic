using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.Commons.Utils;
using Construmart.Core.Configurations;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using Construmart.Core.ProcessorContracts.Notification;
using Construmart.Core.ProcessorContracts.Notification.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Construmart.Infrastructure.Processors.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResult _result;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthConfig _authConfig;
        private readonly IEncryptionUtility _encryptionUtility;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly INotificationService _notificationService;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IResult responseUtility,
            SignInManager<ApplicationUser> signInManager,
            IOptions<AuthConfig> authConfig,
            IEncryptionUtility encryptionUtility,
            RoleManager<ApplicationRole> roleManager,
            INotificationService notificationService,
            ILogger<IdentityService> logger)
        {
            _userManager = Guard.Against.Null(userManager, nameof(userManager));
            _result = Guard.Against.Null(responseUtility, nameof(responseUtility));
            _signInManager = Guard.Against.Null(signInManager, nameof(signInManager));
            _authConfig = Guard.Against.Null(authConfig.Value, nameof(authConfig));
            _encryptionUtility = Guard.Against.Null(encryptionUtility, nameof(encryptionUtility));
            _roleManager = Guard.Against.Null(roleManager, nameof(roleManager));
            _notificationService = Guard.Against.Null(notificationService, nameof(notificationService));
            _logger = Guard.Against.Null(logger, nameof(_logger));
        }

        public async Task<BaseResponse> AuthenticateAsync(AuthenticateUserRequest request, Clients client)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.Null(client, nameof(client));
            ApplicationRole role = null;
            if (client.DisplayName == Clients.AdministratorApp.DisplayName)
            {
                role = await _roleManager.FindByNameAsync(RoleTypes.SuperAdmin.DisplayName);
            }
            if (client.DisplayName == Clients.CustomerApp.DisplayName)
            {
                role = await _roleManager.FindByNameAsync(RoleTypes.Customer.DisplayName);
            }
            if (client.DisplayName == Clients.DriverApp.DisplayName)
            {
                role = await _roleManager.FindByNameAsync(RoleTypes.Driver.DisplayName);
            }
            if (role == null)
            {
                return _result.Failure(
                    ResponseCodes.InvalidClient,
                    StatusCodes.Status400BadRequest);
            }
            var user = await _userManager.FindByNameAsync(request.Email + client.DisplayName);
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidLoginCredentials, StatusCodes.Status401Unauthorized);
            }
            var canSignIn = await _signInManager.CanSignInAsync(user);
            if (!canSignIn || !user.IsActive)
            {
                return _result.Failure(ResponseCodes.UserLockedOrInactive, StatusCodes.Status401Unauthorized);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
            if (signInResult.IsLockedOut || signInResult.IsNotAllowed)
            {
                return _result.Failure(ResponseCodes.UserLockedOrInactive, StatusCodes.Status401Unauthorized);
            }
            if (!signInResult.Succeeded)
            {
                return _result.Failure(ResponseCodes.InvalidLoginCredentials, StatusCodes.Status401Unauthorized);
            }
            var accessFailedCountResult = await _userManager.ResetAccessFailedCountAsync(user);
            if (!accessFailedCountResult.Succeeded)
            {
                return _result.Failure(
                    ResponseCodes.UserActivationFailure,
                    StatusCodes.Status401Unauthorized,
                    reasons: accessFailedCountResult.Errors.Select(x => x.Description).ToList());
            }
            user.RegisterLastLogin();
            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded || identityResult.Errors.Any())
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await GenerateJwt(user, tokenHandler);

            return _result.Success(new AuthenticateUserResponse(tokenHandler.WriteToken(token), token.ValidTo));
        }

        public async Task<BaseResponse> AuthenticateAsync(long userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            var canSignIn = await _signInManager.CanSignInAsync(user);
            if (!canSignIn || !user.IsActive)
            {
                return _result.Failure(ResponseCodes.UserLockedOrInactive, StatusCodes.Status401Unauthorized);
            }
            await _signInManager.SignInAsync(user, false);
            user.RegisterLastLogin();
            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded || identityResult.Errors.Any())
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await GenerateJwt(user, tokenHandler);

            return _result.Success(new AuthenticateUserResponse(tokenHandler.WriteToken(token), token.ValidTo));
        }

        private async Task<SecurityToken> GenerateJwt(ApplicationUser user, JwtSecurityTokenHandler tokenHandler)
        {
            var key = Encoding.UTF8.GetBytes(Env.JwtSecret ?? _authConfig.JwtSecret);
            var expiry = DateTime.UtcNow.AddMinutes(30);
            var issuedAt = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Exp, expiry.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, Env.JwtValidIssuer ?? _authConfig.JwtValidIssuer),
                new Claim(JwtRegisteredClaimNames.Aud, Env.JwtValidAudience ?? _authConfig.JwtValidAudience),
                new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, issuedAt.ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = Env.JwtValidAudience ?? _authConfig.JwtValidAudience,
                IssuedAt = issuedAt,
                Expires = expiry,
                Issuer = Env.JwtValidIssuer ?? _authConfig.JwtValidIssuer,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public async Task<BaseResponse> RegisterAsync(CreateUserRequest request, Clients client)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.NullOrWhiteSpace(request.Email, nameof(request.Email));
            Guard.Against.NullOrWhiteSpace(request.Password, nameof(request.Password));
            Guard.Against.Null(client, nameof(client));
            ApplicationRole role = null;
            IdentityResult roleResult = null;
            if (client.DisplayName == Clients.CustomerApp.DisplayName)
            {
                role = await _roleManager.FindByNameAsync(RoleTypes.Customer.DisplayName);
                if (role == null)
                {
                    roleResult = await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = RoleTypes.Customer.DisplayName,
                        Description = RoleTypes.Customer.DisplayName
                    });
                }
            }
            if (client.DisplayName == Clients.DriverApp.DisplayName)
            {
                role = await _roleManager.FindByNameAsync(RoleTypes.Driver.DisplayName);
                if (role == null)
                {
                    roleResult = await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = RoleTypes.Driver.DisplayName,
                        Description = RoleTypes.Driver.DisplayName
                    });
                }
            }
            if (client.DisplayName == Clients.AdministratorApp.DisplayName)
            {
                role = await _roleManager.FindByNameAsync(RoleTypes.Driver.DisplayName);
                if (role == null)
                {
                    roleResult = await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = RoleTypes.Admin.DisplayName,
                        Description = RoleTypes.Admin.DisplayName
                    });
                }
            }
            if (roleResult != null && !roleResult.Succeeded)
            {
                return _result.Failure(
                        ResponseCodes.UserSignupFailure,
                        StatusCodes.Status400BadRequest,
                        reasons: roleResult.Errors.Select(x => x.Description).ToList());
            }
            role ??= await _roleManager.FindByNameAsync(RoleTypes.Customer.DisplayName);
            var user = await _userManager.FindByNameAsync(request.Email + client.DisplayName);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return _result.Failure(
                            ResponseCodes.EmailTaken,
                            StatusCodes.Status400BadRequest);
                }
                user.SavePassword(request.Password);
                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    return _result.Failure(
                        ResponseCodes.UserSignupFailure,
                        StatusCodes.Status400BadRequest,
                        reasons: roleResult.Errors.Select(x => x.Description).ToList());
                }
            }
            else
            {
                user = new ApplicationUser(
                    request.Email + client.DisplayName,
                    (request.Email + client.DisplayName).ToUpper(),
                    request.Email,
                    !request.ConfirmEmail,
                    false,
                    !request.ConfirmEmail
                );
                var userResult = await _userManager.CreateAsync(user, request.Password);
                if (!userResult.Succeeded)
                {
                    return _result.Failure(
                        ResponseCodes.UserSignupFailure,
                        StatusCodes.Status400BadRequest,
                        reasons: userResult.Errors.Select(x => x.Description).ToList());
                }
            }
            if (!request.ConfirmEmail && request.Roles != null && request.Roles.Any())
            {
                roleResult = await _userManager.AddToRolesAsync(user, request.Roles);
                if (!roleResult.Succeeded)
                {
                    return _result.Failure(
                        ResponseCodes.UserSignupFailure,
                        reasons: roleResult.Errors.Select(x => x.Description).ToList());
                }
            }
            return _result.Success<UserIdResponse>(
                new UserIdResponse(user.Id),
                statusCode: StatusCodes.Status201Created);
        }

        public async Task<BaseResponse> ActivateAsync(long applicationUserId, IEnumerable<string> roles)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == applicationUserId && !x.IsActive && !x.EmailConfirmed && x.LockoutEnabled);
            if (user == null)
            {
                _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            user.ToggleActiveStatus();
            user.ToggleEmailConfirmed();
            user.ToggleLockoutEnabled();
            var appRoles = new List<ApplicationRole>();
            foreach (var roleString in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleString);
                if (role != null) appRoles.Add(role);
            }
            user.AddToRoles(appRoles);
            var updateUserResult = await _userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                return _result.Failure(
                    ResponseCodes.UserActivationFailure,
                    reasons: updateUserResult.Errors.Select(x => x.Description).ToList());
            }
            return _result.Success();
        }

        public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordRequest request, Clients client)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.NullOrWhiteSpace(request.CurrentPassword, nameof(request.CurrentPassword));
            Guard.Against.NullOrWhiteSpace(request.NewPassword, nameof(request.NewPassword));
            Guard.Against.Null(client, nameof(client));
            var user = await _userManager.FindByNameAsync(request.Email + client.DisplayName);
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return _result.Failure(
                        ResponseCodes.UserSignupFailure,
                        reasons: changePasswordResult.Errors.Select(x => x.Description).ToList());
            }
            return _result.Success(statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request, Clients client)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.NullOrWhiteSpace(request.Email, nameof(request.Email));
            Guard.Against.NullOrWhiteSpace(request.Otp, nameof(request.Otp));
            Guard.Against.NullOrWhiteSpace(request.Password, nameof(request.Password));
            Guard.Against.Null(client, nameof(client));
            var user = await _userManager.FindByNameAsync(request.Email + client.DisplayName);
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            var otpResult = await VerifyOtp(user.Id, request.Otp);
            if (!otpResult.IsSuccess)
            {
                return otpResult;
            }
            user.SavePassword(request.Password);
            await _userManager.UpdateAsync(user);
            return _result.Success();
        }

        public BaseResponse GetUserIdFromClaims(ClaimsPrincipal claimsPrincipal)
        {
            Guard.Against.Null(claimsPrincipal, nameof(claimsPrincipal));
            var userId = long.Parse(_userManager.GetUserId(claimsPrincipal));
            return userId <= 0
                ? _result.Failure(ResponseCodes.InvalidUserAccount, StatusCodes.Status401Unauthorized)
                : _result.Success(new UserIdResponse(userId));
        }

        public async Task<BaseResponse> GetUserFromEmail(string email, Clients client)
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.Null(client, nameof(client));
            var user = await _userManager.FindByNameAsync(email + client);
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            return _result.Success(new UserResponse(user));
        }

        public async Task<BaseResponse> SendOtp(long userId, OtpPurpose otpPurpose)
        {
            Guard.Against.NegativeOrZero(userId, nameof(userId));
            Guard.Against.Null(otpPurpose, nameof(otpPurpose));
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            user.SaveOtp(_encryptionUtility, otpPurpose);
            var placeholders = new Dictionary<string, string>{
                    {Constants.NotificationTemplates.SignupOtp.Placeholders.OTP, user.Otp.GetOtpCode()}
                };
            var body = await _notificationService.PrepareTemplateAsync(Constants.NotificationTemplates.SignupOtp.SIGNUP_OTP_TEMPLATE, placeholders);
            var emailRequest = new EmailRequest(user.Email, otpPurpose.DisplayName, HtmlBody: body);
            _notificationService.SendSmtpEmail(emailRequest);
            await _userManager.UpdateAsync(user);
            _logger.LogInformation("OTP_GENERATED =>" + user.Email.ToString());
            return _result.Success();
        }

        public async Task<BaseResponse> SendOtp(string email, OtpPurpose otpPurpose, Clients client)
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.Null(otpPurpose, nameof(otpPurpose));
            Guard.Against.Null(client, nameof(client));
            var user = await _userManager.FindByNameAsync(email + client);
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            return await SendOtp(user.Id, otpPurpose);
        }

        public async Task<BaseResponse> VerifyOtp(long userId, string otp)
        {
            Guard.Against.NegativeOrZero(userId, nameof(userId));
            Guard.Against.NullOrWhiteSpace(otp, nameof(otp));
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            var otpResult = user.Otp.Verify(otp, _encryptionUtility);
            await _userManager.UpdateAsync(user);
            if (!otpResult.Item1)
            {
                return _result.Failure(ResponseCodes.InvalidOtp, reasons: new List<string> { otpResult.Item2 });
            }
            return _result.Success();
        }

        public async Task<BaseResponse> VerifyOtp(string email, string otp, Clients client)
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.NullOrWhiteSpace(otp, nameof(otp));
            Guard.Against.Null(client, nameof(client));
            var user = await _userManager.FindByNameAsync(email + client);
            if (user == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            return await VerifyOtp(user.Id, otp);
        }

        public async Task<BaseResponse> UpdatePhoneNumberAsync(long applicationUserId, string phoneNumber)
        {
            Guard.Against.NullOrEmpty(phoneNumber, nameof(phoneNumber));
            Guard.Against.NegativeOrZero(applicationUserId, nameof(applicationUserId));
            var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
            if (user == null)
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded || result.Errors.Any())
                return _result.Failure(ResponseCodes.GeneralError, reasons: result.Errors.Select(x => x.Description).ToList());
            return _result.Success();
        }
    }
}