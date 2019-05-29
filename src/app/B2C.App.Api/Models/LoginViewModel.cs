using Never;
using Never.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace B2C.App.Api.Models
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [Serializable, Validator(typeof(RequestValidator))]
    public class LoginViewModel
    {
        #region prop

        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [DisplayName("登录密码")]
        public string Password { get; set; }

        #endregion prop

        #region validator

        private class RequestValidator : Validator<LoginViewModel>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<LoginViewModel, object>>, string>> RuleFor(LoginViewModel target)
            {
                if (target.UserName.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<LoginViewModel, object>>, string>(m => m.UserName, "手机号码为空");

                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<LoginViewModel, object>>, string>(m => m.Password, "密码为空");
            }
        }

        #endregion validator
    }
}