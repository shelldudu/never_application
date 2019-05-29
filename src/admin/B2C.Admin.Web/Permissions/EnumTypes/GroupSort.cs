using Never.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2C.Admin.Web.Permissions.EnumTypes
{
    /// <summary>
    /// 级的分类
    /// </summary>
    public enum GroupSort
    {
        /// <summary>
        /// 麻瓜
        /// </summary>
        [Remark(Name = "公司职员")]
        Muggle = 0,

        /// <summary>
        /// 组长顶呱呱
        /// </summary>
        [Remark(Name = "部门负责人")]
        Leader = 8,

        /// <summary>
        /// 管理员一级棒
        /// </summary>
        [Remark(Name = "超级管理员")]
        Super = 1024
    }
}