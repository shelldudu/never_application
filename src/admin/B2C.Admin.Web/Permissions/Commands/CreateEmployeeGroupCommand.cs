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
    [Validator(typeof(CreateDepartmentCommandValidator))]
    public class CreateEmployeeGroupCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        private CreateEmployeeGroupCommand() : this(string.Empty)
        {
        }

        public CreateEmployeeGroupCommand(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 组Id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public string EmployeeId { get; set; }

        #endregion prop

        #region validator

        public class CreateDepartmentCommandValidator : Never.DataAnnotations.Validator<CreateEmployeeGroupCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateEmployeeGroupCommand, object>>, string>> RuleFor(CreateEmployeeGroupCommand instance)
            {
                if (instance.GroupId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<CreateEmployeeGroupCommand, object>>, string>(model => model.GroupId, "组Id为空");

                if (instance.EmployeeId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<CreateEmployeeGroupCommand, object>>, string>(model => model.EmployeeId, "员工Id为空");
            }
        }

        #endregion validator
    }
}