using System;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 农历
    /// </summary>
    public class NongLiModel
    {
        #region property

        /// <summary>
        ///
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string YearDescn { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string MonthDescn { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string DayDescn { get; set; }

        /// <summary>
        /// 是否闰年
        /// </summary>
        public bool IsLeapYear { get; set; }

        /// <summary>
        /// 是否为闰月
        /// </summary>
        public bool IsLeapMonth { get; set; }

        /// <summary>
        /// 闰月月份
        /// </summary>
        public int LeapMonth { get; set; }

        /// <summary>
        ///
        /// </summary>
        public readonly DateTime 公历时间;

        #endregion property

        #region ctor

        /// <summary>
        ///
        /// </summary>
        static NongLiModel()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        public NongLiModel(DateTime 公历时间)
        {
            this.公历时间 = 公历时间;
        }

        #endregion ctor
    }
}