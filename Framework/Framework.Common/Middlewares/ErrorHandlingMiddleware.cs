using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Framework.Constract.SeedWork;
using Framework.Constract.Constant;
using Newtonsoft.Json.Serialization;

namespace Framework.Common.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //TODO: Handle Custom Exceptions here
            //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (exception is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException) code = HttpStatusCode.BadRequest;

            var result = new RequestResult
            {
                State = RequestState.Failed
            };

            result.ErrorMessages.Add(new ValidationError
            {
                Error = exception.Message,
                InnerException = exception.InnerException?.Message,
                StackTrace = exception.StackTrace
            });

            context.Response.ContentType = HttpContentType.ApplicationJson;
            context.Response.StatusCode = (int)code;

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(result, jsonSerializerSettings));
        }
    }
}
