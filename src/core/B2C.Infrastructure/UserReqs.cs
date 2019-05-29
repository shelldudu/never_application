using Never.DataAnnotations;
using Never.Deployment;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 用户请求
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class UserReqs : IRoutePrimaryKeySelect
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 路由主键
        /// </summary>
        public virtual string PrimaryKey => this.UserId.ToString();

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<UserReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<UserReqs, object>>, string>> RuleFor(UserReqs target)
            {
                if (target.UserId <= 0)
                    yield return new KeyValuePair<Expression<Func<UserReqs, object>>, string>(model => model.UserId, "UserId为空");
            }
        }

        #endregion validator
    }
}