using Never;
using Never.DataAnnotations;
using Never.Deployment;
using B2C.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace B2C.App.Api
{
    /// <summary>
    /// 注册用户请求
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class RegisterUserReqs : Never.Deployment.IRoutePrimaryKeySelect
    {
        #region prop

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegisteIP { get; set; }

        /// <summary>
        /// 注册平台
        /// </summary>
        public OperatePlatform Platform { get; set; }

        /// <summary>
        /// 路由主键
        /// </summary>
        string IRoutePrimaryKeySelect.PrimaryKey => this.Mobile;

        #endregion prop

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<RegisterUserReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<RegisterUserReqs, object>>, string>> RuleFor(RegisterUserReqs target)
            {
                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RegisterUserReqs, object>>, string>(model => model.Password, "密码为空");

                if (target.Mobile.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RegisterUserReqs, object>>, string>(model => model.Password, "手机号码为空");
            }
        }

        #endregion validator
    }
}