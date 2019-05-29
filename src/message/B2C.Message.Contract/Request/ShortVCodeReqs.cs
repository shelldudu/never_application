using Never;
using Never.DataAnnotations;
using Never.Deployment;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace B2C.Message.Contract.Request
{
    /// <summary>
    /// 短信验证码
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class ShortVCodeReqs : IRoutePrimaryKeySelect
    {
        public string VCode { get; set; }
        public string Mobile { get; set; }

        string IRoutePrimaryKeySelect.PrimaryKey => this.Mobile;

        private class RequestValidator : Validator<ShortVCodeReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<ShortVCodeReqs, object>>, string>> RuleFor(ShortVCodeReqs target)
            {
                if (target.VCode.IsNullOrWhiteSpace())
                {
                    yield return new KeyValuePair<Expression<Func<ShortVCodeReqs, object>>, string>(m => m.VCode, "验证码为空");
                }

                if (target.Mobile.IsNullOrWhiteSpace())
                {
                    yield return new KeyValuePair<Expression<Func<ShortVCodeReqs, object>>, string>(m => m.Mobile, "手机号码为空");
                }
            }
        }
    }
}
