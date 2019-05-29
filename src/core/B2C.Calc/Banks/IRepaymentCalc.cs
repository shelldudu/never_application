using System;
using System.Collections.Generic;

namespace B2C.Calc.Banks
{
    /// <summary>
    /// 还款计算器
    /// </summary>
    public interface IRepaymentCalc
    {
        /// <summary>
        /// 还款时间
        /// </summary>
        DateTime StartDate { get; }

        /// <summary>
        /// 利率计算方式
        /// </summary>
        IDateTypeStrategy Strategy { get; }

        /// <summary>
        /// 总利息
        /// </summary>
        /// <param name="principal">本金</param>
        /// <param name="rateType">利率计算方式</param>
        /// <param name="rate">利率</param>
        /// <param name="deadline">期限</param>
        /// <param name="periods">期数</param>
        /// <returns></returns>
        decimal GetTotalInterest(decimal principal, RepaymentRateType rateType, decimal rate, decimal deadline, int periods = 1);

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
        IEnumerable<RepaymentModel> GetRepayments(decimal principal, RepaymentRateType rateType, decimal rate, decimal deadline, int periods = 1, bool autoFixPrincipalToInteger = true);
    }
}