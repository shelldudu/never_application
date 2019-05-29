namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 黄历
    /// </summary>
    public partial class HuangLiModel
    {
        #region 天干地支

        /// <summary>
        /// 天干地支
        /// </summary>
        public TianGanDiZhiModel TianGanDiZhi { get; set; }

        #endregion 天干地支

        #region 农历

        /// <summary>
        /// 农历年（用中文描述）
        /// </summary>
        public string NongLiYearDescn { get; set; }

        /// <summary>
        /// 农历年（数字）
        /// </summary>
        public int NongLiYear { get; set; }

        /// <summary>
        /// 农历月（用中文描述）
        /// </summary>
        public string NongLiMonthDescn { get; set; }

        /// <summary>
        /// 农历月（数字）
        /// </summary>
        public int NongLiMonth { get; set; }

        /// <summary>
        /// 农历日（用中文描述）
        /// </summary>
        public string NongLiDayDescn { get; set; }

        /// <summary>
        /// 农历日（数字）
        /// </summary>
        public int NongLiDay { get; set; }

        /// <summary>
        /// 是否闰年
        /// </summary>
        public bool IsLeapYear { get; set; }

        /// <summary>
        /// 是否为闰月
        /// </summary>
        public bool IsLeapMonth { get; set; }

        /// <summary>
        /// 闰月
        /// </summary>
        public int LeapMonth { get; set; }

        #endregion 农历

        #region 公历

        /// <summary>
        /// 公历年（用中文描述）
        /// </summary>
        public string GongLiYearDescn { get; set; }

        /// <summary>
        /// 公历年（数字）
        /// </summary>
        public int GongLiYear { get; set; }

        /// <summary>
        /// 公历月（用中文描述）
        /// </summary>
        public string GongLiMonthDescn { get; set; }

        /// <summary>
        /// 公历月（数字）
        /// </summary>
        public int GongLiMonth { get; set; }

        /// <summary>
        /// 公历日（用中文描述）
        /// </summary>
        public string GongLiDayDescn { get; set; }

        /// <summary>
        /// 公历日（数字）
        /// </summary>
        public int GongLiDay { get; set; }

        #endregion 公历

        #region 生肖

        /// <summary>
        /// 生肖
        /// </summary>
        public ShiErShengXiaoType ShengXiao { get; set; }

        #endregion 生肖

        #region cotr

        /// <summary>
        ///
        /// </summary>
        static HuangLiModel()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public HuangLiModel()
        {
            this.TianGanDiZhi = new TianGanDiZhiModel();
        }

        #endregion cotr
    }
}