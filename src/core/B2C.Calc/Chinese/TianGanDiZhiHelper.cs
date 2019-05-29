using System;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 天干地支
    /// </summary>
    public partial class TianGanDiZhiHelper
    {
        #region 获取某天的天干与地支

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static TianGanDiZhiModel 获取天干地支(DateTime 公历时间)
        {
            var 纪月 = ChineseCalendar.计算纪月(公历时间);
            var 纪时 = ChineseCalendar.计算纪时(公历时间);

            var result = new TianGanDiZhiModel()
            {
                TianGanYear = new TianGanModel() { Name = ChineseCalendar.计算纪年天干(公历时间) },
                TianGanMonth = new TianGanModel() { Name = 纪月.ToString().Substring(0, 1).To天干() },
                TianGanDay = new TianGanModel() { Name = ChineseCalendar.计算纪日天干(公历时间) },
                TianGanHour = new TianGanModel() { Name = 纪时.ToString().Substring(0, 1).To天干() },

                DiZhiYear = new DiZhiModel() { Name = ChineseCalendar.计算纪年地支(公历时间) },
                DiZhiMonth = new DiZhiModel() { Name = 纪月.ToString().Substring(1, 1).To地支() },
                DiZhiDay = new DiZhiModel() { Name = ChineseCalendar.计算纪日地支(公历时间) },
                DiZhiHour = new DiZhiModel() { Name = 纪时.ToString().Substring(1, 1).To地支() },
            };

            return result;
        }

        #endregion 获取某天的天干与地支
    }
}