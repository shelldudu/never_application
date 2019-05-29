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
    public class CreateDepartmentCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        private CreateDepartmentCommand() : this(string.Empty)
        {
        }

        public CreateDepartmentCommand(string aggregateId) : base(aggregateId)
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

        private class CommandValidator : Never.DataAnnotations.Validator<CreateDepartmentCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateDepartmentCommand, object>>, string>> RuleFor(CreateDepartmentCommand instance)
            {
                if (instance.Name.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateDepartmentCommand, object>>, string>(model => model.Name, "名称为空");

                if (instance.Descn.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateDepartmentCommand, object>>, string>(model => model.Descn, "描述为空");
            }
        }

        #endregion validator
    }
}