using Never.EventStreams;
using B2C.Admin.Web.Permissions.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Events
{
    /// <summary>
    ///
    /// </summary>
    [Serializable, EventDomain(Domain = "admin")]
    public class CreateResourceEvent : Never.Events.AggregateRootCreateEvent<string>
    {
        public string ActionDescn { get; set; }
        public GroupSort GroupSort { get; internal set; }
    }
}