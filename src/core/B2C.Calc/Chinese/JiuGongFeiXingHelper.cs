using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 九宫飞星
    /// </summary>
    public partial class JiuGongFeiXingHelper
    {
        #region 日紫白九星入中表Key

        /// <summary>
        /// 日紫白九星入中表
        /// </summary>
        private static readonly IDictionary<LiuShiJiaZiType, IDictionary<Tuple<JieQiEnum, JieQiEnum>, JiuGongFeiXingModel>> 日紫白九星入中表 = null;

        #endregion 日紫白九星入中表Key

        #region

        /// <summary>
        ///
        /// </summary>
        private static readonly IDictionary<int, IEnumerable<JiuGongFeiXingModel>> 中宫位置飞星列表 = null;

        #endregion

        #region static ctor

        /// <summary>
        ///
        /// </summary>
        static JiuGongFeiXingHelper()
        {
            中宫位置飞星列表 = new Dictionary<int, IEnumerable<JiuGongFeiXingModel>>()
            {
                {1,new []{JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.二黑巨门星}},
                {2,new []{JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.三碧禄存星}},
                {3,new []{JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.四绿文曲星}},
                {4,new []{JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.五黄廉贞星}},
                {5,new []{JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.六白武曲星}},
                {6,new []{JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.七赤破军星}},
                {7,new []{JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.八白左辅星}},
                {8,new []{JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.一白贪狼星,JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.九紫右弼星}},
                {9,new []{JiuGongFeiXingModel.八白左辅星,JiuGongFeiXingModel.四绿文曲星,JiuGongFeiXingModel.六白武曲星,JiuGongFeiXingModel.七赤破军星,JiuGongFeiXingModel.九紫右弼星,JiuGongFeiXingModel.二黑巨门星,JiuGongFeiXingModel.三碧禄存星,JiuGongFeiXingModel.五黄廉贞星,JiuGongFeiXingModel.一白贪狼星}}
            };

            日紫白九星入中表 = new Dictionary<LiuShiJiaZiType, IDictionary<Tuple<JieQiEnum, JieQiEnum>, JiuGongFeiXingModel>>()
            {
                { LiuShiJiaZiType.甲子,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                    {
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.一白贪狼星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.七赤破军星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.四绿文曲星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.九紫右弼星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.三碧禄存星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.六白武曲星 },
                    }
                },
                { LiuShiJiaZiType.乙丑,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                    {
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.二黑巨门星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.八白左辅星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.五黄廉贞星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.八白左辅星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.二黑巨门星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.五黄廉贞星 },
                    }
                },
                { LiuShiJiaZiType.丙寅,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                    {
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.三碧禄存星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.九紫右弼星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.六白武曲星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.七赤破军星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.一白贪狼星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.四绿文曲星 },
                    }
                },
                { LiuShiJiaZiType.丁卯,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                    {
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.四绿文曲星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.一白贪狼星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.七赤破军星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.六白武曲星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.九紫右弼星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.三碧禄存星 },
                    }
                },
                { LiuShiJiaZiType.戊辰,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                    {
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.五黄廉贞星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.二黑巨门星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.八白左辅星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.五黄廉贞星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.八白左辅星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.二黑巨门星 },
                    }
                },
                { LiuShiJiaZiType.己巳,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                    {
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.六白武曲星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.三碧禄存星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.九紫右弼星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.四绿文曲星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.七赤破军星 },
                        { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.一白贪狼星 },
                    }
                },
            { LiuShiJiaZiType.庚午,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.九紫右弼星 },
                }
            },
            { LiuShiJiaZiType.辛未,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.八白左辅星 },
                }
            },
            { LiuShiJiaZiType.壬申,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.七赤破军星 },
                }
            },
            { LiuShiJiaZiType.癸酉,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.六白武曲星 },
                }
            },
            { LiuShiJiaZiType.甲戌,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.五黄廉贞星 },
                }
            },
            { LiuShiJiaZiType.乙亥,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.四绿文曲星 },
                }
            },
            { LiuShiJiaZiType.丙子,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.三碧禄存星 },
                }
            },
            { LiuShiJiaZiType.丁丑,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.二黑巨门星 },
                }
            },
            { LiuShiJiaZiType.戊寅,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.一白贪狼星 },
                }
            },
            { LiuShiJiaZiType.己卯,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.九紫右弼星 },
                }
            },
            { LiuShiJiaZiType.庚辰,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.八白左辅星 },
                }
            },
            { LiuShiJiaZiType.辛巳,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.七赤破军星 },
                }
            },
            { LiuShiJiaZiType.壬午,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.六白武曲星 },
                }
            },
            { LiuShiJiaZiType.癸未,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.五黄廉贞星 },
                }
            },
            { LiuShiJiaZiType.甲申,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.四绿文曲星 },
                }
            },
            { LiuShiJiaZiType.乙酉,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.三碧禄存星 },
                }
            },
            { LiuShiJiaZiType.丙戌,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.二黑巨门星 },
                }
            },
            { LiuShiJiaZiType.丁亥,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.一白贪狼星 },
                }
            },
            { LiuShiJiaZiType.戊子,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.九紫右弼星 },
                }
            },
            { LiuShiJiaZiType.己丑,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.八白左辅星 },
                }
            },
            { LiuShiJiaZiType.庚寅,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.七赤破军星 },
                }
            },
            { LiuShiJiaZiType.辛卯,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.六白武曲星 },
                }
            },
            { LiuShiJiaZiType.壬辰,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.五黄廉贞星 },
                }
            },
            { LiuShiJiaZiType.癸巳,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.四绿文曲星 },
                }
            },
            { LiuShiJiaZiType.甲午,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.三碧禄存星 },
                }
            },
            { LiuShiJiaZiType.乙未,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.二黑巨门星 },
                }
            },
            { LiuShiJiaZiType.丙申,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.一白贪狼星 },
                }
            },
            { LiuShiJiaZiType.丁酉,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.九紫右弼星 },
                }
            },
            { LiuShiJiaZiType.戊戌,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.八白左辅星 },
                }
            },
            { LiuShiJiaZiType.己亥,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.七赤破军星 },
                }
            },
            { LiuShiJiaZiType.庚子,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.六白武曲星 },
                }
            },
            { LiuShiJiaZiType.辛丑,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.五黄廉贞星 },
                }
            },
            { LiuShiJiaZiType.壬寅,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.四绿文曲星 },
                }
            },
            { LiuShiJiaZiType.癸卯,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.三碧禄存星 },
                }
            },
            { LiuShiJiaZiType.甲辰,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.二黑巨门星 },
                }
            },
            { LiuShiJiaZiType.乙巳,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.一白贪狼星 },
                }
            },
            { LiuShiJiaZiType.丙午,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.九紫右弼星 },
                }
            },
            { LiuShiJiaZiType.丁未,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.八白左辅星 },
                }
            },
            { LiuShiJiaZiType.戊申,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.七赤破军星 },
                }
            },
            { LiuShiJiaZiType.己酉,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.六白武曲星 },
                }
            },
            { LiuShiJiaZiType.庚戌,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.五黄廉贞星 },
                }
            },
            { LiuShiJiaZiType.辛亥,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.四绿文曲星 },
                }
            },
            { LiuShiJiaZiType.壬子,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.三碧禄存星 },
                }
            },
            { LiuShiJiaZiType.癸丑,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.二黑巨门星 },
                }
            },
            { LiuShiJiaZiType.甲寅,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.一白贪狼星 },
                }
            },
            { LiuShiJiaZiType.乙卯,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.九紫右弼星 },
                }
            },
            { LiuShiJiaZiType.丙辰,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.八白左辅星 },
                }
            },
            { LiuShiJiaZiType.丁巳,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.七赤破军星 },
                }
            },
            { LiuShiJiaZiType.戊午,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.六白武曲星 },
                }
            },
            { LiuShiJiaZiType.己未,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.五黄廉贞星 },
                }
            },
            { LiuShiJiaZiType.庚申,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.四绿文曲星 },
                }
            },
            { LiuShiJiaZiType.辛酉,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.一白贪狼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.三碧禄存星 },
                }
            },
            { LiuShiJiaZiType.壬戌,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.二黑巨门星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.五黄廉贞星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.八白左辅星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.二黑巨门星 },
                }
            },
            { LiuShiJiaZiType.癸亥,new Dictionary<Tuple<JieQiEnum,JieQiEnum>,JiuGongFeiXingModel>()
                {
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.冬至,JieQiEnum.雨水),JiuGongFeiXingModel.六白武曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.雨水,JieQiEnum.谷雨),JiuGongFeiXingModel.三碧禄存星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.谷雨,JieQiEnum.夏至),JiuGongFeiXingModel.九紫右弼星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.夏至,JieQiEnum.处暑),JiuGongFeiXingModel.四绿文曲星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.处暑,JieQiEnum.霜降),JiuGongFeiXingModel.七赤破军星 },
                    { new Tuple<JieQiEnum,JieQiEnum>(JieQiEnum.霜降,JieQiEnum.冬至),JiuGongFeiXingModel.一白贪狼星 },
                }
            },
        };
        }

        #endregion

        #region utils

        /// <summary>
        /// 获取某年6个节气的时间
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static IDictionary<JieQiEnum, DateTime> 获取飞星6个节气时间(DateTime 公历时间)
        {
            var dict = new Dictionary<JieQiEnum, DateTime>(18);
            dict[JieQiEnum.雨水] = ChineseCalendar.计算节气日期(公历时间.Year, JieQiEnum.雨水);
            dict[JieQiEnum.谷雨] = ChineseCalendar.计算节气日期(公历时间.Year, JieQiEnum.谷雨);
            dict[JieQiEnum.夏至] = ChineseCalendar.计算节气日期(公历时间.Year, JieQiEnum.夏至);
            dict[JieQiEnum.处暑] = ChineseCalendar.计算节气日期(公历时间.Year, JieQiEnum.处暑);
            dict[JieQiEnum.霜降] = ChineseCalendar.计算节气日期(公历时间.Year, JieQiEnum.霜降);
            dict[JieQiEnum.冬至] = ChineseCalendar.计算节气日期(公历时间.Year, JieQiEnum.冬至);
            return dict;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="当前节气"></param>
        /// <returns></returns>
        public static JieQiEnum 获取当前节气上一个节气(JieQiEnum 当前节气)
        {
            switch (当前节气)
            {
                case JieQiEnum.雨水:
                    {
                        return JieQiEnum.冬至;
                    }
                case JieQiEnum.谷雨:
                    {
                        return JieQiEnum.雨水;
                    }
                case JieQiEnum.夏至:
                    {
                        return JieQiEnum.谷雨;
                    }
                case JieQiEnum.处暑:
                    {
                        return JieQiEnum.夏至;
                    }
                case JieQiEnum.霜降:
                    {
                        return JieQiEnum.处暑; ;
                    }
                case JieQiEnum.冬至:
                    {
                        return JieQiEnum.霜降;
                    }
                default:
                    {
                        return JieQiEnum.冬至;
                    }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="当前节气"></param>
        /// <returns></returns>
        public static JieQiEnum 获取当前节气下一个节气(JieQiEnum 当前节气)
        {
            switch (当前节气)
            {
                case JieQiEnum.雨水:
                    {
                        return JieQiEnum.谷雨;
                    }
                case JieQiEnum.谷雨:
                    {
                        return JieQiEnum.夏至;
                    }
                case JieQiEnum.夏至:
                    {
                        return JieQiEnum.处暑;
                    }
                case JieQiEnum.处暑:
                    {
                        return JieQiEnum.霜降;
                    }
                case JieQiEnum.霜降:
                    {
                        return JieQiEnum.冬至; ;
                    }
                case JieQiEnum.冬至:
                    {
                        return JieQiEnum.雨水;
                    }
                default:
                    {
                        return JieQiEnum.冬至;
                    }
            }
        }

        #endregion

        #region 获取每日飞星数字

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static IEnumerable<JiuGongFeiXingModel> 每日九宫飞星列表(DateTime 公历时间)
        {
            //用天干来获取60甲子列表
            var 天干 = TianGanDiZhiHelper.获取天干地支(公历时间);
            //这一天的纪日
            var 纪日 = 天干.TianGanDay.Name.ToString() + 天干.DiZhiDay.Name.ToString();

            var 纪日飞星列表 = 日紫白九星入中表[纪日.To60甲子()];

            //今年节气，从雨水开始到冬至结束
            var 今年节气时间列表 = 获取飞星6个节气时间(公历时间);
            //有些日期是
            IDictionary<JieQiEnum, DateTime> 上年节气时间列表 = null;
            if (ChineseCalendar.计算节气日期(公历时间.Year, JieQiEnum.雨水) >= 公历时间)
                上年节气时间列表 = 获取飞星6个节气时间(公历时间.AddYears(-1));

            //当前节气中宫所定居的飞星
            int 中宫们置 = 1;

            if (上年节气时间列表 != null)
            {
                //表明是上一年的冬至到今天的雨水期间
                中宫们置 = 纪日飞星列表.ElementAt(0).Value.Num;
                return 中宫位置飞星列表[中宫们置];
            }

            IDictionary<JieQiEnum, DateTime> 当年6个节气对应的时间 = new Dictionary<JieQiEnum, DateTime>()
            {
                {
                    JieQiEnum.雨水,
                    ChineseCalendar.计算节气日期(公历时间.Year,JieQiEnum.雨水)
                },
                {
                    JieQiEnum.谷雨,
                    ChineseCalendar.计算节气日期(公历时间.Year,JieQiEnum.谷雨)
                },
                {
                    JieQiEnum.夏至,
                    ChineseCalendar.计算节气日期(公历时间.Year,JieQiEnum.夏至)
                },
                {
                    JieQiEnum.处暑,
                    ChineseCalendar.计算节气日期(公历时间.Year,JieQiEnum.处暑)
                },
                {
                    JieQiEnum.霜降,
                    ChineseCalendar.计算节气日期(公历时间.Year,JieQiEnum.霜降)
                },
                {
                    JieQiEnum.冬至,
                    ChineseCalendar.计算节气日期(公历时间.Year,JieQiEnum.冬至)
                }
            };
            bool 是下年的时间 = true;

            for (var i = 0; i < 当年6个节气对应的时间.Count; i++)
            {
                var item = 当年6个节气对应的时间.ElementAt(i);
                int days = (item.Value - 公历时间).Days;
                if (days >= 0)
                {
                    Tuple<JieQiEnum, JieQiEnum> key = new Tuple<JieQiEnum, JieQiEnum>(获取当前节气上一个节气(item.Key), item.Key);
                    中宫们置 = 纪日飞星列表[key].Num;
                    是下年的时间 = false;
                    break;
                }
            }
            if (是下年的时间)
            {
                //是在下一年的
                return 中宫位置飞星列表[纪日飞星列表[new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.冬至, JieQiEnum.雨水)].Num];
            }

            return 中宫位置飞星列表[中宫们置];
        }

        #endregion
    }
}