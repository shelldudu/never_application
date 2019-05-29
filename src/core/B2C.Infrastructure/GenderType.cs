using Never.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum GenderType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Remark(Name = "未知")]
        未知 = 0,

        /// <summary>
        /// 男
        /// </summary>
        [Remark(Name = "男")]
        男 = 1,

        /// <summary>
        /// 女
        /// </summary>
        [Remark(Name = "女")]
        女 = 2,
    }
}