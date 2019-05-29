using Never.Events;
using Never.EventStreams;
using B2C.Message.Contract.EnumTypes;
using B2C.Infrastructure;
using System;

namespace B2C.Message.Contract.Events
{
    /// <summary>
    /// 创建短信验证码事件
    /// </summary>
    [Serializable, EventDomain(Domain = "Message")]
    public class CreateMobileCodeEvent : AggregateRootCreateEvent<Guid>, IBehaviorPlatform
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public long Mobile { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public UsageType UsageType { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 生成客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 客户端平台
        /// </summary>
        public OperatePlatform Platform { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}