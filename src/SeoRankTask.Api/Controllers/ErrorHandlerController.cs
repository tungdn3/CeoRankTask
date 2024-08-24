using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SeoRankTask.Api.Models;
using CeoRankTask.Core.Exceptions;

namespace SeoRankTask.Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorHandlerController : Controller
{
    private readonly ILogger<ErrorHandlerController> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public ErrorHandlerController(ILogger<ErrorHandlerController> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    [Route("/error")]
    public IActionResult HandleError()
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        _logger.LogError(exceptionHandlerFeature.Error, "An error has occurred.");

        InnerError? innerError = _hostEnvironment.IsDevelopment()
            ? new InnerError { Trace = exceptionHandlerFeature.Error.StackTrace }
            : null;

        if (exceptionHandlerFeature.Error is ValidationException validationException)
        {
            var error = new Error
            {
                Code = SeoRankTaskConstants.ErrorCodes.MalformedValue,
                Message = "Validation failed",
                Details = validationException.Errors.Select(x => new Error
                {
                    Code = x.ErrorCode,
                    Message = x.ErrorMessage,
                    Target = x.PropertyName,
                }),
                InnerError = innerError
            };

            return BadRequest(error);
        }

        // Other exceptions here
        // ...

        var unknownError = new Error
        {
            Code = "InternalServerError",
            Message = _hostEnvironment.IsDevelopment()
                ? exceptionHandlerFeature.Error.Message
                : "An error happened while processing the request.",
            InnerError = innerError,
        };

        return StatusCode((int)HttpStatusCode.InternalServerError, unknownError);
    }
}
