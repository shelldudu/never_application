using System;

namespace B2C.Calc.Banks.Strategies
{
    /// <summary>
    /// 按月计算策略
    /// </summary>
    public class MonthStrategy : IDateTypeStrategy
    {
        /// <summary>
        /// 利率计算方式
        /// </summary>
        public RepaymentTimeUnitType DateType
        {
            get
            {
                return Banks.RepaymentTimeUnitType.月;
            }
        }

        /// <summary>
        /// 策略名字
        /// </summary>
        public string Name
        {
            get { return "按月计算"; }
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
                    return amount * 1m * 30;

                case RepaymentRateType.月利率:
                    return amount * 1m;

                case RepaymentRateType.季利率:
                    return amount * 1m / 3;

                default:
                case RepaymentRateType.年利率:
                    return amount * 1m / 12;
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
                    return 1m * 30;

                case RepaymentRateType.月利率:
                    return 1m;

                case RepaymentRateType.季利率:
                    return 1m / 3;

                default:
                case RepaymentRateType.年利率:
                    return 1m / 12;
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
            // 要找出这个月的所有天数
            //var thisMonth = new DateTime(startTime.Year, startTime.Month, 1);
            // var nextMonth = thisMonth.AddMonths(1);
            //var totalDays = (nextMonth - thisMonth).Days;

            var inter = (int)Math.Floor(interval);
            var left = interval - inter;

            // var nextTime = startTime.Date.AddMonths(inter).AddDays(Math.Floor((double)left * totalDays));

            var nextTime = startTime.Date.AddMonths(inter).AddDays(Math.Floor((double)left * 30));
            return nextTime.Date.Add(startTime - startTime.Date);
        }
    }
}