using System;

namespace B2C.Calc.Banks.Strategies
{
    /// <summary>
    /// 按天计算策略
    /// </summary>
    public class DayStrategy : IDateTypeStrategy
    {
        /// <summary>
        /// 利率计算方式
        /// </summary>
        public RepaymentTimeUnitType DateType
        {
            get
            {
                return Banks.RepaymentTimeUnitType.天;
            }
        }

        /// <summary>
        /// 策略名字
        /// </summary>
        public string Name
        {
            get { return "按天计算"; }
        }

        /// <summary>
        /// 获取N倍的单元金额
        /// </summary>
        /// <param name="amount">总金额</param>
        /// <param name="rateType">利率计算方式</param>
        /// <returns></returns>
        public decimal GetNUnitAmount(decimal amount, RepaymentRateType rateType = RepaymentRateType.年利率)
        {
            switch (rateType)
            {
                case RepaymentRateType.天利率:
                    return amount * 1m;

                case RepaymentRateType.月利率:
                    return amount * 1m / 30;

                case RepaymentRateType.季利率:
                    return amount * 1m / 90;

                default:
                case RepaymentRateType.年利率:
                    return amount * 1m / 360;
            }
        }

        /// <summary>
        /// 单元金额
        /// </summary>
        /// <param name="rateType">利率计算方式</param>
        /// <returns></returns>
        public decimal GetUnitAmount(RepaymentRateType rateType = RepaymentRateType.年利率)
        {
            switch (rateType)
            {
                case RepaymentRateType.天利率:
                    return 1m;

                case RepaymentRateType.月利率:
                    return 1m / 30;

                case RepaymentRateType.季利率:
                    return 1m / 90;

                default:
                case RepaymentRateType.年利率:
                    return 1m / 360;
            }
        }

        /// <summary>
        /// 获取还款时间
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="interval">间隔</param>
        /// <returns></returns>
        public DateTime GetPayTime(DateTime startTime, decimal interval)
        {
            var inter = (int)Math.Floor(interval);
            var left = interval - inter;

            var nextTime = startTime.Date.AddDays(inter).AddHours(Math.Floor((double)left * 24));

            return nextTime.Date.Add(startTime - startTime.Date);
        }
    }
}