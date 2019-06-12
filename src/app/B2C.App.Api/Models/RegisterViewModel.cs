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
    /// 注册用户请求
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class RegisterViewModel
    {
        #region prop

        /// <summary>
        /// 手机号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string VCode { get; set; }

        #endregion prop

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<RegisterViewModel>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<RegisterViewModel, object>>, string>> RuleFor(RegisterViewModel target)
            {
                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RegisterViewModel, object>>, string>(model => model.Password, "密码为空");

                if (target.UserName.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RegisterViewModel, object>>, string>(model => model.UserName, "手机号码为空");

                if (target.VCode.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<RegisterViewModel, object>>, string>(model => model.VCode, "短信验证码为空");
            }
        }

        #endregion validator
    }
}