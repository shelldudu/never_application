using B2C.Calc.Banks.Repayments;
using System;
using System.Collections.Generic;

namespace B2C.Calc.Banks
{
    /// <summary>
    /// 计算器工厂
    /// </summary>
    public class RepaymentFactory
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="RepaymentFactory"/> class.
        /// </summary>
        protected RepaymentFactory()
        {
        }

        #endregion ctor

        #region field

        /// <summary>
        /// 原始计算器
        /// </summary>
        private readonly static RepaymentFactory instance = new RepaymentFactory();

        #endregion field

        #region singleton

        /// <summary>
        /// 单例
        /// </summary>
        public static RepaymentFactory Default
        {
            get
            {
                return instance;
            }
        }

        #endregion singleton

        #region calc

        /// <summary>
        /// 获取 <see cref="IRepaymentCalc"/> 还款计算器.
        /// </summary>
        /// <value>
        /// 还款计算器 <see cref="IRepaymentCalc"/>.
        /// </value>
        /// <param name="dtype">结算方式</param>
        /// <param name="reType">还款模型</param>
        /// <returns></returns>
        public IRepaymentCalc this[RepaymentTimeUnitType dtype, RepaymentType reType]
        {
            get
            {
                return this[dtype, reType, DateTime.Now];
            }
        }

        /// <summary>
        /// 获取 <see cref="IRepaymentCalc"/> 还款计算器.
        /// </summary>
        /// <value>
        /// 还款计算器 <see cref="IRepaymentCalc"/>.
        /// </value>
        /// <param name="dtype">结算方式</param>
        /// <param name="reType">还款模型</param>
        /// <param name="startDate">开始结算时间</param>
        /// <returns></returns>
        public IRepaymentCalc this[RepaymentTimeUnitType dtype, RepaymentType reType, DateTime startDate]
        {
            get
            {
                if (reType == RepaymentType.按期付息N期还本)
                    throw new ArgumentException("该方法不能创建按期付息N期还本之本金固定");

                if (reType == RepaymentType.按期付息L期还本)
                    throw new ArgumentException("该方法不能创建按期付息N期还本之本金缩小");

                switch (reType)
                {
                    case RepaymentType.等额本息:
                        return new 等额本息(dtype, startDate);

                    case RepaymentType.等额本金:
                        return new 等额本金(dtype, startDate);

                    default:
                    case RepaymentType.一次性还本付息:
                        return new 一次性还本付息(dtype, startDate);

                    case RepaymentType.按期付息到期还本:
                        return new 按期付息到期还本(dtype, startDate);

                    case RepaymentType.等本等息:
                        return new 等本等息(dtype, startDate);
                }
            }
        }

        /// <summary>
        /// 获取 <see cref="IRepaymentCalc"/> 还款计算器.
        /// </summary>
        /// <value>
        /// 还款计算器 <see cref="IRepaymentCalc"/>.
        /// </value>
        /// <param name="dtype">结算方式</param>
        /// <param name="reType">还款模型</param>
        /// <param name="principalPeriods">本金的还款信息</param>
        /// <returns></returns>
        public IRepaymentCalc this[RepaymentTimeUnitType dtype, RepaymentType reType, IEnumerable<PrincipalRepayType> principalPeriods]
        {
            get
            {
                return this[dtype, reType, principalPeriods, DateTime.Now];
            }
        }

        /// <summary>
        /// 获取 <see cref="IRepaymentCalc"/> 还款计算器.
        /// </summary>
        /// <value>
        /// 还款计算器 <see cref="IRepaymentCalc"/>.
        /// </value>
        /// <param name="dtype">结算方式</param>
        /// <param name="reType">还款模型</param>
        /// <param name="principalPeriods">本金的还款信息</param>
        /// <returns></returns>
        public IRepaymentCalc this[RepaymentTimeUnitType dtype, RepaymentType reType, IEnumerable<PrincipalRepayPercent> principalPeriods]
        {
            get
            {
                return this[dtype, reType, principalPeriods, DateTime.Now];
            }
        }

        /// <summary>
        /// 获取 <see cref="IRepaymentCalc"/> 还款计算器.
        /// </summary>
        /// <value>
        /// 还款计算器 <see cref="IRepaymentCalc"/>.
        /// </value>
        /// <param name="dtype">结算方式</param>
        /// <param name="principalPeriods">本金的还款信息</param>
        /// <param name="reType">还款模型</param>
        /// <param name="startDate">开始结算时间</param>
        /// <returns></returns>
        public IRepaymentCalc this[RepaymentTimeUnitType dtype, RepaymentType reType, IEnumerable<PrincipalRepayType> principalPeriods, DateTime startDate]
        {
            get
            {
                switch (reType)
                {
                    case RepaymentType.按期付息L期还本:
                        return new 按期付息L期还本(dtype, startDate, principalPeriods);

                    case RepaymentType.按期付息N期还本:
                        return new 按期付息N期还本(dtype, startDate, principalPeriods);
                }


                return this[dtype, reType, startDate];
            }
        }

        /// <summary>
        /// 获取 <see cref="IRepaymentCalc"/> 还款计算器.
        /// </summary>
        /// <value>
        /// 还款计算器 <see cref="IRepaymentCalc"/>.
        /// </value>
        /// <param name="dtype">结算方式</param>
        /// <param name="principalPeriods">本金的还款信息</param>
        /// <param name="reType">还款模型</param>
        /// <param name="startDate">开始结算时间</param>
        /// <returns></returns>
        public IRepaymentCalc this[RepaymentTimeUnitType dtype, RepaymentType reType, IEnumerable<PrincipalRepayPercent> principalPeriods, DateTime startDate]
        {
            get
            {
                switch (reType)
                {
                    case RepaymentType.本金按百分比还:
                        return new 本金按百分比还(dtype, startDate, principalPeriods);
                }

                return this[dtype, reType, startDate];
            }
        }

        #endregion calc
    }
}