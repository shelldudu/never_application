using Never;
using Never.DataAnnotations;
using Never.Deployment;
using B2C.Infrastructure;
using B2C.Message.Contract.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Message.Contract.Request
{
    /// <summary>
    /// 检查验证码
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class CheckMobileValidateCodeReqs : IRoutePrimaryKeySelect, IBehaviorPlatform
    {
        #region prop

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VCode { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public UsageType UsageType { get; set; }

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
        private class RequestValidator : Validator<CheckMobileValidateCodeReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<CheckMobileValidateCodeReqs, object>>, string>> RuleFor(CheckMobileValidateCodeReqs target)
            {
                if (target.Mobile.IsNullOrWhiteSpace())
                {
                    yield return new KeyValuePair<Expression<Func<CheckMobileValidateCodeReqs, object>>, string>(model => model.Mobile, "手机号码为空");
                }

                if (!target.Mobile.IsChineseMobile())
                {
                    yield return new KeyValuePair<Expression<Func<CheckMobileValidateCodeReqs, object>>, string>(model => model.Mobile, "手机号码不正确");
                }

                if (target.VCode.IsNullOrEmpty())
                {
                    yield return new KeyValuePair<Expression<Func<CheckMobileValidateCodeReqs, object>>, string>(model => model.VCode, "验证码为空");
                }
            }
        }

        #endregion validator
    }
}