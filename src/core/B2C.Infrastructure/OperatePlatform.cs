using Never.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 操作平台
    /// </summary>
    public enum OperatePlatform : byte
    {
        /// <summary>
        /// PC
        /// </summary>
        [Remark(Name = "PC")]
        PC = 1,

        /// <summary>
        /// Android
        /// </summary>
        [Remark(Name = "Android")]
        Android = 2,

        /// <summary>
        /// Apple
        /// </summary>
        [Remark(Name = "Apple")]
        Apple = 4,

        /// <summary>
        /// WX
        /// </summary>
        [Remark(Name = "WX")]
        WX = 8,

        /// <summary>
        /// Wap
        /// </summary>
        [Remark(Name = "Wap")]
        Wap = 16
    }
}
