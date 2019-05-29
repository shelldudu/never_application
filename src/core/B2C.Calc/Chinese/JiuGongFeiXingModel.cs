using System.Collections.Generic;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 飞星定义
    /// </summary>
    public class JiuGongFeiXingModel
    {
        #region property

        /// <summary>
        /// 数字
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 名字（全称）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 短名字
        /// </summary>
        public string ShorName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public WuXingType WuXing { get; set; }

        /// <summary>
        /// 九星吉凶
        /// </summary>
        public HuoFuType HuoFu { get; set; }

        #endregion property

        #region field

        /// <summary>
        /// 一白
        /// </summary>
        private static readonly JiuGongFeiXingModel 一白 = null;

        /// <summary>
        /// 二黑
        /// </summary>
        private static readonly JiuGongFeiXingModel 二黑 = null;

        /// <summary>
        /// 三碧
        /// </summary>
        private static readonly JiuGongFeiXingModel 三碧 = null;

        /// <summary>
        /// 四绿
        /// </summary>
        private static readonly JiuGongFeiXingModel 四绿 = null;

        /// <summary>
        /// 五黄
        /// </summary>
        private static readonly JiuGongFeiXingModel 五黄 = null;

        /// <summary>
        /// 六白
        /// </summary>
        private static readonly JiuGongFeiXingModel 六白 = null;

        /// <summary>
        /// 七赤
        /// </summary>
        private static readonly JiuGongFeiXingModel 七赤 = null;

        /// <summary>
        /// 八白
        /// </summary>
        private static readonly JiuGongFeiXingModel 八白 = null;

        /// <summary>
        /// 九紫
        /// </summary>
        private static readonly JiuGongFeiXingModel 九紫 = null;

        #endregion field

        #region ctor

        /// <summary>
        ///
        /// </summary>
        static JiuGongFeiXingModel()
        {
            一白 = new JiuGongFeiXingModel()
            {
                Num = 1,
                Name = "一白贪狼星",
                ShorName = "一白",
                HuoFu = HuoFuType.吉,
                WuXing = WuXingType.水
            };
            二黑 = new JiuGongFeiXingModel()
            {
                Num = 2,
                Name = "二黑巨门星",
                ShorName = "二黑",
                HuoFu = HuoFuType.凶,
                WuXing = WuXingType.土
            };
            三碧 = new JiuGongFeiXingModel()
            {
                Num = 3,
                Name = "三碧禄存星",
                ShorName = "三碧",
                HuoFu = HuoFuType.凶,
                WuXing = WuXingType.木
            };
            四绿 = new JiuGongFeiXingModel()
            {
                Num = 4,
                Name = "四绿文曲星",
                ShorName = "四绿",
                HuoFu = HuoFuType.吉,
                WuXing = WuXingType.木
            };
            五黄 = new JiuGongFeiXingModel()
            {
                Num = 5,
                Name = "五黄廉贞星",
                ShorName = "五黄",
                HuoFu = HuoFuType.凶,
                WuXing = WuXingType.土
            };
            六白 = new JiuGongFeiXingModel()
            {
                Num = 6,
                Name = "六白武曲星",
                ShorName = "六白",
                HuoFu = HuoFuType.吉,
                WuXing = WuXingType.金
            };
            七赤 = new JiuGongFeiXingModel()
            {
                Num = 7,
                Name = "七赤破军星",
                ShorName = "七赤",
                HuoFu = HuoFuType.凶,
                WuXing = WuXingType.金
            };
            八白 = new JiuGongFeiXingModel()
            {
                Num = 8,
                Name = "八白左辅星",
                ShorName = "八白",
                HuoFu = HuoFuType.吉,
                WuXing = WuXingType.土
            };
            九紫 = new JiuGongFeiXingModel()
            {
                Num = 9,
                Name = "九紫右弼星",
                ShorName = "九紫",
                HuoFu = HuoFuType.吉,
                WuXing = WuXingType.火
            };

            飞行顺序 = new Dictionary<int, JiuGongFeiXingModel>(9)
            {
                {1,JiuGongFeiXingModel.一白贪狼星},
                {2,JiuGongFeiXingModel.二黑巨门星},
                {3,JiuGongFeiXingModel.三碧禄存星},
                {4,JiuGongFeiXingModel.四绿文曲星},
                {5,JiuGongFeiXingModel.五黄廉贞星},
                {6,JiuGongFeiXingModel.六白武曲星},
                {7,JiuGongFeiXingModel.七赤破军星},
                {8,JiuGongFeiXingModel.八白左辅星},
                {9,JiuGongFeiXingModel.九紫右弼星}
            };
        }

        /// <summary>
        /// 不允许构造
        /// </summary>
        private JiuGongFeiXingModel()
        {
        }

        #endregion ctor

        #region 定义

        /// <summary>
        /// 一白
        /// </summary>
        public static JiuGongFeiXingModel 一白贪狼星
        {
            get
            {
                return 一白;
            }
        }

        /// <summary>
        /// 二黑
        /// </summary>
        public static JiuGongFeiXingModel 二黑巨门星
        {
            get
            {
                return 二黑;
            }
        }

        /// <summary>
        /// 三碧
        /// </summary>
        public static JiuGongFeiXingModel 三碧禄存星
        {
            get
            {
                return 三碧;
            }
        }

        /// <summary>
        /// 四绿
        /// </summary>
        public static JiuGongFeiXingModel 四绿文曲星
        {
            get
            {
                return 四绿;
            }
        }

        /// <summary>
        /// 五黄
        /// </summary>
        public static JiuGongFeiXingModel 五黄廉贞星
        {
            get
            {
                return 五黄;
            }
        }

        /// <summary>
        /// 六白
        /// </summary>
        public static JiuGongFeiXingModel 六白武曲星
        {
            get
            {
                return 六白;
            }
        }

        /// <summary>
        /// 七赤
        /// </summary>
        public static JiuGongFeiXingModel 七赤破军星
        {
            get
            {
                return 七赤;
            }
        }

        /// <summary>
        /// 八白
        /// </summary>
        public static JiuGongFeiXingModel 八白左辅星
        {
            get
            {
                return 八白;
            }
        }

        /// <summary>
        /// 九紫
        /// </summary>
        public static JiuGongFeiXingModel 九紫右弼星
        {
            get
            {
                return 九紫;
            }
        }

        #endregion 定义

        #region 根据数字获取相应的飞星

        /// <summary>
        ///
        /// </summary>
        /// <param name="number"></param>
        public static JiuGongFeiXingModel 获取飞星(int number)
        {
            switch (number % 10)
            {
                case 1:
                    {
                        return JiuGongFeiXingModel.一白贪狼星;
                    }
                case 2:
                    {
                        return JiuGongFeiXingModel.二黑巨门星;
                    }
                case 3:
                    {
                        return JiuGongFeiXingModel.三碧禄存星;
                    }
                case 4:
                    {
                        return JiuGongFeiXingModel.四绿文曲星;
                    }
                case 5:
                    {
                        return JiuGongFeiXingModel.五黄廉贞星;
                    }
                case 6:
                    {
                        return JiuGongFeiXingModel.六白武曲星;
                    }
                case 7:
                    {
                        return JiuGongFeiXingModel.七赤破军星;
                    }
                case 8:
                    {
                        return JiuGongFeiXingModel.八白左辅星;
                    }
                case 9:
                    {
                        return JiuGongFeiXingModel.九紫右弼星;
                    }
                default:
                    {
                        return JiuGongFeiXingModel.一白贪狼星;
                    }
            }
        }

        #endregion 根据数字获取相应的飞星

        #region 顺飞与逆飞

        /// <summary>
        ///
        /// </summary>
        private static readonly IDictionary<int, JiuGongFeiXingModel> 飞行顺序 = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="当前"></param>
        /// <returns></returns>
        public static JiuGongFeiXingModel 顺飞(JiuGongFeiXingModel 当前)
        {
            if (当前.Num == 9)
                return 飞行顺序[1];

            return 飞行顺序[当前.Num + 1];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="当前"></param>
        /// <returns></returns>
        public static JiuGongFeiXingModel 逆飞(JiuGongFeiXingModel 当前)
        {
            if (当前.Num == 1)
                return 飞行顺序[9];

            return 飞行顺序[当前.Num - 1];
        }

        #endregion 顺飞与逆飞
    }
}