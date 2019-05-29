using Never;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Calc.Banks.Repayments
{
    /// <summary>
    /// 还款计算器抽象
    /// </summary>
    public abstract class RepaymentCalc : IRepaymentCalc
    {
        #region ctor

        /// <summary>
        /// Initializes static members of the <see cref="RepaymentCalc"/> class.
        /// </summary>
        static RepaymentCalc()
        {
            Singleton<StrategyFactory>.Instance = new StrategyFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepaymentCalc"/> class.
        /// </summary>
        /// <param name="dateType">结算类型</param>
        /// <param name="startDate">开始结算日期</param>
        protected RepaymentCalc(RepaymentTimeUnitType dateType, DateTime startDate)
        {
            this.StartDate = startDate;
            this.DateType = dateType;
            this.Strategy = Singleton<StrategyFactory>.Instance[dateType];
            if (this.Strategy == null)
                throw new ArgumentNullException("结算方式不在定义内");
        }

        #endregion ctor

        #region calc

        /// <summary>
        /// 结算类型
        /// </summary>
        public RepaymentTimeUnitType DateType { get; private set; }

        /// <summary>
        /// 开始还款时间
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// 利率计算方式
        /// </summary>
        public IDateTypeStrategy Strategy { get; private set; }

        /// <summary>
        /// 总利息
        /// </summary>
        /// <param name="principal">本金</param>
        /// <param name="rateType">利率计算方式</param>
        /// <param name="rate">利率</param>
        /// <param name="deadline">期限</param>
        /// <param name="periods">期数</param>
        /// <returns></returns>
        public decimal GetTotalInterest(decimal principal, RepaymentRateType rateType, decimal rate, decimal deadline, int periods = 1)
        {
            var list = this.GetRepayments(principal, rateType, rate, deadline, periods);
            if (list == null)
                return 0;
            return list.Sum(r => r.Interest);
        }

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
        public abstract IEnumerable<RepaymentModel> GetRepayments(decimal principal, RepaymentRateType rateType, decimal rate, decimal deadline, int periods = 1, bool autoFixPrincipalToInteger = true);

        #endregion calc
    }
}