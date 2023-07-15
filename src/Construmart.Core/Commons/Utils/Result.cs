using System.Collections.Generic;
using Construmart.Core.Commons.Utils;
using Construmart.Core.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.Commons.Utils
{
    public class Result : IResult
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Result(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public BaseResponse Failure(
            ResponseCodes responseCode,
            int statusCode = StatusCodes.Status400BadRequest,
            string responseDescription = null,
            IList<string> reasons = default)
        {
            return new BaseResponse
            (
                false,
                new ErrorResponse
                (
                    _httpContextAccessor.HttpContext.Request.Method,
                    _httpContextAccessor.HttpContext.Request.Path,
                    responseCode.GetCode(),
                    string.IsNullOrEmpty(responseDescription) ? responseCode.GetDescription() : responseDescription,
                    reasons ?? new List<string>()
                ),
                statusCode
            );
        }

        public ServiceResponse<T> Success<T>(T payload = default, int statusCode = StatusCodes.Status200OK) where T : class
        {
            return new ServiceResponse<T>(true, statusCode, payload);
        }

        public ServiceResponse<object> Success(int statusCode = StatusCodes.Status200OK)
        {
            return new ServiceResponse<object>(true, statusCode, default);
        }
    }
}