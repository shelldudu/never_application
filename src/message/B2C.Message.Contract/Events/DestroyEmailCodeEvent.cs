using Never.Events;
using Never.EventStreams;
using B2C.Message.Contract.EnumTypes;
using System;

namespace B2C.Message.Contract.Events
{
    /// <summary>
    /// 销毁邮箱验证码事件
    /// </summary>
    [Serializable, EventDomain(Domain = "Message")]
    public class DestroyEmailCodeEvent : AggregateRootChangeEvent<Guid>
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public UsageType UsageType { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public UsageStatus UsageStatus { get; set; }
    }
}