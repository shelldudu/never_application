using System;
using System.Collections.Generic;

namespace B2C.Calc.Banks.Repayments
{
    /// <summary>
    /// 按期付息到期还本
    /// </summary>
    public class 按期付息到期还本 : RepaymentCalc, IRepaymentCalc
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="按期付息到期还本"/> class.
        /// </summary>
        /// <param name="dateType">结算类型</param>
        /// <param name="startDate">开始结算日期</param>
        public 按期付息到期还本(RepaymentTimeUnitType dateType, DateTime startDate)
            : base(dateType, startDate)
        {
        }

        #endregion ctor

        #region calc

        /// <summary>
        /// 还款列表
        /// </summary>
        /// <param name="principal">本金</param>
        /// <param name="rateType">利率计算方式</param>
        /// <param name="rate">利率</param>
        /// <param name="deadline">期限</param>
        /// <param name="periods">期数</param>
        /// <param name="autoFixPrincipalToInteger">是否为自动修正本金整数</param>
        /// <returns></returns>
        public override IEnumerable<RepaymentModel> GetRepayments(decimal principal, RepaymentRateType rateType, decimal rate, decimal deadline, int periods = 1, bool autoFixPrincipalToInteger = true)
        {
            if (periods <= 0)
                throw new ArgumentException("还款期数不能小于0");

            var list = new List<RepaymentModel>(periods);

            for (var i = 1; i < periods; i++)
                list.Add(new RepaymentModel() { Principal = 0, Interest = this.Strategy.GetNUnitAmount(principal * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });

            list.Add(new RepaymentModel() { Principal = principal, Interest = this.Strategy.GetNUnitAmount(principal * rate * deadline, rateType) / periods, Period = periods, PayDate = this.Strategy.GetPayTime(this.StartDate, deadline) });

            return list;
        }

        #endregion calc

        #region tostring

        /// <summary>
        /// 返回string
        /// </summary>
        public override string ToString()
        {
            return "按期付息到期还本";
        }

        #endregion tostring
    }
}