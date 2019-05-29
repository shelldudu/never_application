using Never.Security;
using B2C.Infrastructure;
using B2C.Admin.Web.Permissions.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Models
{
    public class EmployeeModel : _AggregateModel<string>
    {
        #region prop

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名字
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 级的分类，分配的时候不能越级
        /// </summary>
        public GroupSort GroupSort { get; set; }

        #endregion prop

        #region resource

        /// <summary>
        /// 所有的资源
        /// </summary>
        public IEnumerable<ActionDesciptor> Resources { get; set; }

        #endregion resource
    }
}