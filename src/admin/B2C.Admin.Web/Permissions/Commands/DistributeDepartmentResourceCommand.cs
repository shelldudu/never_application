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
    public class DistributeDepartmentResourceCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        public DistributeDepartmentResourceCommand() : this(string.Empty)
        {
        }

        private DistributeDepartmentResourceCommand(string aggregateId) : base(aggregateId)
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
        public IEnumerable<string> ResourceId { get; set; }

        #endregion prop

        #region validator

        public class CreateDepartmentCommandValidator : Never.DataAnnotations.Validator<DistributeDepartmentResourceCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<DistributeDepartmentResourceCommand, object>>, string>> RuleFor(DistributeDepartmentResourceCommand instance)
            {
                if (instance.DepartmentId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<DistributeDepartmentResourceCommand, object>>, string>(model => model.DepartmentId, "部门Id为空");
            }
        }

        #endregion validator
    }
}