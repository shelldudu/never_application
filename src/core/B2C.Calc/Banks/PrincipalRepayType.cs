namespace B2C.Calc.Banks
{
    /// <summary>
    /// 还款金额类型
    /// </summary>
    public class PrincipalRepayType
    {
        /// <summary>
        /// 是否本金期
        /// </summary>
        public readonly bool PayPrincipal;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalRepayType"/> class.
        /// </summary>
        /// <param name="isPayPrincipal">是否本金期</param>
        public PrincipalRepayType(bool isPayPrincipal)
        {
            this.PayPrincipal = isPayPrincipal;
        }
    }
}