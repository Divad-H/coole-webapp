using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CooleWebapp.Core.ErrorHandling;
using System.Net;

namespace CooleWebapp.Backend.ErrorHandling;

public record ErrorData
{
  public string Message { get; set; } = string.Empty;
}

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
  public int Order => int.MaxValue - 10;

  public void OnActionExecuting(ActionExecutingContext context) { }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.Exception is ClientError httpResponseException)
    {
      context.Result = new ObjectResult(new ErrorData() { Message = httpResponseException.Message })
      {
        StatusCode = httpResponseException.Type switch 
        { 
          ErrorType.InvalidOperation => (int)HttpStatusCode.BadRequest,
          ErrorType.Forbidden => (int)HttpStatusCode.Forbidden,
          ErrorType.Unauthorized => (int)HttpStatusCode.Unauthorized,
          ErrorType.NotFound => (int)HttpStatusCode.NotFound, 
          _ => (int)HttpStatusCode.InternalServerError
        }
      };

      context.ExceptionHandled = true;
    }
  }
}
