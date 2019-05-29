using Never;
using Never.CommandStreams;
using Never.DataAnnotations;
using B2C.Infrastructure;
using B2C.Message.Contract.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Message.Contract.Commands
{
    /// <summary>
    /// 创建邮箱验证码命令
    /// </summary>
    [CommandDomain(Domain = "Message"), Serializable]
    [Validator(typeof(CommandValidator))]
    public class CreateEmailCodeCommand : Never.Domains.GuidAggregateCommand, IBehaviorPlatform
    {
        #region ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="CreateEmailCodeCommand"/> class from being created.
        /// </summary>
        private CreateEmailCodeCommand() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEmailCodeCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public CreateEmailCodeCommand(Guid aggregateId) : base(aggregateId)
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

        private class CommandValidator : Validator<CreateEmailCodeCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateEmailCodeCommand, object>>, string>> RuleFor(CreateEmailCodeCommand target)
            {
                if (target.Email.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateEmailCodeCommand, object>>, string>(model => model.Email, "邮箱为空");

                if (!target.Email.IsEmail())
                    yield return new KeyValuePair<Expression<Func<CreateEmailCodeCommand, object>>, string>(model => model.Email, "邮箱无法识别");
            }
        }

        #endregion validator
    }
}