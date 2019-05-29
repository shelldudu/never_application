using Never;
using Never.CommandStreams;
using Never.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace B2C.Admin.Web.Permissions.Commands
{
    [Serializable, CommandDomain(Domain = "admin")]
    [Validator(typeof(CommandValidator))]
    public class RemoveDepartmentCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveDepartmentCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public RemoveDepartmentCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveDepartmentCommand"/> class.
        /// </summary>
        private RemoveDepartmentCommand() : this(string.Empty)
        {
        }

        #endregion ctor

        #region validator

        private class CommandValidator : Validator<RemoveDepartmentCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<RemoveDepartmentCommand, object>>, string>> RuleFor(RemoveDepartmentCommand target)
            {
                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RemoveDepartmentCommand, object>>, string>(m => m.AggregateId, "部门Id为空");
            }
        }

        #endregion validator
    }
}