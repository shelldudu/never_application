using Never.EventStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class ChangeEmployeePasswordEvent : Never.Events.AggregateRootChangeEvent<string>
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}