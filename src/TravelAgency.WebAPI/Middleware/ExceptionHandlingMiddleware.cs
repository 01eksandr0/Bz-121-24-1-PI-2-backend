using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using TravelAgency.BLL.Exceptions;

namespace TravelAgency.WebAPI.Middleware
{
    /// <summary>
    /// Єдине місце обробки винятків: бізнес-винятки перетворюються на відповідні HTTP-коди
    /// у форматі ProblemDetails (RFC 7807), а неочікувані - логуються і повертаються як 500 без деталей.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private const string ProblemContentType = "application/problem+json";
        private const string InternalErrorMessage = "Внутрішня помилка сервера. Спробуйте пізніше.";

        private static readonly Dictionary<Type, int> StatusByException = new Dictionary<Type, int>
        {
            { typeof(BusinessValidationException), StatusCodes.Status400BadRequest },
            { typeof(InvalidCredentialsException), StatusCodes.Status401Unauthorized },
            { typeof(AccessDeniedException), StatusCodes.Status403Forbidden },
            { typeof(EntityNotFoundException), StatusCodes.Status404NotFound },
            { typeof(BusinessConflictException), StatusCodes.Status409Conflict }
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException exception)
            {
                int status;
                if (!StatusByException.TryGetValue(exception.GetType(), out status))
                {
                    status = StatusCodes.Status400BadRequest;
                }
                await WriteProblemAsync(context, status, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
                await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, InternalErrorMessage);
            }
        }

        private static Task WriteProblemAsync(HttpContext context, int status, string detail)
        {
            var problem = new ProblemDetails
            {
                Status = status,
                Title = ReasonPhrases.GetReasonPhrase(status),
                Detail = detail,
                Instance = context.Request.Path
            };
            context.Response.StatusCode = status;
            return context.Response.WriteAsJsonAsync(problem, (System.Text.Json.JsonSerializerOptions)null, ProblemContentType);
        }
    }
}
