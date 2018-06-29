using System;
using Blue.Constract.Dtos;
using Blue.Data.Models;
using Framework.Constract.Constant;
using Framework.Constract.Interfaces;
using Framework.Data.SeedWork;
using IdentityServer4.Events;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Blue.DomainService
{
    public interface IAuditService : IService<AuditTrail, AuditDto>
    {
        void AddAuthenticationEvent(Event evt);
    }

    public class AuditService : Service<AuditTrail, AuditDto>, IAuditService
    {
        private readonly IRepository<AuditTrail> _auditRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(
            unitOfWork, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditRepository = unitOfWork.Repository<AuditTrail>();
        }

        public void AddAuthenticationEvent(Event evt)
        {
            var audit = new AuditDto
            {
                ExternalIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                TimeStamp = DateTimeOffset.Now,
                CreatedBy = UserType.SystemGenerated,
                AudittingLevel = (int) AudittingLevel.Middle,
                UserAction = UserAction.Login,
                EventType = (int) evt.EventType
            };

            var castMethod = GetType().GetMethod("Cast").MakeGenericMethod(evt.GetType());
            var castedObject = castMethod.Invoke(null, new object[] { evt });
            audit.Data = JsonConvert.SerializeObject(castedObject);
            audit.UserName = castedObject is UserLoginSuccessEvent @event ? @event.Username : null;

            Add(audit);
        }

        public static T Cast<T>(object o)
        {
            return (T) o;
        }


    }
}
