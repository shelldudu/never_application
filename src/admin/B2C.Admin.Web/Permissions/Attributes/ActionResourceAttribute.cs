using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Attributes
{
    /// <summary>
    /// 权限资源描述
    /// </summary>
    /// <remarks>这里设计了order这个属性，是因为要确保第个action生成唯一的id,而用gethashcode不能确保第个进程生成的值一样，所以目前只能手动加入这属性</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ActionResourceAttribute : Attribute
    {
        #region prop

        /// <summary>
        /// 排序
        /// </summary>
        public string AggregateId { get; set; }

        /// <summary>
        /// action名称
        /// </summary>
        public string ActionDescn { get; set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionResourceAttribute"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="actionDescn">The action descn.</param>
        public ActionResourceAttribute(string aggregateId, string actionDescn)
        {
            this.AggregateId = aggregateId;
            this.ActionDescn = actionDescn;
        }

        #endregion ctor
    }
}