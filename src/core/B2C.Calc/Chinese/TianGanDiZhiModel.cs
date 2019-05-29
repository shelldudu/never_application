namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 天干地支(干支)
    /// </summary>
    public class TianGanDiZhiModel
    {
        #region 天干Property

        /// <summary>
        /// 天干年
        /// </summary>
        public TianGanModel TianGanYear { get; set; }

        /// <summary>
        /// 天干月
        /// </summary>
        public TianGanModel TianGanMonth { get; set; }

        /// <summary>
        /// 天干日
        /// </summary>
        public TianGanModel TianGanDay { get; set; }

        /// <summary>
        /// 天干小时
        /// </summary>
        public TianGanModel TianGanHour { get; set; }

        #endregion 天干Property

        #region 地支Property

        /// <summary>
        /// 地支年
        /// </summary>
        public DiZhiModel DiZhiYear { get; set; }

        /// <summary>
        /// 地支月
        /// </summary>
        public DiZhiModel DiZhiMonth { get; set; }

        /// <summary>
        /// 地支日
        /// </summary>
        public DiZhiModel DiZhiDay { get; set; }

        /// <summary>
        /// 地支小时
        /// </summary>
        public DiZhiModel DiZhiHour { get; set; }

        #endregion 地支Property

        #region ctor

        /// <summary>
        ///
        /// </summary>
        public TianGanDiZhiModel()
        {
            this.DiZhiDay = new DiZhiModel();
            this.DiZhiHour = new DiZhiModel();
            this.DiZhiMonth = new DiZhiModel();
            this.DiZhiYear = new DiZhiModel();

            this.TianGanDay = new TianGanModel();
            this.TianGanHour = new TianGanModel();
            this.TianGanMonth = new TianGanModel();
            this.TianGanYear = new TianGanModel();
        }

        #endregion ctor
    }
}