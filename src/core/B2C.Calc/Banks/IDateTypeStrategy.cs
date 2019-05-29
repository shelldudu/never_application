using System;

namespace B2C.Calc.Banks
{
    /// <summary>
    /// 时间计算策略
    /// </summary>
    public interface IDateTypeStrategy
    {
        /// <summary>
        /// 利率计算方式
        /// </summary>
        RepaymentTimeUnitType DateType { get; }

        /// <summary>
        /// 策略名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取N倍的单元金额
        /// </summary>
        /// <param name="amount">总金额</param>
        /// <param name="rateType">利率计算方式</param>
        /// <returns></returns>
        decimal GetNUnitAmount(decimal amount, RepaymentRateType rateType = RepaymentRateType.年利率);

        /// <summary>
        /// 获取还款时间
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="interval">间隔</param>
        /// <returns></returns>
        DateTime GetPayTime(DateTime startTime, decimal interval);
    }
}