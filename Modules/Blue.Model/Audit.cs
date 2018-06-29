using System;
using Framework.Constract.Constant;
using Framework.Constract.SeedWork;

namespace Blue.Model
{
    public class Audit : BaseEntity
    {
        // A new SessionId that will be used to link an entire
        // users "Session" of Audit Logs together to help 
        // identifier patterns involving erratic behavior
        public string SessionId { get; set; }

        public string ExternalIpAddress { get; set; }

        public string UserName { get; set; }

        public string UrlAccessed { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public AudittingLevel AudittingLevel { get; set; }

        public int EventType { get; set; }

        // A new Data property that is going to store JSON 
        // string objects that will later be able to be 
        // deserialized into objects if necessary to view 
        // details about a Request
        public string Data { get; set; }

        public UserAction UserAction { get; set; }
    }
}
