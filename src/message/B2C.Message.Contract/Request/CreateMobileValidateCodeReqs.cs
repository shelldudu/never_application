using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using B2C.Infrastructure;
using B2C.Message.Contract.EnumTypes;
using Never;
using Never.DataAnnotations;
using Never.Deployment;

namespace B2C.Message.Contract.Request
{
    /// <summary>
    /// 请求创建验证码
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class CreateMobileValidateCodeReqs : IRoutePrimaryKeySelect, IBehaviorPlatform
    {
        #region prop

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

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
                return this.Mobile;
            }
        }

        #endregion prop

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<CreateMobileValidateCodeReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CreateMobileValidateCodeReqs, object>>, string>> RuleFor(CreateMobileValidateCodeReqs target)
            {
                if (target.Mobile.IsNullOrWhiteSpace())
                {
                    yield return new KeyValuePair<Expression<Func<CreateMobileValidateCodeReqs, object>>, string>(model => model.Mobile, "手机号码为空");
                }

                if (!target.Mobile.IsChineseMobile())
                {
                    yield return new KeyValuePair<Expression<Func<CreateMobileValidateCodeReqs, object>>, string>(model => model.Mobile, "手机号码不正确");
                }
            }
        }

        #endregion validator
    }
}