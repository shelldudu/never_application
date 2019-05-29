using B2C.Message.Contract.EnumTypes;
using B2C.Infrastructure;
using System;

namespace B2C.Message.Contract.Models
{
    /// <summary>
    /// 手机验证码
    /// </summary>
    public class EmailCodeModel : _AggregateModel<Guid>
    {
        #region prop

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get;  set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public UsageType UsageType { get;  set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Message { get;  set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get;  set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public UsageStatus UsageStatus { get;  set; }

        /// <summary>
        /// 生成客户端IP
        /// </summary>
        public string ClientIP { get;  set; }

        /// <summary>
        /// 客户端平台
        /// </summary>
        public OperatePlatform Platform { get;  set; }

        #endregion prop
    }
}