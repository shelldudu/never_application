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
    /// 修改密码请求
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class ChangePwdViewModel
    {
        #region prop

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 原始密码
        /// </summary>
        public string Original { get; set; }

        #endregion prop

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<ChangePwdViewModel>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ChangePwdViewModel, object>>, string>> RuleFor(ChangePwdViewModel target)
            {
                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangePwdViewModel, object>>, string>(model => model.Password, "密码为空");

                if (target.Original.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangePwdViewModel, object>>, string>(model => model.Original, "原始密码为空");
            }
        }

        #endregion validator
    }
}