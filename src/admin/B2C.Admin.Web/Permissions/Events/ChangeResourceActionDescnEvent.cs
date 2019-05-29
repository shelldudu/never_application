using Never.EventStreams;
using B2C.Admin.Web.Permissions.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class ChangeResourceActionDescnEvent : Never.Events.AggregateRootChangeEvent<string>
    {
        public string Descn { get; set; }
        public GroupSort GroupSort { get; internal set; }
    }
}