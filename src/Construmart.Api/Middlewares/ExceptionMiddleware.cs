using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Construmart.Core.Commons;
using Construmart.Core.DTOs.Response;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Construmart.Api.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionMiddleware
    {
        private static string responseJson;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="logger"></param>
        /// <param name="responseUtility"></param>
        public static void UseAppExceptionHandler(this IApplicationBuilder app, ILogger logger, IResult responseUtility)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        if (contextFeature.Error is ValidationException validationException)
                        {
                            responseJson = WriteValidationExceptionAsync(validationException, context, responseUtility);
                        }
                        else
                        {
                            responseJson = WriteGenericExceptionAsync(contextFeature.Error, context, responseUtility);
                        }
                        await context.Response.WriteAsync(responseJson);
                    }
                });
            });
        }

        private static string WriteValidationExceptionAsync(
            ValidationException exception,
            HttpContext context,
            IResult responseUtility
        )
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var errorMessages = new List<string>();
            foreach (var error in exception.Errors)
            {
                errorMessages.Add(error.ErrorMessage);
            }
            var response = responseUtility.Failure(ResponseCodes.RequestValidationFailure, reasons: errorMessages);
            var responseJson = JsonSerializer.Serialize<BaseResponse>(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return responseJson;
        }

        private static string WriteGenericExceptionAsync(
            Exception exception,
            HttpContext context,
            IResult responseUtility
        )
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = responseUtility.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            response.Error.Reasons.Add(exception.Message);
            var responseJson = JsonSerializer.Serialize<BaseResponse>(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return responseJson;
        }
    }
}