using AutoMapper;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using Construmart.Core.UseCases.AuthUseCases;
using Construmart.Core.UseCases.CustomerUseCases;

namespace Construmart.Core.Commons.ObjectMappers
{
    public class IdentityServiceMapper : Profile
    {
        public IdentityServiceMapper()
        {
            CreateMap<LoginCommand, AuthenticateUserRequest>();
            CreateMap<AuthenticateUserResponse, LoginResponse>();
            CreateMap<InitiateCustomerSignupCommand, CreateUserRequest>();
            CreateMap<ChangePasswordCommand, ChangePasswordRequest>();
            CreateMap<CompleteResetPasswordCommand, ResetPasswordRequest>();
        }
    }
}