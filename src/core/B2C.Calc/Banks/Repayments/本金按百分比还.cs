using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Calc.Banks.Repayments
{
    /// <summary>
    /// 本金按百分比还
    /// </summary>
    public class 本金按百分比还 : RepaymentCalc, IRepaymentCalc
    {
        #region field

        /// <summary>
        /// 本金还款信息
        /// </summary>
        private readonly IEnumerable<PrincipalRepayPercent> principalPeriods = null;

        #endregion field

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="本金按百分比还"/> class.
        /// </summary>
        /// <param name="dateType">结算类型</param>
        /// <param name="startDate">开始结算日期</param>
        /// <param name="principalPeriods">本金的还款信息</param>
        public 本金按百分比还(RepaymentTimeUnitType dateType, DateTime startDate, IEnumerable<PrincipalRepayPercent> principalPeriods)
            : base(dateType, startDate)
        {
            this.principalPeriods = principalPeriods ?? Enumerable.Empty<PrincipalRepayPercent>();
            if (this.principalPeriods.Sum(t => t.Percent) != 100)
                throw new ArgumentException("所有还款期数总和不等于100%");
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

            if (periods != this.principalPeriods.Count() || periods != this.principalPeriods.Max(p => p.Period))
                throw new ArgumentException("还款总期数不等于剩余还款本金期数");

            var list = new List<RepaymentModel>(periods);
            var hadPayPrincipal = 0m;
            /*不用修正本金*/
            if (!autoFixPrincipalToInteger)
            {
                for (var i = 1; i <= periods; i++)
                {
                    var percent = this.principalPeriods.ElementAt(i - 1);
                    list.Add(new RepaymentModel() { Principal = principal * percent.Percent / 100, Interest = this.Strategy.GetNUnitAmount((principal - hadPayPrincipal) * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });
                    hadPayPrincipal += principal * percent.Percent / 100;
                }

                return list;
            }

            for (var i = 1; i <= periods; i++)
            {
                var percent = this.principalPeriods.ElementAt(i - 1);

                if (i != periods)
                {
                    list.Add(new RepaymentModel() { Principal = principal * percent.Percent / 100, Interest = this.Strategy.GetNUnitAmount((principal - hadPayPrincipal) * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });
                }
                else
                {
                    list.Add(new RepaymentModel() { Principal = principal - hadPayPrincipal, Interest = this.Strategy.GetNUnitAmount((principal - hadPayPrincipal) * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });
                }

                hadPayPrincipal += principal * percent.Percent / 100;
            }

            return list;
        }

        #endregion calc

        #region tostring

        /// <summary>
        /// 返回string
        /// </summary>
        public override string ToString()
        {
            return "本金按百分比还";
        }

        #endregion tostring
    }
}