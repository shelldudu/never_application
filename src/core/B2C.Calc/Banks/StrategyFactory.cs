using B2C.Calc.Banks.Strategies;
using System.Collections.Generic;

namespace B2C.Calc.Banks
{
    /// <summary>
    /// 策略工厂
    /// </summary>
    internal class StrategyFactory
    {
        #region field

        /// <summary>
        /// 结算字典
        /// </summary>
        private readonly static IDictionary<RepaymentTimeUnitType, IDateTypeStrategy> dict = null;

        #endregion field

        #region ctor

        /// <summary>
        /// Initializes static members of the <see cref="StrategyFactory"/> class.
        /// </summary>
        static StrategyFactory()
        {
            dict = new Dictionary<RepaymentTimeUnitType, IDateTypeStrategy>(4);
            dict[RepaymentTimeUnitType.年] = new YearStrategy();
            dict[RepaymentTimeUnitType.季] = new SeasonStrategy();
            dict[RepaymentTimeUnitType.月] = new MonthStrategy();
            dict[RepaymentTimeUnitType.天] = new DayStrategy();
        }

        #endregion ctor

        #region index

        /// <summary>
        /// 返回结算策略对象 <see cref="IDateTypeStrategy"/>
        /// </summary>
        /// <value>
        /// The <see cref="IDateTypeStrategy"/>.
        /// </value>
        /// <param name="type">结算方式</param>
        /// <returns></returns>
        public IDateTypeStrategy this[RepaymentTimeUnitType type]
        {
            get
            {
                return dict.ContainsKey(type) ? dict[type] : null;
            }
        }

        #endregion index
    }
}