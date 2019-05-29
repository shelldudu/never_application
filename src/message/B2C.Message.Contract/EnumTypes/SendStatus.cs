using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Message.Contract.EnumTypes
{
    /// <summary>
    /// 短信发送状态
    /// </summary>
    public enum SendStatus
    {
        /// <summary>
        /// 待发送
        /// </summary>
        待发送 = 0,

        /// <summary>
        /// 发送成功
        /// </summary>
        发送成功 = 1,

        /// <summary>
        /// 发送失败
        /// </summary>
        发送失败 = 2
    }
}