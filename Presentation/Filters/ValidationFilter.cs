using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = new CustomResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $@"VALIDATION_ERROR ""{string.Join("|", context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))}"""
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
