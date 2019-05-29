using Never.EventStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class RemoveDepartmentEvent : Never.Events.AggregateRootDeleteEvent<string>
    {
    }
}