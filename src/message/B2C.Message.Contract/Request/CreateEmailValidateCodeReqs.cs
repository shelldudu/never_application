using Never;
using Never.DataAnnotations;
using Never.Deployment;
using B2C.Infrastructure;
using B2C.Message.Contract.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace B2C.Message.Contract.Request
{
    /// <summary>
    /// 创建邮箱验证码
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class CreateEmailValidateCodeReqs : IRoutePrimaryKeySelect, IBehaviorPlatform
    {
        #region prop

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public UsageType UsageType { get; set; }

        /// <summary>
        /// 验证码长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 生成客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 客户端平台
        /// </summary>
        public OperatePlatform Platform { get; set; }

        /// <summary>
        /// api路由主键
        /// </summary>
        string IRoutePrimaryKeySelect.PrimaryKey
        {
            get
            {
                return this.Email;
            }
        }

        #endregion prop

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<CreateEmailValidateCodeReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateEmailValidateCodeReqs, object>>, string>> RuleFor(CreateEmailValidateCodeReqs target)
            {
                if (target.Email.IsNullOrWhiteSpace())
                {
                    yield return new KeyValuePair<Expression<Func<CreateEmailValidateCodeReqs, object>>, string>(model => model.Email, "邮箱为空");
                }

                if (!target.Email.IsEmail())
                {
                    yield return new KeyValuePair<Expression<Func<CreateEmailValidateCodeReqs, object>>, string>(model => model.Email, "邮箱不正确");
                }
            }
        }

        #endregion validator
    }
}