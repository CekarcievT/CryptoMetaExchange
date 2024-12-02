using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;
using Shared.Helpers;

namespace BSDigitalPart2.Infrastructure
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string message = context.Exception.InnerException == null ? context.Exception.Message : context.Exception.InnerException.Message;
            var errorCodes = EnumHelper.GetKeyValuePairsFromEnum<ErrorCodes>();
            if (errorCodes.Any(error => error.name == message))
            {
                JsonDataResult<object> jsonDataResult = new JsonDataResult<object>();
                jsonDataResult.success = false;
                jsonDataResult.data = null;
                jsonDataResult.errors = new List<string>() { EnumHelper.GetDescription<ErrorCodes>(message)};
                context.Result = new JsonResult(jsonDataResult);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                JsonDataResult<object> jsonDataResult = new JsonDataResult<object>();
                jsonDataResult.success = false;
                jsonDataResult.data = null;
                jsonDataResult.errors = new List<string>() { $"Unexpected error occurred. {message}" };
                context.Result = new JsonResult(jsonDataResult);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            base.OnException(context);
        }
    }
}
