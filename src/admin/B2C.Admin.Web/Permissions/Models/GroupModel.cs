using B2C.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Models
{
    public class GroupModel : _AggregateModel<string>
    {
        #region prop

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get;  set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        public string Descn { get;  set; }

        #endregion prop
    }
}
