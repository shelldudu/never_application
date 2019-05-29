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
    /// <summary>
    /// 删除用户命令
    /// </summary>
    [Serializable, CommandDomain(Domain = "admin")]
    [Validator(typeof(CommandValidator))]
    public class RemoveEmployeeCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveEmployeeCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public RemoveEmployeeCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveEmployeeCommand"/> class.
        /// </summary>
        private RemoveEmployeeCommand()
            : this(string.Empty)
        {
        }

        #endregion ctor

        #region vaidator

        private class CommandValidator : Validator<RemoveEmployeeCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<RemoveEmployeeCommand, object>>, string>> RuleFor(RemoveEmployeeCommand target)
            {
                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RemoveEmployeeCommand, object>>, string>(m => m.AggregateId, "会员Id为空");
            }
        }

        #endregion vaidator
    }
}