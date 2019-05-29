using Never;
using Never.Commands;
using Never.CommandStreams;
using Never.DataAnnotations;
using B2C.Infrastructure;
using B2C.Message.Contract.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace B2C.Message.Contract.Commands
{
    /// <summary>
    /// 创建手机验证码命令
    /// </summary>
    [CommandDomain(Domain = "Message"), Serializable]
    [Validator(typeof(CommandValidator))]
    public class CreateMobileCodeCommand : Never.Domains.GuidAggregateCommand, IBehaviorPlatform
    {
        #region ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="CreateMobileCodeCommand"/> class from being created.
        /// </summary>
        private CreateMobileCodeCommand() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMobileCodeCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public CreateMobileCodeCommand(Guid aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public UsageType UsageType { get; set; }

        /// <summary>
        /// 验证码长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 生成客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 客户端平台
        /// </summary>
        public OperatePlatform Platform { get; set; }

        #endregion prop

        #region validator

        private class CommandValidator : Validator<CreateMobileCodeCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateMobileCodeCommand, object>>, string>> RuleFor(CreateMobileCodeCommand target)
            {
                if (target.Mobile.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateMobileCodeCommand, object>>, string>(model => model.Mobile, "手机为空");

                if (!target.Mobile.IsChineseMobile())
                    yield return new KeyValuePair<Expression<Func<CreateMobileCodeCommand, object>>, string>(model => model.Mobile, "手机号码无法识别");
            }
        }

        #endregion validator
    }
}