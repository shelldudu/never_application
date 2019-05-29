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
    public class ChangeEmployeeOwnerEvent : Never.Events.AggregateRootChangeEvent<string>
    {
        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 级的分类，分配的时候不能越级
        /// </summary>
        public GroupSort GroupSort { get; set; }
    }
}