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
    public class UserViewModel
    {
        #region prop

        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        public string UserName { get; set; }

        #endregion prop

        #region validator

        private class RequestValidator : Validator<UserViewModel>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<UserViewModel, object>>, string>> RuleFor(UserViewModel target)
            {
                if (target.UserName.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<UserViewModel, object>>, string>(m => m.UserName, "手机号码为空");
            }
        }

        #endregion validator
    }
}