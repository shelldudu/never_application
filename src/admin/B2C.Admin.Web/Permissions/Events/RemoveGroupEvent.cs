using Never.EventStreams;
using B2C.Admin.Web.Permissions.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class RemoveGroupEvent : Never.Events.AggregateRootDeleteEvent<string>
    {
        #region prop

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentId { get; set; }

        #endregion
    }
}