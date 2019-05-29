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
    public class RemoveGroupCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveGroupCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public RemoveGroupCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveGroupCommand"/> class.
        /// </summary>
        private RemoveGroupCommand()
            : this(string.Empty)
        {
        }

        #endregion ctor

        #region vaidator

        private class CommandValidator : Validator<RemoveGroupCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<RemoveGroupCommand, object>>, string>> RuleFor(RemoveGroupCommand target)
            {
                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RemoveGroupCommand, object>>, string>(m => m.AggregateId, "组Id为空");
            }
        }

        #endregion vaidator
    }
}