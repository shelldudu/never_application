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
    public class CreateGroupResourceCommand : Never.Domains.StringAggregateCommand
    {
        #region ctor

        private CreateGroupResourceCommand() : this(string.Empty)
        {
        }

        public CreateGroupResourceCommand(string aggregateId) : base(aggregateId)
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
        public string ResourceId { get; set; }

        #endregion prop

        #region validator

        public class CreateDepartmentCommandValidator : Never.DataAnnotations.Validator<CreateGroupResourceCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateGroupResourceCommand, object>>, string>> RuleFor(CreateGroupResourceCommand instance)
            {
                if (instance.GroupId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<CreateGroupResourceCommand, object>>, string>(model => model.GroupId, "组Id为空");

                if (instance.ResourceId.IsNullOrEmpty())
                    yield return new KeyValuePair<Expression<Func<CreateGroupResourceCommand, object>>, string>(model => model.ResourceId, "资源为空");
            }
        }

        #endregion validator
    }
}