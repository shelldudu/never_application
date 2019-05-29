namespace B2C.Calc.Banks
{
    /// <summary>
    /// 利率计算方式，比如年利率，月利率，季利率，天利率
    /// </summary>
    public enum RepaymentRateType
    {
        /// <summary>
        /// 年利率
        /// </summary>
        年利率 = 1,

        /// <summary>
        /// 季利率
        /// </summary>
        季利率 = 2,

        /// <summary>
        /// 月利率
        /// </summary>
        月利率 = 3,

        /// <summary>
        /// 天利率
        /// </summary>
        天利率 = 4
    }
}