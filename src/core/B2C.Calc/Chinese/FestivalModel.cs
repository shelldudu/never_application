using System;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 节日信息
    /// </summary>
    [Serializable]
    public class FestivalModel
    {
        /// <summary>
        /// 节日名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节日时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 节日描述
        /// </summary>
        public string Descn { get; set; }

        /// <summary>
        /// 节日类型
        /// </summary>
        public FestivalType Type { get; set; }
    }
}