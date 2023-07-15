using System.Collections.Generic;
using Construmart.Core.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.Commons
{
    public interface IResult
    {
        BaseResponse Failure(
            ResponseCodes responseCode,
            int statusCode = StatusCodes.Status400BadRequest,
            string responseDescription = default,
            IList<string> reasons = default);

        ServiceResponse<T> Success<T>(
            T payload = default,
            int statusCode = StatusCodes.Status200OK) where T : class;

        ServiceResponse<object> Success(int statusCode = StatusCodes.Status200OK);
    }
}