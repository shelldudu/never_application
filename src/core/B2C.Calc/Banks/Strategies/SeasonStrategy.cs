using System;

namespace B2C.Calc.Banks.Strategies
{
    /// <summary>
    /// 按季计算策略
    /// </summary>
    public class SeasonStrategy : IDateTypeStrategy
    {
        /// <summary>
        /// 利率计算方式
        /// </summary>
        public RepaymentTimeUnitType DateType
        {
            get
            {
                return Banks.RepaymentTimeUnitType.季;
            }
        }

        /// <summary>
        /// 策略名字
        /// </summary>
        public string Name
        {
            get { return "按季计算"; }
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
                    return amount * 1m * 90;

                case RepaymentRateType.月利率:
                    return amount * 1m * 3;

                case RepaymentRateType.季利率:
                    return amount * 1m;

                default:
                case RepaymentRateType.年利率:
                    return amount * 1m / 4;
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
                    return 1m * 90;

                case RepaymentRateType.月利率:
                    return 1m * 3;

                case RepaymentRateType.季利率:
                    return 1m;

                default:
                case RepaymentRateType.年利率:
                    return 1m / 4;
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
            //int days = 0;
            //switch (startTime.Month)
            //{
            //    default:
            //    case 1:
            //    case 2:
            //    case 3:
            //        days = (new DateTime(startTime.Year, 4, 1).AddSeconds(-1) - new DateTime(startTime.Year, 1, 1)).Days;
            //        break;

            //    case 4:
            //    case 5:
            //    case 6:
            //        days = (new DateTime(startTime.Year, 7, 1).AddSeconds(-1) - new DateTime(startTime.Year, 4, 1)).Days;
            //        break;

            //    case 7:
            //    case 8:
            //    case 9:
            //        days = (new DateTime(startTime.Year, 10, 1).AddSeconds(-1) - new DateTime(startTime.Year, 8, 1)).Days;
            //        break;

            //    case 10:
            //    case 11:
            //    case 12:
            //        days = (new DateTime(startTime.Year + 1, 1, 1).AddSeconds(-1) - new DateTime(startTime.Year, 10, 1)).Days;
            //        break;
            //}

            var nextTime = startTime.Date.AddMonths(inter * 4).AddDays(Math.Floor((double)left * 90));

            return nextTime.Date.Add(startTime - startTime.Date);
        }
    }
}