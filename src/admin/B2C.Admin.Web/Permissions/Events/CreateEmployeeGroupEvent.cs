using Never.EventStreams;
using Never.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class CreateEmployeeGroupEvent : Never.Events.AggregateRootCreateEvent<string>
    {
        public string EmployeeId { get; set; }

        public string GroupId { get; set; }
    }
}