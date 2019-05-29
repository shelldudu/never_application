using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2C.Admin.Web.Permissions.Attributes
{
    /// <summary>
    /// 分配权限功能的资源描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SuperActionResourceAttribute : ActionResourceAttribute
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperActionResourceAttribute"/> class.
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="actionDescn"></param>
        public SuperActionResourceAttribute(string aggregateId, string actionDescn) : base(aggregateId, actionDescn)
        {
        }

        #endregion ctor
    }
}