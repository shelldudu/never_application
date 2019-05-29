using Never;
using Never.Commands;
using Never.CommandStreams;
using Never.DataAnnotations;
using B2C.Message.Contract.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace B2C.Message.Contract.Commands
{
    /// <summary>
    /// 销毁手机验证码命令
    /// </summary>
    [CommandDomain(Domain = "Message"), Serializable]
    [Validator(typeof(CommandValidator))]
    public class DestroyMobileCodeCommand : Never.Domains.GuidAggregateCommand
    {
        #region ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="DestroyMobileCodeCommand"/> class from being created.
        /// </summary>
        private DestroyMobileCodeCommand() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DestroyMobileCodeCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public DestroyMobileCodeCommand(Guid aggregateId) : base(aggregateId)
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
        /// 验证码
        /// </summary>
        public string VCode { get; set; }

        #endregion prop

        #region validator

        private class CommandValidator : Validator<DestroyMobileCodeCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<DestroyMobileCodeCommand, object>>, string>> RuleFor(DestroyMobileCodeCommand target)
            {
                if (target.Mobile.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<DestroyMobileCodeCommand, object>>, string>(model => model.Mobile, "手机为空");

                if (!target.Mobile.IsChineseMobile())
                    yield return new KeyValuePair<Expression<Func<DestroyMobileCodeCommand, object>>, string>(model => model.Mobile, "手机号码无法识别");

                if (target.VCode.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<DestroyMobileCodeCommand, object>>, string>(model => model.Mobile, "验证码为空");
            }
        }

        #endregion validator
    }
}