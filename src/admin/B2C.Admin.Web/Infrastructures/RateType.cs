using Never.Attributes;
using System;

namespace B2C.Admin.Web.Infrastructures
{
    /// <summary>
    /// 利率单位
    /// </summary>
    public enum RateType : byte
    {
        /// <summary>
        /// 天利率
        /// </summary>
        [Remark(Name = "天利率")]
        天 = 4,

        /// <summary>
        /// 月利率
        /// </summary>
        [Remark(Name = "月利率")]
        月 = 3,

        /// <summary>
        /// 季利率
        /// </summary>
        [Remark(Name = "季利率")]
        季 = 2,

        /// <summary>
        /// 年利率
        /// </summary>
        [Remark(Name = "年利率")]
        年 = 1
    }
}