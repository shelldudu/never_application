using Never;
using Never.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace B2C.Admin.Web.Models
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [Serializable, Validator(typeof(ModelValidator))]
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
        [DisplayName("登录密码"), DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 用户验证码
        /// </summary>
        [DisplayName("用户验证码")]
        public string RandCode { get; set; }

        #endregion prop

        #region validator

        private class ModelValidator : Validator<LoginViewModel>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<LoginViewModel, object>>, string>> RuleFor(LoginViewModel target)
            {
                if (target.UserName.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<LoginViewModel, object>>, string>(m => m.UserName, "用户名为空");

                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<LoginViewModel, object>>, string>(m => m.Password, "密码为空");

                if (target.RandCode.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<LoginViewModel, object>>, string>(m => m.RandCode, "验证码为空");
            }
        }

        #endregion validator
    }
}