using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Message.Contract.EnumTypes
{
    /// <summary>
    /// 使用状态
    /// </summary>
    public enum UsageStatus : byte
    {
        /// <summary>
        /// The 未使用
        /// </summary>
        未使用 = 0,

        /// <summary>
        /// The 已使用
        /// </summary>
        已使用 = 1
    }
}