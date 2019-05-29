namespace B2C.Calc.Banks
{
    /// <summary>
    /// 还款金额占比
    /// </summary>
    public struct PrincipalRepayPercent
    {
        /// <summary>
        /// 百分比，使用30%-30%-30%
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalRepayPercent"/> class.
        /// </summary>
        /// <param name="percent">百分比</param>
        /// <param name="period">期数</param>
        public PrincipalRepayPercent(int period, decimal percent)
        {
            this.Period = period;
            this.Percent = percent;
        }
    }
}