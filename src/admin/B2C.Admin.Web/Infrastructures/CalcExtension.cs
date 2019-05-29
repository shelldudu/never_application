using B2C.Calc.Banks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Admin.Web.Infrastructures
{
    /// <summary>
    /// 计算器扩展
    /// </summary>
    public static class CalcExtension
    {
        /// <summary>
        /// 获取还款方式
        /// </summary>
        /// <param name="payment">资产的还款方式</param>
        /// <param name="timeUnitType">利率单位</param>
        /// <param name="startTime">计息时间</param>
        /// <param name="assetPeriod">资产期限，这里会如果遇到N期还本这两种还款方式，自动转换本金平均还款期数，比如36的会转换为3，24的会转换为2</param>
        /// <returns></returns>
        public static IRepaymentCalc GetCalc(this Infrastructures.RepayType payment, Infrastructures.TimeUnitType timeUnitType, DateTime startTime, int assetPeriod)
        {
            return GetCalc(payment, timeUnitType, startTime, Enumerable.Empty<PrincipalRepayType>());
        }

        /// <summary>
        /// 获取还款方式
        /// </summary>
        /// <param name="payment">资产的还款方式</param>
        /// <param name="timeUnitType">利率单位</param>
        /// <param name="startTime">计息时间</param>
        /// <param name="principalPeriods">资产期限，这里会如果遇到N期还本这两种还款方式，自动转换本金平均还款期数，比如36的会转换为3，24的会转换为2</param>
        /// <returns></returns>
        public static IRepaymentCalc GetCalc(this Infrastructures.RepayType payment, Infrastructures.TimeUnitType timeUnitType, DateTime startTime, IEnumerable<PrincipalRepayType> principalPeriods)
        {
            return RepaymentFactory.Default[(RepaymentTimeUnitType)timeUnitType, (RepaymentType)payment, startTime];
        }

        /// <summary>
        /// 获取还款方式
        /// </summary>
        /// <param name="payment">资产的还款方式</param>
        /// <param name="timeUnitType">利率单位</param>
        /// <param name="startTime">计息时间</param>
        /// <param name="principalPeriods">资产期限，这里会如果遇到N期还本这两种还款方式，自动转换本金平均还款期数，比如36的会转换为3，24的会转换为2</param>
        /// <returns></returns>
        private static IRepaymentCalc GetCalc(this Infrastructures.RepayType payment, Infrastructures.TimeUnitType timeUnitType, DateTime startTime, IEnumerable<PrincipalRepayPercent> principalPeriods)
        {
            return RepaymentFactory.Default[(RepaymentTimeUnitType)timeUnitType, (RepaymentType)payment, startTime];
        }

        /// <summary>
        /// 利率单位转换
        /// </summary>
        /// <param name="rateType">Type of the rate.</param>
        /// <returns></returns>
        public static RepaymentRateType ConvertRateType(this RateType rateType)
        {
            switch (rateType)
            {
                case RateType.天:
                    return RepaymentRateType.天利率;
                case RateType.月:
                    return RepaymentRateType.月利率;
                case RateType.季:
                    return RepaymentRateType.季利率;
                case RateType.年:
                    return RepaymentRateType.年利率;
            }

            return RepaymentRateType.年利率;
        }
    }
}
