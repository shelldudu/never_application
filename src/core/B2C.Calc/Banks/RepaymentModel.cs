using System;

namespace B2C.Calc.Banks
{
    /// <summary>
    /// 还款模型
    /// </summary>
    public struct RepaymentModel
    {
        #region prop

        /// <summary>
        /// 期数
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 还款
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 利息
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        public decimal Principal { get; set; }

        #endregion prop
    }
}