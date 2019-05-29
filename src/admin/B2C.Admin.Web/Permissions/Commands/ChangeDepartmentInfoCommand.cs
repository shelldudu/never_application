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
    public class ChangeDepartmentInfoCommand : Never.Domains.ChangeAggregateCommand<string>
    {
        #region ctor

        private ChangeDepartmentInfoCommand() : this(string.Empty)
        {
        }

        public ChangeDepartmentInfoCommand(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

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

        private class CommandValidator : Validator<ChangeDepartmentInfoCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ChangeDepartmentInfoCommand, object>>, string>> RuleFor(ChangeDepartmentInfoCommand target)
            {
                if (target.Name.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeDepartmentInfoCommand, object>>, string>(model => model.Name, "名称为空");

                if (target.Descn.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeDepartmentInfoCommand, object>>, string>(model => model.Descn, "描述为空");

                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeDepartmentInfoCommand, object>>, string>(m => m.AggregateId, "聚合Id为空");
            }
        }

        #endregion validator
    }
}