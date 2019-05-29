using System;
using System.Collections.Generic;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class ChineseExtension
    {
        #region 中文年份

        private static readonly IDictionary<int, string> 零到十中文字典 = new Dictionary<int, string>()
        {
            {0,"零"},
            {1,"一"},
            {2,"二"},
            {3,"三"},
            {4,"四"},
            {5,"五"},
            {6,"六"},
            {7,"七"},
            {8,"八"},
            {9,"九"},
            {10,"十"}
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string To中文年份(this int year)
        {
            int four = 0, three = 0, two = 0, first = 0;
            four = (year / 1000);
            three = (year - 1000 * four) / 100;
            two = (year - four * 1000 - three * 100) / 10;
            first = (year - four * 1000 - three * 100 - two * 10);

            return string.Concat(零到十中文字典[four], 零到十中文字典[three], 零到十中文字典[two], 零到十中文字典[first]);
        }

        #endregion 中文年份

        #region 中文月份

        /// <summary>
        ///
        /// </summary>
        private static IDictionary<int, string> intDescnDict = new Dictionary<int, string>(7)
        {
            { 1 , "一" },
            { 2 , "二" },
            { 3 , "三" },
            { 4 , "四" },
            { 5 , "五" },
            { 6 , "六" },
            { 7 , "七" },
            { 8 , "八" },
            { 9 , "九" },
            { 10 , "十" },
            { 11 , "十一" },
            { 12 , "十二" }
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>

        public static string ChineseWordDescnForNum(this int number)
        {
            return intDescnDict[number];
        }

        #endregion 中文月份

        #region 星期

        /// <summary>
        ///
        /// </summary>
        private static IDictionary<DayOfWeek, string> weekDict = new Dictionary<DayOfWeek, string>(7)
        {
            { DayOfWeek.Friday , "星期五" },
            { DayOfWeek.Monday , "星期一" },
            { DayOfWeek.Saturday , "星期六"},
            { DayOfWeek.Sunday  ,"星期天"},
            { DayOfWeek.Thursday , "星期四"},
            { DayOfWeek.Tuesday , "星期二"},
            { DayOfWeek.Wednesday , "星期三"}
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>

        public static string WeekDescn(this DateTime time)
        {
            return weekDict[time.DayOfWeek];
        }

        #endregion 星期

        #region 60甲子转换

        /// <summary>
        ///
        /// </summary>
        private static readonly IDictionary<string, LiuShiJiaZiType> _60甲子字典 = new Dictionary<string, LiuShiJiaZiType>(60)
        {
            {"甲子",LiuShiJiaZiType.甲子},
            {"乙丑",LiuShiJiaZiType.乙丑},
            {"丙寅",LiuShiJiaZiType.丙寅},
            {"丁卯",LiuShiJiaZiType.丁卯},
            {"戊辰",LiuShiJiaZiType.戊辰},
            {"己巳",LiuShiJiaZiType.己巳},
            {"庚午",LiuShiJiaZiType.庚午},
            {"辛未",LiuShiJiaZiType.辛未},
            {"壬申",LiuShiJiaZiType.壬申},
            {"癸酉",LiuShiJiaZiType.癸酉},
            {"甲戌",LiuShiJiaZiType.甲戌},
            {"乙亥",LiuShiJiaZiType.乙亥},
            {"丙子",LiuShiJiaZiType.丙子},
            {"丁丑",LiuShiJiaZiType.丁丑},
            {"戊寅",LiuShiJiaZiType.戊寅},
            {"己卯",LiuShiJiaZiType.己卯},
            {"庚辰",LiuShiJiaZiType.庚辰},
            {"辛巳",LiuShiJiaZiType.辛巳},
            {"壬午",LiuShiJiaZiType.壬午},
            {"癸未",LiuShiJiaZiType.癸未},
            {"甲申",LiuShiJiaZiType.甲申},
            {"乙酉",LiuShiJiaZiType.乙酉},
            {"丙戌",LiuShiJiaZiType.丙戌},
            {"丁亥",LiuShiJiaZiType.丁亥},
            {"戊子",LiuShiJiaZiType.戊子},
            {"己丑",LiuShiJiaZiType.己丑},
            {"庚寅",LiuShiJiaZiType.庚寅},
            {"辛卯",LiuShiJiaZiType.辛卯},
            {"壬辰",LiuShiJiaZiType.壬辰},
            {"癸巳",LiuShiJiaZiType.癸巳},
            {"甲午",LiuShiJiaZiType.甲午},
            {"乙未",LiuShiJiaZiType.乙未},
            {"丙申",LiuShiJiaZiType.丙申},
            {"丁酉",LiuShiJiaZiType.丁酉},
            {"戊戌",LiuShiJiaZiType.戊戌},
            {"己亥",LiuShiJiaZiType.己亥},
            {"庚子",LiuShiJiaZiType.庚子},
            {"辛丑",LiuShiJiaZiType.辛丑},
            {"壬寅",LiuShiJiaZiType.壬寅},
            {"癸丑",LiuShiJiaZiType.癸丑},
            {"甲辰",LiuShiJiaZiType.甲辰},
            {"乙巳",LiuShiJiaZiType.乙巳},
            {"丙午",LiuShiJiaZiType.丙午},
            {"丁未",LiuShiJiaZiType.丁未},
            {"戊申",LiuShiJiaZiType.戊申},
            {"己酉",LiuShiJiaZiType.己酉},
            {"庚戌",LiuShiJiaZiType.庚戌},
            {"辛亥",LiuShiJiaZiType.辛亥},
            {"壬子",LiuShiJiaZiType.壬子},
            {"癸卯",LiuShiJiaZiType.癸卯},
            {"甲寅",LiuShiJiaZiType.甲寅},
            {"乙卯",LiuShiJiaZiType.乙卯},
            {"丙辰",LiuShiJiaZiType.丙辰},
            {"丁巳",LiuShiJiaZiType.丁巳},
            {"戊午",LiuShiJiaZiType.戊午},
            {"己未",LiuShiJiaZiType.己未},
            {"庚申",LiuShiJiaZiType.庚申},
            {"辛酉",LiuShiJiaZiType.辛酉},
            {"壬戌",LiuShiJiaZiType.壬戌},
            {"癸亥",LiuShiJiaZiType.癸亥}
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="天干地支"></param>
        /// <returns></returns>
        public static LiuShiJiaZiType To60甲子(this string 天干地支)
        {
            if (_60甲子字典.ContainsKey(天干地支))
                return _60甲子字典[天干地支];

            throw new ArgumentOutOfRangeException("天干定义超出范围");
        }

        #endregion 60甲子转换

        #region 天干地支

        /// <summary>
        /// _天干字典
        /// </summary>
        private static readonly IDictionary<string, TianGanType> _天干字典 = new Dictionary<string, TianGanType>()
        {
            {"甲",TianGanType.甲},
            {"乙",TianGanType.乙},
            {"丙",TianGanType.丙},
            {"丁",TianGanType.丁},
            {"戊",TianGanType.戊},
            {"己",TianGanType.己},
            {"庚",TianGanType.庚},
            {"辛",TianGanType.辛},
            {"壬",TianGanType.壬},
            {"癸",TianGanType.癸},
        };

        /// <summary>
        /// _地支字典
        /// </summary>
        private static readonly IDictionary<string, DiZhiEnum> _地支字典 = new Dictionary<string, DiZhiEnum>()
        {
            {"子",DiZhiEnum.子},
            {"丑",DiZhiEnum.丑},
            {"寅",DiZhiEnum.寅},
            {"卯",DiZhiEnum.卯},
            {"辰",DiZhiEnum.辰},
            {"巳",DiZhiEnum.巳},
            {"午",DiZhiEnum.午},
            {"未",DiZhiEnum.未},
            {"申",DiZhiEnum.申},
            {"酉",DiZhiEnum.酉},
            {"戌",DiZhiEnum.戌},
            {"亥",DiZhiEnum.亥},
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="天干"></param>
        /// <returns></returns>
        public static TianGanType To天干(this string 天干)
        {
            if (_天干字典.ContainsKey(天干))
                return _天干字典[天干];

            throw new ArgumentOutOfRangeException("天干定义超出范围");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="地支"></param>
        /// <returns></returns>
        public static DiZhiEnum To地支(this string 地支)
        {
            if (_地支字典.ContainsKey(地支))
                return _地支字典[地支];

            throw new ArgumentOutOfRangeException("地支定义超出范围");
        }

        #endregion 天干地支

        #region 28星宿中文描述

        /// <summary>
        ///
        /// </summary>
        private static readonly IDictionary<ErShiBaXingXiuType, string> 二十八星宿中文描述字典 = new Dictionary<ErShiBaXingXiuType, string>()
        {
            {ErShiBaXingXiuType.角木蛟,"诚恳合谐，福缘深厚，平易近人不拘小节，有丰富的人生经历，擅长决策，有条不紊。若是太过自我，容易自寻烦恼，而刚愎自用"},
            {ErShiBaXingXiuType.亢金龙,"精明决策，具有说服力，计划欠周详，容易意气用事，有斗志但运程有反复，脾气容易冲动。常因高傲和爱慕虚荣而出现损失"},
            {ErShiBaXingXiuType.氐土貉,"善解人意，易得别人帮助，善于谋略，八面玲珑具有野心，行动果决也不失斯文和气。常因不愿受束缚而显得游荡"},
            {ErShiBaXingXiuType.房日兔,"生来幸运，具有财气，需要脚踏实地，不然爬的高摔的疼，要保持低调，以免引起妒嫉。戒骄戒躁，这能够在事业上有所成就"},
            {ErShiBaXingXiuType.心月狐,"坚毅勤奋，忌恶扬善，不怕吃苦，积极具有正义感，属于能者多劳。不足的地方是疑心过重，有时会妨碍才能的发挥与事业的发展"},
            {ErShiBaXingXiuType.尾火虎,"个性严肃，能干谨慎，喜欢竞争，要注意修养和内涵，才能够成就财富。要注意的地方：外泠内热，容易弄巧成拙，带来相反的效果"},
            {ErShiBaXingXiuType.箕水豹,"具智慧和才干，不畏权威，无拘无束的享受主义者，遇到挫折也不会悲观。若是思想过于开放，在现今的社会，能够促使家庭观念淡薄，这是应该注意的地方"},
            {ErShiBaXingXiuType.斗木獬,"遇强则强，遇弱则弱，情绪变化较大，具有突破逆境的力量，富有创造力。个性深藏不露，容易招人误会，需要一颗安定的心"},
            {ErShiBaXingXiuType.女土蝠,"乐于助人，适合学习专门的技能，思考敏捷，为了自己的信念会努力奋斗。挫败的原因，常因个性刚强，不懂情调，或者是太坦白了"},
            {ErShiBaXingXiuType.虚日鼠,"富贵天生，人缘好，具有坚韧的耐力，对玄学有兴趣，好奇心重。但由于主观强爱争执，而令自己的精神受压抑"},
            {ErShiBaXingXiuType.危月燕,"脾气容易急躁，性情刚硬，本性善良而无心机，容易在人际关系上吃亏。好坏两个极端，比较明显，因为本身具有实力，要看把握的方向了"},
            {ErShiBaXingXiuType.室火猪,"威武刚烈，具有斗志和竞争心，积极乐观，欲望强烈。缺点是断臂独行，轻率急躁，需慎重思考，懂得温柔，过分的豪放会带来失败"},
            {ErShiBaXingXiuType.壁水貐,"内向冷静，心思甚密，处事周详，容意得到上司信任，但人缘运欠佳。固执和孤僻，会使原有的正义感，而不被别人理解，最好远离是非地，避免口舌"},
            {ErShiBaXingXiuType.奎木狼,"感情丰富，耿直而友善热情，追求真善美，人生必较幸运。欠缺胆识和耐力，只要放下固执，幸福就在身边了"},
            {ErShiBaXingXiuType.娄金狗,"思想敏捷，办事效率高，精力充沛，求知欲强，乐于助人。有任性和反叛的潜意识，若露出冷酷的一面，就增强了个人主意的色彩"},
            {ErShiBaXingXiuType.胃土雉,"刚强不屈，凭借不屈不挠的精神可以平步青云，但也因为冷酷无情，造成起落较大的人生。努力搞好人际关系，才更具有竞争力"},
            {ErShiBaXingXiuType.昴日鸡,"宽厚慈和，勤奋好学，能言善辩，刚柔并济。但是欠缺风趣，内在的个性强烈，能力越是出众，越应该谦虚待人"},
            {ErShiBaXingXiuType.毕月乌,"坚毅稳重，安详和谐，必较理想主义，有财气懂得计划。要注意提高自己的应变能力，作事有始有终，才不被别人认为眼高手底"},
            {ErShiBaXingXiuType.觜火猴,"言行谨慎，注重原则，善于表达，不喜欢暴力，心地善良充满爱心。缺陷在，过度自信容易树敌，因人缘受制而有失败"},
            {ErShiBaXingXiuType.参水猿,"有才干守信用，临危不乱，欠缺耐性，容易冲动，带有反叛或者心浮的意味。若是忍不住别人的批评和指责，就会造成孤立的局面"},
            {ErShiBaXingXiuType.井木犴,"重感情，风趣幽默，本性忠厚，属于性情中人，有双重的性格，开朗和沉稳。只是有时因为自尊心过强，不懂变通"},
            {ErShiBaXingXiuType.鬼金羊,"平易近人，正义凛然，容易受到欢迎，感情丰富，一生的变化大。很快地能将不愉快的事情忘记，讨厌受到束缚，耐力不足容易失去良机"},
            {ErShiBaXingXiuType.柳土獐,"善恶分明，个性强烈，一旦动怒不可收拾，要谨慎由于冲动而受骗。表面温柔随和，内在心高气傲，常以自我为中心的话，会造成离群而孤立"},
            {ErShiBaXingXiuType.星日马,"天资聪敏，刻苦耐劳，向往自由变化不定，属于大器晚成的类型。由于个性高，不爱巴结讨好奉承，容易造成人缘较差，影响才能的发挥"},
            {ErShiBaXingXiuType.张月鹿,"懂得讨人喜欢，有计谋，重视安逸的生活，喜欢研究学问。若是固持好胜，一生则波折不断，具有成功的条件，还要把握机会才行"},
            {ErShiBaXingXiuType.翼火蛇,"具有艺术气质，不喜欢竞争，陶醉在自我的世界，通常在外地发展有收获。由于主观强，言词尖锐，容易引起是非或者误会"},
            {ErShiBaXingXiuType.轸水蚓,"思想敏捷，适应能力强，稳重有内涵，喜欢深藏不露，可成大事。由于处世较低调，所以适合位居幕后，或者从事决策性的工作更为适宜"}
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="二十八星宿"></param>
        /// <returns></returns>
        public static string To28星宿中文描述(this ErShiBaXingXiuType 二十八星宿)
        {
            if (二十八星宿中文描述字典.ContainsKey(二十八星宿))
                return 二十八星宿中文描述字典[二十八星宿];

            return null;
        }

        #endregion 28星宿中文描述

        #region 建除12神

        /// <summary>
        ///
        /// </summary>
        private static readonly IDictionary<JianChuShiErShenType, string> 建除12神字典 = new Dictionary<JianChuShiErShenType, string>()
        {
            {JianChuShiErShenType.闭日,"坚固之意。最宜埋葬，代表能富贵大吉大利。逢闭日不宜看眼睛及求医 问学。外出经商，上任就职，逢闭日也不理想"},
            {JianChuShiErShenType.开日,"为开放、开心之意。除了埋葬主大凶外，凡事求财、求子、求缘、求职、求名都是好日子"},
            {JianChuShiErShenType.收日,"为收成之意。经商开市、外出求财，买屋签约、嫁娶订盟诸事吉利"},
            {JianChuShiErShenType.成日,"为成功、成就、结果之意。凡事皆有成，祈福、入学、开市、嫁娶、求医、远行、移徙、上任都是好日子。但不宜打官司，否则必定赢不了"},
            {JianChuShiErShenType.危日,"为危险之意。最忌登高、冒险，喜登山踏青的朋友，逢危日就应该特别小心"},
            {JianChuShiErShenType.破日,"为刚旺破败之日，万事皆忘，婚姻不谐。惟宜求医疗病及赴考求名。逢破日，不宜多管闲事"},
            {JianChuShiErShenType.执日,"为固执之意，执持操守也。司法警察人员，选择执日抓人最好不过了，十拿九稳。一般执日宜祈福、祭祀、求子、结婚、立约。忌搬家、远行"},
            {JianChuShiErShenType.定日,"按定为不动，不动则为死气。因此定日诸事不宜，只可做计划性的工作，尤其打官司如逢定日必不妙"},
            {JianChuShiErShenType.平日,"平者平常也，无凶无吉之日。一般修屋、求福、外出、求财、嫁娶都可以用平日"},
            {JianChuShiErShenType.满日,"为丰收圆满之意。祈福、结亲、开市都是好日子，如好友想结拜成兄弟，或准备替小孩认干爹，选择满日最好。不可服药、栽种、下葬、求医疗病、上官赴任"},
            {JianChuShiErShenType.除日,"为除旧布新之象。除服、疗病、避邪、出行、嫁娶都是好日子，如有久病想找个日子换医生试试不妨选择除日，效果甚佳。不可求官、上任、开张、搬家。逢除日不到上司家，以免吃力不讨好，新官上任更不可选在除日，以免官运受阻，断送前程"},
            {JianChuShiErShenType.建日,"为一岁之君之义。众神之统帅，健旺之气，吉利。宜修造、嫁娶、出行、祈福、谒贵、上书。忌动土、开仓"},
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="建除12神"></param>
        /// <returns></returns>
        public static string To建除12神描述(this JianChuShiErShenType 建除12神)
        {
            if (建除12神字典.ContainsKey(建除12神))
                return 建除12神字典[建除12神];

            return null;
        }

        #endregion 建除12神

        #region 六任时To祸福

        /// <summary>
        ///
        /// </summary>
        /// <param name="六任时"></param>
        /// <returns></returns>
        public static HuoFuType To祸福(this LiuRenShiType 六任时)
        {
            switch (六任时)
            {
                case LiuRenShiType.大安:
                case LiuRenShiType.速喜:
                case LiuRenShiType.小吉:
                    {
                        return HuoFuType.吉;
                    };
                case LiuRenShiType.赤口:
                case LiuRenShiType.空亡:
                case LiuRenShiType.留连:
                    {
                        return HuoFuType.凶;
                    };
                default:
                    {
                        //不知道返回些什么
                        return HuoFuType.凶;
                    };
            }
        }

        #endregion 六任时To祸福
    }
}