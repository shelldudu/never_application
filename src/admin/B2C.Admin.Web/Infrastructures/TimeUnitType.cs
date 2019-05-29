using Never.Attributes;
using System;

namespace B2C.Admin.Web.Infrastructures
{
    /// <summary>
    /// 时间单位
    /// </summary>
    public enum TimeUnitType : byte
    {
        /// <summary>
        /// 天标
        /// </summary>
        [Remark(Name = "天")]
        天 = 1,

        /// <summary>
        /// 月标
        /// </summary>
        [Remark(Name = "月")]
        月 = 2,

        /// <summary>
        /// 季标
        /// </summary>
        [Remark(Name = "季")]
        季 = 3,

        /// <summary>
        /// 年标
        /// </summary>
        [Remark(Name = "年")]
        年 = 4
    }
}