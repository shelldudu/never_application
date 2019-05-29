using Never.Attributes;
using System;

namespace B2C.Admin.Web.Infrastructures
{
    /// <summary>
    /// 还款方式
    /// </summary>
    public enum RepayType : byte
    {
        /// <summary>
        /// 一次性还本付息
        /// </summary>
        [Remark(Name = "一次性还本付息")]
        一次性还本付息 = 1,

        /// <summary>
        /// 等额本息
        /// </summary>
        [Remark(Name = "等额本息")]
        等额本息 = 2,

        /// <summary>
        /// 等额本金
        /// </summary>
        [Remark(Name = "等额本金")]
        等额本金 = 3,

        /// <summary>
        /// 按期付息到期还本
        /// </summary>
        [Remark(Name = "按期付息到期还本")]
        按期付息到期还本 = 4,

        /// <summary>
        /// 等本等息
        /// </summary>
        [Remark(Name = "等本等息")]
        等本等息 = 5,
    }
}