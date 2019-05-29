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
    public class CreateGroupCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        private CreateGroupCommand() : this(string.Empty)
        {
        }

        public CreateGroupCommand(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descn { get; set; }

        #endregion prop

        #region validator

        private class CommandValidator : Validator<CreateGroupCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateGroupCommand, object>>, string>> RuleFor(CreateGroupCommand target)
            {
                if (target.Name.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateGroupCommand, object>>, string>(model => model.Name, "名称为空");

                if (target.Descn.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateGroupCommand, object>>, string>(model => model.Descn, "描述为空");

                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateGroupCommand, object>>, string>(m => m.AggregateId, "聚合Id为空");

                if (target.DepartmentId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateGroupCommand, object>>, string>(m => m.AggregateId, "部门Id为空");
            }
        }

        #endregion validator
    }
}