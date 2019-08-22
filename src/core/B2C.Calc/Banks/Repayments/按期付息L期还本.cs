using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Calc.Banks.Repayments
{
    /// <summary>
    /// 按期付息L期还本，利息一直以剩余金额计算
    /// </summary>
    public class 按期付息L期还本 : RepaymentCalc, IRepaymentCalc
    {
        #region field

        /// <summary>
        /// 本金还款信息
        /// </summary>
        private readonly IEnumerable<PrincipalRepayType> principalPeriods = null;

        #endregion field

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="按期付息L期还本"/> class.
        /// </summary>
        /// <param name="dateType">结算类型</param>
        /// <param name="startDate">开始结算日期</param>
        /// <param name="principalPeriods">本金的还款信息</param>
        public 按期付息L期还本(RepaymentTimeUnitType dateType, DateTime startDate, IEnumerable<PrincipalRepayType> principalPeriods)
            : base(dateType, startDate)
        {
            this.principalPeriods = principalPeriods ?? Enumerable.Empty<PrincipalRepayType>();
            if (!this.principalPeriods.LastOrDefault().PayPrincipal)
                throw new ArgumentException("最后一期一定是还款本金");
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

            var avgPrincipalPeriod = this.principalPeriods.Count(o => o.PayPrincipal);
            if (periods != this.principalPeriods.Count())
                throw new ArgumentException("还款总期数不等于剩余还款本金期数");

            /*先平均分出本金，在还了本金后要重新减去已经还的本金*/
            var prinList = new List<decimal>(avgPrincipalPeriod);
            var list = new List<RepaymentModel>(periods);

            var hadPayPrincipal = 0m;

            /*不用修正本金*/
            if (!autoFixPrincipalToInteger)
            {
                for (var i = 0; i < avgPrincipalPeriod; i++)
                    prinList.Add(principal / avgPrincipalPeriod);

                int j = 0;
                for (var i = 1; i <= periods; i++)
                {
                    if (this.principalPeriods.ElementAt(i - 1).PayPrincipal)
                    {
                        list.Add(new RepaymentModel() { Principal = prinList[j], Interest = this.Strategy.GetNUnitAmount((principal - hadPayPrincipal) * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });
                        hadPayPrincipal += prinList[j];
                        j++;
                    }
                    else
                    {
                        list.Add(new RepaymentModel() { Principal = 0, Interest = this.Strategy.GetNUnitAmount((principal - hadPayPrincipal) * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });
                    }
                }

                return list;
            }

            if (principal % avgPrincipalPeriod == 0)
            {
                for (var i = 0; i < avgPrincipalPeriod; i++)
                    prinList.Add(principal / avgPrincipalPeriod);
            }
            else
            {
                var fixPrincipal = principal - principal % avgPrincipalPeriod;
                var firstPrinciapl = principal - (avgPrincipalPeriod - 1) * fixPrincipal / avgPrincipalPeriod;
                prinList.Add(firstPrinciapl);
                if (avgPrincipalPeriod > 1)
                {
                    for (var i = 1; i < avgPrincipalPeriod; i++)
                    {
                        prinList.Add((principal - firstPrinciapl) / (avgPrincipalPeriod - 1));
                    }
                }
            }

            int k = 0;
            for (var i = 1; i <= periods; i++)
            {
                if (this.principalPeriods.ElementAt(i - 1).PayPrincipal)
                {
                    list.Add(new RepaymentModel() { Principal = prinList[k], Interest = this.Strategy.GetNUnitAmount((principal - hadPayPrincipal) * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });
                    hadPayPrincipal += prinList[k];
                    k++;
                }
                else
                {
                    list.Add(new RepaymentModel() { Principal = 0, Interest = this.Strategy.GetNUnitAmount((principal - hadPayPrincipal) * rate * deadline, rateType) / periods, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, i * deadline / periods) });
                }
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
            return "按期付息N期还本本缩计息";
        }

        #endregion tostring
    }
}