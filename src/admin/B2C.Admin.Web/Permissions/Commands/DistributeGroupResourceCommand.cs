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
    public class DistributeGroupResourceCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        public DistributeGroupResourceCommand() : this(string.Empty)
        {
        }

        private DistributeGroupResourceCommand(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 组Id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public IEnumerable<string> ResourceId { get; set; }

        #endregion prop

        #region validator

        public class CreateDepartmentCommandValidator : Never.DataAnnotations.Validator<DistributeGroupResourceCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<DistributeGroupResourceCommand, object>>, string>> RuleFor(DistributeGroupResourceCommand instance)
            {
                if (instance.GroupId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<DistributeGroupResourceCommand, object>>, string>(model => model.GroupId, "组Id为空");
            }
        }

        #endregion validator
    }
}