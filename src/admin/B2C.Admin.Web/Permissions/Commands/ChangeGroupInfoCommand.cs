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
    public class ChangeGroupInfoCommand : Never.Domains.ChangeAggregateCommand<string>
    {
        #region ctor

        private ChangeGroupInfoCommand() : this(string.Empty)
        {
        }

        public ChangeGroupInfoCommand(string aggregateId) : base(aggregateId)
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

        private class CommandValidator : Never.DataAnnotations.Validator<ChangeGroupInfoCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ChangeGroupInfoCommand, object>>, string>> RuleFor(ChangeGroupInfoCommand instance)
            {
                if (instance.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeGroupInfoCommand, object>>, string>(model => model.AggregateId, "聚合Id为空");

                if (instance.Name.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeGroupInfoCommand, object>>, string>(model => model.Name, "名称为空");

                if (instance.Descn.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangeGroupInfoCommand, object>>, string>(model => model.Descn, "描述为空");
            }
        }

        #endregion validator
    }
}