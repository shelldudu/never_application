using Never;
using Never.CommandStreams;
using Never.DataAnnotations;
using B2C.Admin.Web.Permissions.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace B2C.Admin.Web.Permissions.Commands
{
    [Serializable, CommandDomain(Domain = "admin")]
    [Validator(typeof(CommandValidator))]
    public class ChangeEmployeeOwnerCommand : Never.Domains.StringAggregateCommand
    {
        #region prop

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 级的分类，分配的时候不能越级
        /// </summary>
        public GroupSort GroupSort { get; set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmployeeOwnerCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public ChangeEmployeeOwnerCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmployeeOwnerCommand"/> class.
        /// </summary>
        private ChangeEmployeeOwnerCommand()
            : this(string.Empty)
        {
        }

        #endregion ctor

        #region vaidator

        private class CommandValidator : Validator<ChangeEmployeeOwnerCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ChangeEmployeeOwnerCommand, object>>, string>> RuleFor(ChangeEmployeeOwnerCommand target)
            {
                if (target.DepartmentId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeEmployeeOwnerCommand, object>>, string>(m => m.DepartmentId, "部门为空");

                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeEmployeeOwnerCommand, object>>, string>(m => m.AggregateId, "聚合Id为空");
            }
        }

        #endregion vaidator
    }
}