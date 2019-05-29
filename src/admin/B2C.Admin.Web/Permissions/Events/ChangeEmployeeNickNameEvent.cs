using Never.EventStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class ChangeEmployeeNickNameEvent : Never.Events.AggregateRootChangeEvent<string>
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}