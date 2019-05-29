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
    public class DistributeEmployeeGroupCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        public DistributeEmployeeGroupCommand() : this(string.Empty)
        {
        }

        private DistributeEmployeeGroupCommand(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 员工Id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public IEnumerable<string> GroupId { get; set; }

        #endregion prop

        #region validator

        public class CreateDepartmentCommandValidator : Never.DataAnnotations.Validator<DistributeEmployeeGroupCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<DistributeEmployeeGroupCommand, object>>, string>> RuleFor(DistributeEmployeeGroupCommand instance)
            {
                if (instance.EmployeeId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<DistributeEmployeeGroupCommand, object>>, string>(model => model.EmployeeId, "员工Id为空");
            }
        }

        #endregion validator
    }
}