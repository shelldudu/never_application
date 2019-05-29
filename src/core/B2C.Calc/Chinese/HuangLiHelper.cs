using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 黄历
    /// </summary>
    public partial class HuangLiHelper
    {
        #region ctor

        /// <summary>
        ///
        /// </summary>
        static HuangLiHelper()
        {
            //喜神方位
            喜神方位 = new Dictionary<TianGanType, FangXiangType>(10);
            喜神方位.Add(TianGanType.甲, FangXiangType.西北);
            喜神方位.Add(TianGanType.己, FangXiangType.西北);
            喜神方位.Add(TianGanType.乙, FangXiangType.西北);
            喜神方位.Add(TianGanType.庚, FangXiangType.西北);

            喜神方位.Add(TianGanType.丙, FangXiangType.西南);
            喜神方位.Add(TianGanType.辛, FangXiangType.西南);

            喜神方位.Add(TianGanType.丁, FangXiangType.正南);
            喜神方位.Add(TianGanType.壬, FangXiangType.正南);

            喜神方位.Add(TianGanType.戊, FangXiangType.东南);
            喜神方位.Add(TianGanType.癸, FangXiangType.东南);

            //贵神方位
            贵神方位 = new Dictionary<LiuShiJiaZiType, FangXiangType>(60);
            //1-10
            贵神方位.Add(LiuShiJiaZiType.甲子, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.乙丑, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.丙寅, FangXiangType.正西);
            贵神方位.Add(LiuShiJiaZiType.丁卯, FangXiangType.西北);
            贵神方位.Add(LiuShiJiaZiType.戊辰, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.己巳, FangXiangType.正北);
            贵神方位.Add(LiuShiJiaZiType.庚午, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.辛未, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.壬申, FangXiangType.正东);
            贵神方位.Add(LiuShiJiaZiType.癸酉, FangXiangType.东南);
            //11-20
            贵神方位.Add(LiuShiJiaZiType.甲午, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.乙未, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.丙申, FangXiangType.正西);
            贵神方位.Add(LiuShiJiaZiType.丁酉, FangXiangType.西北);
            贵神方位.Add(LiuShiJiaZiType.戊戌, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.己亥, FangXiangType.正北);
            贵神方位.Add(LiuShiJiaZiType.庚子, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.辛丑, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.壬寅, FangXiangType.正东);
            贵神方位.Add(LiuShiJiaZiType.癸卯, FangXiangType.东南);
            //21-30
            贵神方位.Add(LiuShiJiaZiType.甲戌, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.乙亥, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.丙子, FangXiangType.正西);
            贵神方位.Add(LiuShiJiaZiType.丁丑, FangXiangType.西北);
            贵神方位.Add(LiuShiJiaZiType.戊寅, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.己卯, FangXiangType.正北);
            贵神方位.Add(LiuShiJiaZiType.庚辰, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.辛巳, FangXiangType.正东);
            贵神方位.Add(LiuShiJiaZiType.壬午, FangXiangType.正东);
            贵神方位.Add(LiuShiJiaZiType.癸未, FangXiangType.东南);
            //31-40
            贵神方位.Add(LiuShiJiaZiType.甲辰, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.乙巳, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.丙午, FangXiangType.正西);
            贵神方位.Add(LiuShiJiaZiType.丁未, FangXiangType.西北);
            贵神方位.Add(LiuShiJiaZiType.戊申, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.己酉, FangXiangType.正北);
            贵神方位.Add(LiuShiJiaZiType.庚戌, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.辛亥, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.壬子, FangXiangType.正东);
            贵神方位.Add(LiuShiJiaZiType.癸丑, FangXiangType.东南);
            //41-50
            贵神方位.Add(LiuShiJiaZiType.甲申, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.乙酉, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.丙戌, FangXiangType.正西);
            贵神方位.Add(LiuShiJiaZiType.丁亥, FangXiangType.西北);
            贵神方位.Add(LiuShiJiaZiType.戊子, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.己丑, FangXiangType.正北);
            贵神方位.Add(LiuShiJiaZiType.庚寅, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.辛卯, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.壬辰, FangXiangType.正东);
            贵神方位.Add(LiuShiJiaZiType.癸巳, FangXiangType.东南);
            //51-60
            贵神方位.Add(LiuShiJiaZiType.甲寅, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.乙卯, FangXiangType.西南);
            贵神方位.Add(LiuShiJiaZiType.丙辰, FangXiangType.正西);
            贵神方位.Add(LiuShiJiaZiType.丁巳, FangXiangType.正北);
            贵神方位.Add(LiuShiJiaZiType.戊午, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.己未, FangXiangType.正北);
            贵神方位.Add(LiuShiJiaZiType.庚申, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.辛酉, FangXiangType.东北);
            贵神方位.Add(LiuShiJiaZiType.壬戌, FangXiangType.正东);
            贵神方位.Add(LiuShiJiaZiType.癸亥, FangXiangType.东南);

            //财神方位
            财神方位 = new Dictionary<TianGanType, FangXiangType>(10);
            财神方位.Add(TianGanType.甲, FangXiangType.东北);
            财神方位.Add(TianGanType.乙, FangXiangType.东北);

            财神方位.Add(TianGanType.丙, FangXiangType.西南);
            财神方位.Add(TianGanType.丁, FangXiangType.西南);

            财神方位.Add(TianGanType.戊, FangXiangType.正北);
            财神方位.Add(TianGanType.己, FangXiangType.正北);

            财神方位.Add(TianGanType.庚, FangXiangType.正东);
            财神方位.Add(TianGanType.辛, FangXiangType.正东);

            财神方位.Add(TianGanType.壬, FangXiangType.正南);
            财神方位.Add(TianGanType.癸, FangXiangType.正南);

            //生门方位
            生门方位 = new Dictionary<LiuShiJiaZiType, FangXiangType>(60);
            生门方位.Add(LiuShiJiaZiType.甲子, FangXiangType.东北);
            生门方位.Add(LiuShiJiaZiType.乙丑, FangXiangType.东北);
            生门方位.Add(LiuShiJiaZiType.丙寅, FangXiangType.东北);

            生门方位.Add(LiuShiJiaZiType.丁卯, FangXiangType.正西);
            生门方位.Add(LiuShiJiaZiType.戊辰, FangXiangType.正西);
            生门方位.Add(LiuShiJiaZiType.己巳, FangXiangType.正西);

            生门方位.Add(LiuShiJiaZiType.庚午, FangXiangType.东南);
            生门方位.Add(LiuShiJiaZiType.辛未, FangXiangType.东南);
            生门方位.Add(LiuShiJiaZiType.壬申, FangXiangType.东南);

            生门方位.Add(LiuShiJiaZiType.癸酉, FangXiangType.正南);
            生门方位.Add(LiuShiJiaZiType.甲戌, FangXiangType.正南);
            生门方位.Add(LiuShiJiaZiType.乙亥, FangXiangType.正南);

            生门方位.Add(LiuShiJiaZiType.丙子, FangXiangType.正北);
            生门方位.Add(LiuShiJiaZiType.丁丑, FangXiangType.正北);
            生门方位.Add(LiuShiJiaZiType.戊寅, FangXiangType.正北);

            生门方位.Add(LiuShiJiaZiType.己卯, FangXiangType.西北);
            生门方位.Add(LiuShiJiaZiType.庚辰, FangXiangType.西北);
            生门方位.Add(LiuShiJiaZiType.辛巳, FangXiangType.西北);

            生门方位.Add(LiuShiJiaZiType.壬午, FangXiangType.正东);
            生门方位.Add(LiuShiJiaZiType.癸未, FangXiangType.正东);
            生门方位.Add(LiuShiJiaZiType.甲申, FangXiangType.正东);

            生门方位.Add(LiuShiJiaZiType.乙酉, FangXiangType.西南);
            生门方位.Add(LiuShiJiaZiType.丙戌, FangXiangType.西南);
            生门方位.Add(LiuShiJiaZiType.丁亥, FangXiangType.西南);

            生门方位.Add(LiuShiJiaZiType.戊子, FangXiangType.东北);
            生门方位.Add(LiuShiJiaZiType.己丑, FangXiangType.东北);
            生门方位.Add(LiuShiJiaZiType.庚寅, FangXiangType.东北);

            生门方位.Add(LiuShiJiaZiType.辛卯, FangXiangType.正西);
            生门方位.Add(LiuShiJiaZiType.壬辰, FangXiangType.正西);
            生门方位.Add(LiuShiJiaZiType.癸巳, FangXiangType.正西);

            生门方位.Add(LiuShiJiaZiType.甲午, FangXiangType.东南);
            生门方位.Add(LiuShiJiaZiType.乙未, FangXiangType.东南);
            生门方位.Add(LiuShiJiaZiType.丙申, FangXiangType.东南);

            生门方位.Add(LiuShiJiaZiType.丁酉, FangXiangType.正南);
            生门方位.Add(LiuShiJiaZiType.戊戌, FangXiangType.正南);
            生门方位.Add(LiuShiJiaZiType.己亥, FangXiangType.正南);

            生门方位.Add(LiuShiJiaZiType.庚子, FangXiangType.正北);
            生门方位.Add(LiuShiJiaZiType.辛丑, FangXiangType.正北);
            生门方位.Add(LiuShiJiaZiType.壬寅, FangXiangType.正北);

            生门方位.Add(LiuShiJiaZiType.癸卯, FangXiangType.西北);
            生门方位.Add(LiuShiJiaZiType.甲辰, FangXiangType.西北);
            生门方位.Add(LiuShiJiaZiType.乙巳, FangXiangType.西北);

            生门方位.Add(LiuShiJiaZiType.丙午, FangXiangType.正东);
            生门方位.Add(LiuShiJiaZiType.丁未, FangXiangType.正东);
            生门方位.Add(LiuShiJiaZiType.戊申, FangXiangType.正东);

            生门方位.Add(LiuShiJiaZiType.己酉, FangXiangType.西南);
            生门方位.Add(LiuShiJiaZiType.庚戌, FangXiangType.西南);
            生门方位.Add(LiuShiJiaZiType.辛亥, FangXiangType.西南);

            生门方位.Add(LiuShiJiaZiType.壬子, FangXiangType.东北);
            生门方位.Add(LiuShiJiaZiType.癸丑, FangXiangType.东北);
            生门方位.Add(LiuShiJiaZiType.甲寅, FangXiangType.东北);

            生门方位.Add(LiuShiJiaZiType.乙卯, FangXiangType.正西);
            生门方位.Add(LiuShiJiaZiType.丙辰, FangXiangType.正西);
            生门方位.Add(LiuShiJiaZiType.丁巳, FangXiangType.正西);

            生门方位.Add(LiuShiJiaZiType.戊午, FangXiangType.东南);
            生门方位.Add(LiuShiJiaZiType.己未, FangXiangType.东南);
            生门方位.Add(LiuShiJiaZiType.庚申, FangXiangType.东南);

            生门方位.Add(LiuShiJiaZiType.辛酉, FangXiangType.正南);
            生门方位.Add(LiuShiJiaZiType.壬戌, FangXiangType.正南);
            生门方位.Add(LiuShiJiaZiType.癸亥, FangXiangType.正南);
        }

        #endregion ctor

        #region 吉神方位

        /// <summary>
        /// 喜神
        /// </summary>
        private static readonly IDictionary<TianGanType, FangXiangType> 喜神方位 = null;

        /// <summary>
        /// 贵神
        /// </summary>
        private static readonly IDictionary<LiuShiJiaZiType, FangXiangType> 贵神方位 = null;

        /// <summary>
        /// 财神
        /// </summary>
        private static readonly IDictionary<TianGanType, FangXiangType> 财神方位 = null;

        /// <summary>
        /// 生门
        /// </summary>
        public static IDictionary<LiuShiJiaZiType, FangXiangType> 生门方位 = null;

        #endregion 吉神方位

        #region 获取农历

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static HuangLiModel 获取黄历(DateTime 公历时间)
        {
            var nongli = ChineseCalendar.计算农历(公历时间);
            var result = new HuangLiModel()
            {
                TianGanDiZhi = TianGanDiZhiHelper.获取天干地支(公历时间),
                NongLiYearDescn = nongli.YearDescn,
                NongLiYear = nongli.Year,
                NongLiMonthDescn = nongli.MonthDescn,
                NongLiMonth = nongli.Month,
                NongLiDayDescn = nongli.DayDescn,
                NongLiDay = nongli.Day,

                GongLiYear = 公历时间.Year,
                GongLiYearDescn = 公历时间.Year.ToString(),
                GongLiMonth = 公历时间.Month,
                GongLiMonthDescn = 公历时间.Month.ToString(),
                GongLiDay = 公历时间.Day,
                GongLiDayDescn = 公历时间.Day.ToString(),

                ShengXiao = ChineseCalendar.计算生肖(公历时间),

                IsLeapYear = nongli.IsLeapYear,
                IsLeapMonth = nongli.IsLeapMonth,
                LeapMonth = nongli.LeapMonth
            };
            return result;
        }

        #endregion 获取农历

        #region 获取喜神方位

        /// <summary>
        /// 用十天干作为排序方法，排出出每一天的喜神方位
        /// </summary>
        /// <param name="天干"></param>
        /// <returns></returns>
        public static FangXiangType 获取喜神方位(TianGanModel 天干)
        {
            if (天干 == null)
                throw new ArgumentNullException("天干不能为空");

            if (!喜神方位.ContainsKey(天干.Name))
                throw new ArgumentNullException("喜神方位中没有定义该天干信息");

            return 喜神方位[天干.Name];
        }

        /// <summary>
        /// 用十天干作为排序方法，排出出每一天的喜神方位
        /// </summary>
        /// <param name="天干地支"></param>
        /// <returns></returns>
        public static FangXiangType 获取喜神方位(TianGanDiZhiModel 天干地支)
        {
            if (天干地支 == null || 天干地支.TianGanDay == null)
                throw new ArgumentNullException("天干地支不能为空");

            return 获取喜神方位(天干地支.TianGanDay);
        }

        /// <summary>
        /// 用某天十天干作为排序方法，排出出每一天的喜神方位
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static FangXiangType 获取喜神方位(DateTime 公历时间)
        {
            var 天干 = TianGanDiZhiHelper.获取天干地支(公历时间);
            return 获取喜神方位(天干);
        }

        #endregion 获取喜神方位

        #region 获取贵神方位

        /// <summary>
        /// 用天干与地支组合的六十甲子作为排序方法，排出出每一天的贵神方位
        /// </summary>
        /// <param name="天干"></param>
        /// <returns></returns>
        public static FangXiangType 获取贵神方位(TianGanDiZhiModel 天干)
        {
            if (天干 == null)
                throw new ArgumentNullException("天干不能为空");

            var key = string.Concat(天干.TianGanDay.Name.ToString(), 天干.DiZhiDay.Name.ToString()).To60甲子();

            if (!贵神方位.ContainsKey(key))
                throw new ArgumentNullException("贵神方位中没有定义该天干信息");

            return 贵神方位[key];
        }

        /// <summary>
        /// 用当天干与地支组合的六十甲子作为排序方法，排出出每一天的贵神方位
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static FangXiangType 获取贵神方位(DateTime 公历时间)
        {
            var 天干 = TianGanDiZhiHelper.获取天干地支(公历时间);
            return 获取贵神方位(天干);
        }

        #endregion 获取贵神方位

        #region 获取财神方位

        /// <summary>
        /// 用某天十天干作为排序方法，排出出每一天的喜神方位
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static FangXiangType 获取财神方位(DateTime 公历时间)
        {
            var 天干 = TianGanDiZhiHelper.获取天干地支(公历时间);
            return 获取财神方位(天干.TianGanDay);
        }

        /// <summary>
        /// 用十天干作为排序方法，排出出每一天的喜神方位
        /// </summary>
        /// <param name="天干"></param>
        /// <returns></returns>
        public static FangXiangType 获取财神方位(TianGanModel 天干)
        {
            if (天干 == null)
                throw new ArgumentNullException("天干不能为空");

            if (!财神方位.ContainsKey(天干.Name))
                throw new ArgumentNullException("财神方位中没有定义该天干信息");

            return 财神方位[天干.Name];
        }

        #endregion 获取财神方位

        #region 获取生门方位

        /// <summary>
        /// 用天干与地支组合的六十甲子作为排序方法，排出出每一天的生门方位
        /// </summary>
        /// <param name="天干"></param>
        /// <returns></returns>
        public static FangXiangType 获取生门方位(TianGanDiZhiModel 天干)
        {
            if (天干 == null)
                throw new ArgumentNullException("天干不能为空");

            var key = string.Concat(天干.TianGanDay.Name.ToString(), 天干.DiZhiDay.Name.ToString()).To60甲子();
            if (!生门方位.ContainsKey(key))
                throw new ArgumentNullException("生门方位中没有定义该天干信息");

            return 生门方位[key];
        }

        /// <summary>
        /// 用天干与地支组合的六十甲子作为排序方法，排出出每一天的生门方位
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static FangXiangType 获取生门方位(DateTime 公历时间)
        {
            var 天干 = TianGanDiZhiHelper.获取天干地支(公历时间);
            return 获取生门方位(天干);
        }

        #endregion 获取生门方位

        #region 获取公历时间对应农历月份信息

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static IList<DateTime> 获取公历时间对应农历月份信息(DateTime 公历时间)
        {
            var 黄历 = 获取黄历(公历时间);
            var result = new List<DateTime>(2);
            //相差多少天
            var 农历 = new DateTime(黄历.NongLiYear, 黄历.NongLiMonth, 黄历.NongLiDay);// new DateTime(天干.NongLiYear, 天干.NongLiMonth, 1);

            //计算出当时农历过了几天
            int diff = (农历 - new DateTime(黄历.NongLiYear, 黄历.NongLiMonth, 1)).Days;

            result.Add(公历时间.AddDays(0 - diff));
            result.Add(result.ElementAt(0).AddMonths(1).AddDays(-1));

            return result;
        }

        #endregion 获取公历时间对应农历月份信息
    }
}