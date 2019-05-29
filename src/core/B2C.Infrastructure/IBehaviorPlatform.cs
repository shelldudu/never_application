using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 客户端行为接口
    /// </summary>
    public interface IBehaviorPlatform
    {
        /// <summary>
        /// 获取该客户端行为接口
        /// </summary>
        OperatePlatform Platform { get; }
    }
}
