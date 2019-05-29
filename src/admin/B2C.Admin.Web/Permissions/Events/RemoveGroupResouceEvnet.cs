using Never.EventStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class RemoveGroupResouceEvnet : Never.Events.AggregateRootCreateEvent<string>
    {
        #region prop

        /// <summary>
        /// 组Id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public string ResourceId { get; set; }

        #endregion prop
    }
}
