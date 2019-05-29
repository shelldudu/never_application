using Never;
using Never.CommandStreams;
using Never.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Commands
{
    [Serializable, CommandDomain(Domain = "admin")]
    [Validator(typeof(CommandValidator))]
    public class ChangeEmployeePasswordCommand : Never.Domains.StringAggregateCommand
    {
        #region prop

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmployeePasswordCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public ChangeEmployeePasswordCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmployeePasswordCommand"/> class.
        /// </summary>
        protected ChangeEmployeePasswordCommand()
            : this(string.Empty)
        {
        }

        #endregion ctor

        #region vaidator

        private class CommandValidator : Validator<ChangeEmployeePasswordCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ChangeEmployeePasswordCommand, object>>, string>> RuleFor(ChangeEmployeePasswordCommand target)
            {
                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeEmployeePasswordCommand, object>>, string>(m => m.Password, "密码为空");

                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeEmployeePasswordCommand, object>>, string>(m => m.AggregateId, "聚合Id为空");
            }
        }

        #endregion vaidator
    }
}