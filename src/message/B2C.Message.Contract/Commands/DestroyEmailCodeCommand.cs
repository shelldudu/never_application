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
    /// 销毁邮箱验证码命令
    /// </summary>
    [CommandDomain(Domain = "Message"), Serializable]
    [Validator(typeof(CommandValidator))]
    public class DestroyEmailCodeCommand : Never.Domains.GuidAggregateCommand
    {
        #region ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="DestroyEmailCodeCommand"/> class from being created.
        /// </summary>
        private DestroyEmailCodeCommand() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DestroyEmailCodeCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public DestroyEmailCodeCommand(Guid aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

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
        public string VCode { get; set; }

        #endregion prop

        #region validator

        private class CommandValidator : Validator<DestroyEmailCodeCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<DestroyEmailCodeCommand, object>>, string>> RuleFor(DestroyEmailCodeCommand target)
            {
                if (target.Email.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<DestroyEmailCodeCommand, object>>, string>(model => model.Email, "邮箱为空");

                if (!target.Email.IsEmail())
                    yield return new KeyValuePair<Expression<Func<DestroyEmailCodeCommand, object>>, string>(model => model.Email, "邮箱无法识别");

                if (target.VCode.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<DestroyEmailCodeCommand, object>>, string>(model => model.VCode, "验证码为空");
            }
        }

        #endregion validator
    }
}