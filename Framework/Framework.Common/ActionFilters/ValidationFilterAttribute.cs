using System.Net;
using Framework.Constract.Constant;
using Framework.Constract.SeedWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Framework.Common.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);

            if (context.Result is UnauthorizedResult)
            {
                var result = new RequestResult
                {
                    State = RequestState.NotAuth
                };

                var modelState = context.ModelState;

                foreach (var key in modelState.Keys)
                {
                    if (!modelState.TryGetValue(key, out var model)) continue;

                    foreach (var error in model.Errors)
                    {
                        result.ErrorMessages.Add(new ValidationError
                        {
                            Key = key,
                            Error = error.ErrorMessage
                        });
                    }
                }

                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                context.HttpContext.Response.ContentType = HttpContentType.ApplicationJson;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(result, jsonSerializerSettings));
            }
        }
    }
}
