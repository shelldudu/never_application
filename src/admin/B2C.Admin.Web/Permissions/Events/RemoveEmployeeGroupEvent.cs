using Never.EventStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class RemoveEmployeeGroupEvent : Never.Events.AggregateRootDeleteEvent<string>
    {
        public string EmployeeId { get; set; }

        public string GroupId { get; set; }
    }
}