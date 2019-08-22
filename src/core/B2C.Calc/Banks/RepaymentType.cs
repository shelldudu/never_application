namespace B2C.Calc.Banks
{
    /// <summary>
    /// 还款方式
    /// </summary>
    public enum RepaymentType
    {
        /// <summary>
        /// 一次性还本付息
        /// </summary>
        一次性还本付息 = 1,

        /// <summary>
        /// 等额本息
        /// </summary>
        等额本息 = 2,

        /// <summary>
        /// 等额本金
        /// </summary>
        等额本金 = 3,

        /// <summary>
        /// 按期付息到期还本
        /// </summary>
        按期付息到期还本 = 4,

        /// <summary>
        /// 等本等息
        /// </summary>
        等本等息 = 5,

        /// <summary>
        /// 按期付息N期还本，每一次还本金后利息计算是以原来本金计算
        /// </summary>
        按期付息N期还本 = 6,

        /// <summary>
        /// 按期付息L期还本，每一次还本金后利息计算是以剩余本金计算
        /// </summary>
        按期付息L期还本 = 7,

        /// <summary>
        /// 本金按百分比还
        /// </summary>
        本金按百分比还 = 8,
    }
}