using System.Text.Json;
using System.Text.Json.Serialization;
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
                    Message = $@"VALIDATION_ERROR | {JsonSerializer.Serialize(context.ModelState
                    .Where(kv => kv.Value.Errors.Any())
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    ))}"

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