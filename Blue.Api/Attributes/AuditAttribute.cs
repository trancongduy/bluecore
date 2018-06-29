using System;
using Framework.Constract.Constant;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Blue.Constract.Dtos;
using Blue.DomainService;

namespace Blue.Api.Attributes
{
    public class AuditAttribute : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditService _auditService;
        private readonly int _audittingLevel; 

        public AuditAttribute(IHttpContextAccessor httpContextAccessor, IAuditService auditService, int audittingLevel)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditService = auditService;
            _audittingLevel = audittingLevel;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            var audit = new AuditDto
            {
                UserName = _httpContextAccessor.HttpContext.User.IsAuthenticated()
                    ? _httpContextAccessor.HttpContext.User.Identity.Name
                    : UserType.Anonymous,
                UrlAccessed = request.GetDisplayUrl(),
                ExternalIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                TimeStamp = DateTimeOffset.Now,
                CreatedBy = UserType.SystemGenerated,
                AudittingLevel = _audittingLevel,
                Data = SerializeRequest(request)
            };

            _auditService.Add(audit);

            base.OnActionExecuting(context);
        }

        // This will serialize the Request object based on the 
        // level that you determine
        private string SerializeRequest(HttpRequest request)
        {
            switch (_audittingLevel)
            {
                // No Request Data will be serialized
                default:
                    return null;
                // Basic Request Serialization - just stores Data
                case 1:
                    return JsonConvert.SerializeObject(new { request.Cookies, request.Headers });
                // Middle Level - Customize to your Preferences
                case 2:
                    return JsonConvert.SerializeObject(new { request.Cookies, request.Headers, request.Query, request.QueryString });
                // Highest Level - Serialize the entire Request object (As mentioned earlier, this will blow up)
                case 3:
                    // We can't simply just Encode the entire 
                    // request string due to circular references 
                    // as well as objects that cannot "simply" 
                    // be serialized such as Streams, References etc.
                    //return JsonConvert.SerializeObject(request);
                    return JsonConvert.SerializeObject(new { request.Cookies, request.Headers, request.Form, request.QueryString });
            }
        }
    }
}
