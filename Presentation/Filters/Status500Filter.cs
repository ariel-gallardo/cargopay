using System.Text.Json;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation
{
    public class Status500Filter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception.InnerException != null
                ? context.Exception.InnerException.Message
                : context.Exception.Message;

            var errorDetails = new
            {
                Exception = exception,
                Stack = !string.IsNullOrEmpty(context.Exception.StackTrace)
                    ? context.Exception.StackTrace
                    : null
            };

            var errorDetails2 = new
            {
                Exception = exception
            };


            context.Result = new JsonResult(
                new CustomResponse
                {
                    Data = null,
                    Message = $"APPLICATION_ERROR | {JsonSerializer.Serialize(errorDetails2)}",
                    StatusCode = StatusCodes.Status500InternalServerError
                }
            )
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }
}