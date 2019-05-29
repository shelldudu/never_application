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
    public class ChangeEmployeeNickNameCommand : Never.Domains.StringAggregateCommand
    {
        #region prop

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmployeeNickNameCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public ChangeEmployeeNickNameCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmployeeNickNameCommand"/> class.
        /// </summary>
        protected ChangeEmployeeNickNameCommand()
            : this(string.Empty)
        {
        }

        #endregion ctor

        #region vaidator

        private class CommandValidator : Validator<ChangeEmployeeNickNameCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ChangeEmployeeNickNameCommand, object>>, string>> RuleFor(ChangeEmployeeNickNameCommand target)
            {
                if (target.NickName.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeEmployeeNickNameCommand, object>>, string>(m => m.NickName, "用户昵称为空");

                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeEmployeeNickNameCommand, object>>, string>(m => m.AggregateId, "聚合Id为空");
            }
        }

        #endregion vaidator
    }
}