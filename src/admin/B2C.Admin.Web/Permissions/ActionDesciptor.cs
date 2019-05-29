using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions
{
    /// <summary>
    /// action节点描述
    /// </summary>
    public struct ActionDesciptor
    {
        #region prop

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 行为
        /// </summary>
        public string ActionResult { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 聚合Id
        /// </summary>
        public string AggregateId { get; set; }

        #endregion prop
    }
}