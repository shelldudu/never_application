using Never;
using Never.CommandStreams;
using Never.DataAnnotations;
using B2C.Admin.Web.Permissions.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace B2C.Admin.Web.Permissions.Commands
{
    [Serializable, CommandDomain(Domain = "admin")]
    [Validator(typeof(CommandValidator))]
    public class CreateEmployeeCommand : Never.Domains.StringAggregateCommand
    {
        #region prop

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 级的分类，分配的时候不能越级
        /// </summary>
        public GroupSort GroupSort { get; set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEmployeeCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public CreateEmployeeCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEmployeeCommand"/> class.
        /// </summary>
        private CreateEmployeeCommand()
            : this(string.Empty)
        {
        }

        #endregion ctor

        #region vaidator

        private class CommandValidator : Validator<CreateEmployeeCommand>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateEmployeeCommand, object>>, string>> RuleFor(CreateEmployeeCommand target)
            {
                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateEmployeeCommand, object>>, string>(m => m.Password, "用户密码为空");

                if (target.UserName.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateEmployeeCommand, object>>, string>(m => m.UserName, "用户名字为空");

                if (target.DepartmentId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateEmployeeCommand, object>>, string>(m => m.DepartmentId, "部门为空");

                if (target.AggregateId.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<CreateEmployeeCommand, object>>, string>(m => m.AggregateId, "聚合Id为空");
            }
        }

        #endregion vaidator
    }
}