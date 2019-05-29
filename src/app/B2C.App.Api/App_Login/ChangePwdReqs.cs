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
    public class ChangePwdReqs : Never.Deployment.IRoutePrimaryKeySelect
    {
        #region prop

        /// <summary>
        /// 用户Id，跟Mobile可二选一
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 手机号d，跟UserId可二选一
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 旧密码，用来验证的，可选
        /// </summary>
        public string OriginalPwd { get; set; }

        /// <summary>
        /// 路由主键
        /// </summary>
        string IRoutePrimaryKeySelect.PrimaryKey => this.UserId > 0 ? this.UserId.ToString() : this.Mobile;

        #endregion prop

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<ChangePwdReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ChangePwdReqs, object>>, string>> RuleFor(ChangePwdReqs target)
            {
                if (target.Password.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<ChangePwdReqs, object>>, string>(model => model.Password, "密码为空");

                if (target.UserId <= 0 && (target.Mobile.IsNullOrWhiteSpace()))
                    yield return new KeyValuePair<Expression<Func<ChangePwdReqs, object>>, string>(model => model.UserId, "UserId或手机号码为空");
            }
        }

        #endregion validator
    }
}