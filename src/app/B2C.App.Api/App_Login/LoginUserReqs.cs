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
    /// 用户修改密码请求
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class LoginUserReqs : IRoutePrimaryKeySelect
    {
        #region prop and ctor

        /// <summary>
        /// 手机号d
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 路由主键
        /// </summary>
        string IRoutePrimaryKeySelect.PrimaryKey => this.Mobile;

        /// <summary>
        /// 最后登陆IP
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登陆平台
        /// </summary>
        public OperatePlatform Platform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LoginUserReqs()
        {
            this.Platform = OperatePlatform.Android;
        }

        #endregion

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<LoginUserReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<LoginUserReqs, object>>, string>> RuleFor(LoginUserReqs target)
            {
                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<LoginUserReqs, object>>, string>(model => model.Password, "密码为空");

                if (target.Mobile.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<LoginUserReqs, object>>, string>(model => model.Mobile, "手机号码为空");
            }
        }

        #endregion validator
    }
}