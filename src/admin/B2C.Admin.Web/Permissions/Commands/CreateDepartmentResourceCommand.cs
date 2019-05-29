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
    public class CreateDepartmentResourceCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        private CreateDepartmentResourceCommand() : this(string.Empty)
        {
        }

        public CreateDepartmentResourceCommand(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public string ResourceId { get; set; }

        #endregion prop

        #region validator

        public class CreateDepartmentCommandValidator : Never.DataAnnotations.Validator<CreateDepartmentResourceCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateDepartmentResourceCommand, object>>, string>> RuleFor(CreateDepartmentResourceCommand instance)
            {
                if (instance.DepartmentId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<CreateDepartmentResourceCommand, object>>, string>(model => model.DepartmentId, "部门Id为空");

                if (instance.ResourceId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<CreateDepartmentResourceCommand, object>>, string>(model => model.ResourceId, "资源为空");
            }
        }

        #endregion validator
    }
}