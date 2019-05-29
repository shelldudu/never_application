using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 执行结果
    /// </summary>
    public enum OperateResult
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 已存在
        /// </summary>
        Existed = 5,

        /// <summary>
        /// 不存在
        /// </summary>
        NotFound = 6,

        /// <summary>
        /// 等待中,用于重试
        /// </summary>
        Waiting = 10,
    }
}
