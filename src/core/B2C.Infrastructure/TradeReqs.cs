using Never;
using Never.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 交易请求
    /// </summary>
    [Validator(typeof(RequestValidator))]
    public class TradeReqs: UserReqs
    {
        /// <summary>
        /// 交易号
        /// </summary>
        public string TransNo { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 路由主键
        /// </summary>
        public override string PrimaryKey =>string.IsNullOrEmpty(this.TransNo)? this.UserId.ToString():this.TransNo;

        #region validator

        /// <summary>
        /// 创建用户命令验证
        /// </summary>
        private class RequestValidator : Validator<TradeReqs>
        {
            public override IEnumerable<KeyValuePair<Expression<Func<TradeReqs, object>>, string>> RuleFor(TradeReqs target)
            {
                if (target.UserId <= 0)
                    yield return new KeyValuePair<Expression<Func<TradeReqs, object>>, string>(model => model.UserId, "UserId为空");

                if (target.TransNo.IsNullOrWhiteSpace())
                    yield return new KeyValuePair<Expression<Func<TradeReqs, object>>, string>(model => model.TransNo, "交易号为空");
            }
        }

        #endregion validator
    }
}
