using System;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;

namespace Blue.DomainService
{
    public class AuditEventService : IEventService
    {
        private readonly IAuditService _auditService;

        public AuditEventService(IAuditService auditService)
        {
            _auditService = auditService;
        }

        public Task RaiseAsync(Event evt)
        {
            if (evt == null) return Task.FromResult(0);
            
            _auditService.AddAuthenticationEvent(evt);

            return Task.FromResult(0);
        }

        public bool CanRaiseEventType(EventTypes evtType)
        {
            throw new NotImplementedException();
        }
    }
}
