using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace B2C.Calc.Chinese
{
    /// <summary>
    /// 中国传统日历计算
    /// </summary>
    public static partial class ChineseCalendar
    {
        #region 节气数据库

        //数组gLanarHoliDay存放每年的二十四节气对应的阳历日期
        //每年的二十四节气对应的阳历日期几乎固定，平均分布于十二个月中
        // 1月 2月 3月 4月 5月 6月
        //小寒 大寒 立春 雨水 惊蛰 春分 清明 谷雨 立夏 小满 芒种 夏至
        // 7月 8月 9月 10月 11月 12月
        //小暑 大暑 立秋 处暑 白露 秋分 寒露 霜降 立冬 小雪 大雪 冬至
        //*********************************************************************************
        // 节气无任何确定规律,所以只好存表,要节省空间,所以.
        //**********************************************************************************}
        //数据格式说明:
        //如1901年的节气为
        // 1月 2月 3月 4月 5月 6月 7月 8月 9月 10月 11月 12月
        // 6, 21, 4, 19, 6, 21, 5, 21, 6,22, 6,22, 8, 23, 8, 24, 8, 24, 8, 24, 8, 23, 8, 22
        // 9, 6, 11,4, 9, 6, 10,6, 9,7, 9,7, 7, 8, 7, 9, 7, 9, 7, 9, 7, 8, 7, 15
        //上面第一行数据为每月节气对应日期,15减去每月第一个节气,每月第二个节气减去15得第二行
        // 这样每月两个节气对应数据都小于16,每月用一个字节存放,高位存放第一个节气数据,低位存放
        //第二个节气的数据,可得下表
        private static byte[] 节气数据 =
        {
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1901
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1902
            0x96, 0xA5, 0x87, 0x96, 0x87, 0x87, 0x79, 0x69, 0x69, 0x69, 0x78, 0x78, //1903
            0x86, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x78, 0x87, //1904
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1905
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1906
            0x96, 0xA5, 0x87, 0x96, 0x87, 0x87, 0x79, 0x69, 0x69, 0x69, 0x78, 0x78, //1907
            0x86, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1908
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1909
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1910
            0x96, 0xA5, 0x87, 0x96, 0x87, 0x87, 0x79, 0x69, 0x69, 0x69, 0x78, 0x78, //1911
            0x86, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1912
            0x95, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1913
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1914
            0x96, 0xA5, 0x97, 0x96, 0x97, 0x87, 0x79, 0x79, 0x69, 0x69, 0x78, 0x78, //1915
            0x96, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1916
            0x95, 0xB4, 0x96, 0xA6, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x87, //1917
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x77, //1918
            0x96, 0xA5, 0x97, 0x96, 0x97, 0x87, 0x79, 0x79, 0x69, 0x69, 0x78, 0x78, //1919
            0x96, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1920
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x87, //1921
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x77, //1922
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x69, 0x69, 0x78, 0x78, //1923
            0x96, 0xA5, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1924
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x87, //1925
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1926
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1927
            0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1928
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1929
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1930
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x87, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1931
            0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1932
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1933
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1934
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1935
            0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1936
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1937
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1938
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1939
            0x96, 0xA5, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1940
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1941
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1942
            0x96, 0xA4, 0x96, 0x96, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1943
            0x96, 0xA5, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1944
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1945
            0x95, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1946
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1947
            0x96, 0xA5, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1948
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x79, 0x78, 0x79, 0x77, 0x87, //1949
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1950
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x79, 0x79, 0x79, 0x69, 0x78, 0x78, //1951
            0x96, 0xA5, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1952
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1953
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x68, 0x78, 0x87, //1954
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1955
            0x96, 0xA5, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1956
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1957
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1958
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1959
            0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1960
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1961
            0x96, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1962
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1963
            0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1964
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1965
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1966
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1967
            0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1968
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1969
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1970
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x79, 0x69, 0x78, 0x77, //1971
            0x96, 0xA4, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1972
            0xA5, 0xB5, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1973
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1974
            0x96, 0xB4, 0x96, 0xA6, 0x97, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1975
            0x96, 0xA4, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x89, 0x88, 0x78, 0x87, 0x87, //1976
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1977
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x78, 0x87, //1978
            0x96, 0xB4, 0x96, 0xA6, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1979
            0x96, 0xA4, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1980
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x77, 0x87, //1981
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1982
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x78, 0x79, 0x78, 0x69, 0x78, 0x77, //1983
            0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //1984
            0xA5, 0xB4, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //1985
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1986
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x79, 0x78, 0x69, 0x78, 0x87, //1987
            0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //1988
            0xA5, 0xB4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1989
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //1990
            0x95, 0xB4, 0x96, 0xA5, 0x86, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1991
            0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //1992
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1993
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1994
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x76, 0x78, 0x69, 0x78, 0x87, //1995
            0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //1996
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //1997
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //1998
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //1999
            0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2000
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2001
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //2002
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //2003
            0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2004
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2005
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2006
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x69, 0x78, 0x87, //2007
            0x96, 0xB4, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2008
            0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2009
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2010
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x78, 0x87, //2011
            0x96, 0xB4, 0xA5, 0xB5, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2012
            0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //2013
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2014
            0x95, 0xB4, 0x96, 0xA5, 0x96, 0x97, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //2015
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2016
            0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //2017
            0xA5, 0xB4, 0xA6, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2018
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //2019
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x86, //2020
            0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2021
            0xA5, 0xB4, 0xA5, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2022
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x79, 0x77, 0x87, //2023
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2024
            0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2025
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2026
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //2027
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2028
            0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2029
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2030
            0xA5, 0xB4, 0x96, 0xA5, 0x96, 0x96, 0x88, 0x78, 0x78, 0x78, 0x87, 0x87, //2031
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2032
            0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x86, //2033
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x78, 0x88, 0x78, 0x87, 0x87, //2034
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2035
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2036
            0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x86, //2037
            0xA5, 0xB3, 0xA5, 0xA5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2038
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2039
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x96, //2040
            0xA5, 0xC3, 0xA5, 0xB5, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2041
            0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x88, 0x88, 0x88, 0x78, 0x87, 0x87, //2042
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2043
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x88, 0x87, 0x96, //2044
            0xA5, 0xC3, 0xA5, 0xB4, 0xA5, 0xA6, 0x87, 0x88, 0x87, 0x78, 0x87, 0x86, //2045
            0xA5, 0xB3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x88, 0x78, 0x87, 0x87, //2046
            0xA5, 0xB4, 0x96, 0xA5, 0xA6, 0x96, 0x88, 0x88, 0x78, 0x78, 0x87, 0x87, //2047
            0x95, 0xB4, 0xA5, 0xB4, 0xA5, 0xA5, 0x97, 0x87, 0x87, 0x88, 0x86, 0x96, //2048
            0xA4, 0xC3, 0xA5, 0xA5, 0xA5, 0xA6, 0x97, 0x87, 0x87, 0x78, 0x87, 0x86, //2049
            0xA5, 0xC3, 0xA5, 0xB5, 0xA6, 0xA6, 0x87, 0x88, 0x78, 0x78, 0x87, 0x87  //2050
        };

        #endregion 节气数据库

        #region 纪年,纪月,纪日

        /// <summary>
        /// 纪时纪月字典
        /// </summary>
        private static readonly IDictionary<Tuple<int, TianGanType>, LiuShiJiaZiType> 纪月字典 = new Dictionary<Tuple<int, TianGanType>, LiuShiJiaZiType>()
        {
            {new Tuple<int,TianGanType>(1,TianGanType.甲),LiuShiJiaZiType.丙寅},
            {new Tuple<int,TianGanType>(1,TianGanType.己),LiuShiJiaZiType.丙寅},
            {new Tuple<int,TianGanType>(1,TianGanType.乙),LiuShiJiaZiType.戊寅},
            {new Tuple<int,TianGanType>(1,TianGanType.庚),LiuShiJiaZiType.戊寅},
            {new Tuple<int,TianGanType>(1,TianGanType.丙),LiuShiJiaZiType.庚寅},
            {new Tuple<int,TianGanType>(1,TianGanType.辛),LiuShiJiaZiType.庚寅},
            {new Tuple<int,TianGanType>(1,TianGanType.丁),LiuShiJiaZiType.壬寅},
            {new Tuple<int,TianGanType>(1,TianGanType.壬),LiuShiJiaZiType.壬寅},
            {new Tuple<int,TianGanType>(1,TianGanType.戊),LiuShiJiaZiType.甲寅},
            {new Tuple<int,TianGanType>(1,TianGanType.癸),LiuShiJiaZiType.甲寅},
        };

        /// <summary>
        /// 纪年天干字典
        /// </summary>
        private static readonly IDictionary<int, TianGanType> 纪年天干字典 = new Dictionary<int, TianGanType>()
        {
            {4,TianGanType.甲},
            {5,TianGanType.乙},
            {6,TianGanType.丙},
            {7,TianGanType.丁},
            {8,TianGanType.戊},
            {9,TianGanType.己},
            {0,TianGanType.庚},
            {1,TianGanType.辛},
            {2,TianGanType.壬},
            {3,TianGanType.癸}
        };

        /// <summary>
        /// 纪年地支字典
        /// </summary>
        /// <remarks>
        /// Tuple &lt;int, int, bool &gt;
        /// 第一个Int表示末位数，第二个为余数，第三个是奇数组(false)还是偶数组(true)
        /// </remarks>
        private static readonly IDictionary<Tuple<int, int, bool>, DiZhiEnum> 纪年地支字典 = new Dictionary<Tuple<int, int, bool>, DiZhiEnum>()
        {
            //奇数组：            偶数组：
            //1  2  3  4  5  6    1 2  3  4  5  6
            //子 寅 辰 午 申 戌   丑 卯 巳 未 酉 亥
            //末位数字是0，1的，余0为第5个，余1为第4个，余2为第3个，余3为第2个，余4为第1个，余5为第6个
            //末位数字是2，3的，余0为第6个，余1为第5个，余2为第4个，余3为第3个，余4为第2个，余5为第1个。
            //末位数字是4，5的，余0为第1个，余1为第6个，余2为第5个，余3为第4个，余4为第3个，余5为第2个。
            //末位数字是6，7的，余0为第2个，余1为第1个，余2为第6个，余3为第5个，余4为第4个，余5为第3个。
            //末位数字是8，9的，余0为第3个，余1为第2个，余2为第1个，余3为第6个，余4为第5个，余5为第4个。

            //奇数组
            {new Tuple<int,int,bool>(0,0,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(0,1,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(0,2,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(0,3,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(0,4,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(0,5,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(1,0,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(1,1,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(1,2,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(1,3,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(1,4,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(1,5,false),DiZhiEnum.戌},

            {new Tuple<int,int,bool>(2,0,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(2,1,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(2,2,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(2,3,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(2,4,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(2,5,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(3,0,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(3,1,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(3,2,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(3,3,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(3,4,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(3,5,false),DiZhiEnum.子},

            {new Tuple<int,int,bool>(4,0,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(4,1,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(4,2,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(4,3,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(4,4,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(4,5,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(5,0,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(5,1,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(5,2,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(5,3,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(5,4,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(5,5,false),DiZhiEnum.寅},

            {new Tuple<int,int,bool>(6,0,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(6,1,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(6,2,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(6,3,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(6,4,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(6,5,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(7,0,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(7,1,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(7,2,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(7,3,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(7,4,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(7,5,false),DiZhiEnum.辰},

            {new Tuple<int,int,bool>(8,0,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(8,1,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(8,2,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(8,3,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(8,4,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(8,5,false),DiZhiEnum.午},
            {new Tuple<int,int,bool>(9,0,false),DiZhiEnum.辰},
            {new Tuple<int,int,bool>(9,1,false),DiZhiEnum.寅},
            {new Tuple<int,int,bool>(9,2,false),DiZhiEnum.子},
            {new Tuple<int,int,bool>(9,3,false),DiZhiEnum.戌},
            {new Tuple<int,int,bool>(9,4,false),DiZhiEnum.申},
            {new Tuple<int,int,bool>(9,5,false),DiZhiEnum.午},

            //偶数组
            {new Tuple<int,int,bool>(0,0,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(0,1,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(0,2,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(0,3,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(0,4,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(0,5,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(1,0,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(1,1,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(1,2,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(1,3,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(1,4,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(1,5,true),DiZhiEnum.亥},

            {new Tuple<int,int,bool>(2,0,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(2,1,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(2,2,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(2,3,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(2,4,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(2,5,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(3,0,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(3,1,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(3,2,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(3,3,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(3,4,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(3,5,true),DiZhiEnum.丑},

            {new Tuple<int,int,bool>(4,0,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(4,1,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(4,2,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(4,3,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(4,4,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(4,5,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(5,0,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(5,1,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(5,2,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(5,3,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(5,4,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(5,5,true),DiZhiEnum.卯},

            {new Tuple<int,int,bool>(6,0,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(6,1,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(6,2,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(6,3,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(6,4,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(6,5,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(7,0,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(7,1,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(7,2,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(7,3,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(7,4,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(7,5,true),DiZhiEnum.巳},

            {new Tuple<int,int,bool>(8,0,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(8,1,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(8,2,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(8,3,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(8,4,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(8,5,true),DiZhiEnum.未},
            {new Tuple<int,int,bool>(9,0,true),DiZhiEnum.巳},
            {new Tuple<int,int,bool>(9,1,true),DiZhiEnum.卯},
            {new Tuple<int,int,bool>(9,2,true),DiZhiEnum.丑},
            {new Tuple<int,int,bool>(9,3,true),DiZhiEnum.亥},
            {new Tuple<int,int,bool>(9,4,true),DiZhiEnum.酉},
            {new Tuple<int,int,bool>(9,5,true),DiZhiEnum.未},
        };

        /// <summary>
        /// 纪年地支字典
        /// </summary>
        private static readonly IDictionary<int, ShiErShengXiaoType> 生肖字典 = new Dictionary<int, ShiErShengXiaoType>()
        {
            {0,ShiErShengXiaoType.猴},
            {1,ShiErShengXiaoType.鸡},
            {2,ShiErShengXiaoType.狗},
            {3,ShiErShengXiaoType.猪},
            {4,ShiErShengXiaoType.鼠},
            {5,ShiErShengXiaoType.牛},
            {6,ShiErShengXiaoType.兔},
            {7,ShiErShengXiaoType.虎},
            {8,ShiErShengXiaoType.龙},
            {9,ShiErShengXiaoType.蛇},
            {10,ShiErShengXiaoType.马},
            {11,ShiErShengXiaoType.羊}
        };

        /// <summary>
        /// 即若该日是甲或己的，在子时上配上甲为甲子；
        /// 日是乙或庚的，在子时上配上丙为丙子；
        /// 丙辛日子时配上戊为戊子；
        /// 丁壬日为庚子；
        /// 戊癸日为壬子
        /// </summary>
        private static readonly IDictionary<Tuple<int, TianGanType>, LiuShiJiaZiType> 纪时时辰甲子字典 = new Dictionary<Tuple<int, TianGanType>, LiuShiJiaZiType>()
        {
            //0点
            { new Tuple<int,TianGanType>(0,TianGanType.甲),LiuShiJiaZiType.甲子},
            { new Tuple<int,TianGanType>(0,TianGanType.己),LiuShiJiaZiType.甲子},
            { new Tuple<int,TianGanType>(0,TianGanType.乙),LiuShiJiaZiType.丙子},
            { new Tuple<int,TianGanType>(0,TianGanType.庚),LiuShiJiaZiType.丙子},
            { new Tuple<int,TianGanType>(0,TianGanType.丙),LiuShiJiaZiType.戊子},
            { new Tuple<int,TianGanType>(0,TianGanType.辛),LiuShiJiaZiType.戊子},
            { new Tuple<int,TianGanType>(0,TianGanType.丁),LiuShiJiaZiType.庚子},
            { new Tuple<int,TianGanType>(0,TianGanType.壬),LiuShiJiaZiType.庚子},
            { new Tuple<int,TianGanType>(0,TianGanType.戊),LiuShiJiaZiType.壬子},
            { new Tuple<int,TianGanType>(0,TianGanType.癸),LiuShiJiaZiType.壬子}
        };

        #endregion 纪年,纪月,纪日

        #region 计算农历

        /// <summary>
        /// 中国农历计算对象
        /// </summary>
        internal static readonly ChineseLunisolarCalendar 中国农历计算对象 = new ChineseLunisolarCalendar();

        #endregion 计算农历

        #region 计算时间限制

        /// <summary>
        /// 最小计算时间
        /// </summary>
        private static readonly DateTime MinDateTime = new DateTime(1901, 1, 1);

        /// <summary>
        /// 最大计算时间
        /// </summary>
        private static readonly DateTime MaxDateTime = new DateTime(2050, 12, 31);

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">公历时间不在定义范围(1901~2050)</exception>
        public static void 时间合格性检查(DateTime 公历时间)
        {
            var result = MinDateTime <= 公历时间 && 公历时间 <= MaxDateTime;
            if (!result)
                throw new ArgumentOutOfRangeException("公历时间应在1901~2050之间");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历年"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">公历时间不在定义范围(1901~2050)</exception>
        public static void 时间合格性检查(int 公历年)
        {
            var result = MinDateTime.Year <= 公历年 && 公历年 <= MaxDateTime.Year;
            if (!result)
                throw new ArgumentOutOfRangeException("公历时间应在1901~2050之间");
        }

        #endregion 计算时间限制

        #region 获取某年节气对应的一下个甲子时间

        /// <summary>
        ///
        /// </summary>
        private static DateTime _1930年甲子时间 = new DateTime(1930, 09, 11);

        /// <summary>
        /// 获取下一个甲子时间
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static DateTime 获取下一个甲子时间(DateTime 公历时间)
        {
            var days = (公历时间 - _1930年甲子时间).Days;
            var ceil = Math.Ceiling(days * 1.0 % 60);

            return _1930年甲子时间.AddDays(60 * Math.Ceiling(days * 1.0 / 60));
        }

        /// <summary>
        /// 获取上一个甲子时间
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static DateTime 获取上一个甲子时间(DateTime 公历时间)
        {
            var days = (公历时间 - _1930年甲子时间).Days;
            var ceil = Math.Ceiling(days * 1.0 % 60);

            return _1930年甲子时间.AddDays(60 * Math.Floor(days * 1.0 / 60));
        }

        #endregion 获取某年节气对应的一下个甲子时间

        #region 建除12神字典

        /// <summary>
        /// 根据算法得出的结果
        /// </summary>
        private static readonly IDictionary<int, Tuple<LiuShiJiaZiType, DateTime>> 每年正月第一个寅字列表字典 = new Dictionary<int, Tuple<LiuShiJiaZiType, DateTime>>()
        {
            {1901,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1901,2,5))},
            {1902,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1902,2,12))},
            {1903,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1903,2,7))},
            {1904,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1904,2,14))},
            {1905,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1905,2,8))},
            {1906,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1906,2,15))},
            {1907,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1907,2,10))},
            {1908,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1908,2,17))},
            {1909,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1909,2,11))},
            {1910,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1910,2,6))},
            {1911,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1911,2,13))},
            {1912,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1912,2,8))},
            {1913,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1913,2,14))},
            {1914,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1914,2,9))},
            {1915,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1915,2,16))},
            {1916,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1916,2,11))},
            {1917,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1917,2,5))},
            {1918,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1918,2,12))},
            {1919,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1919,2,7))},
            {1920,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1920,2,14))},
            {1921,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1921,2,8))},
            {1922,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1922,2,15))},
            {1923,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1923,2,10))},
            {1924,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1924,2,17))},
            {1925,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1925,2,11))},
            {1926,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1926,2,6))},
            {1927,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1927,2,13))},
            {1928,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1928,2,8))},
            {1929,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1929,2,14))},
            {1930,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1930,2,9))},
            {1931,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1931,2,16))},
            {1932,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1932,2,11))},
            {1933,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1933,2,5))},
            {1934,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1934,2,12))},
            {1935,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1935,2,7))},
            {1936,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1936,2,14))},
            {1937,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1937,2,8))},
            {1938,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1938,2,15))},
            {1939,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1939,2,10))},
            {1940,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1940,2,17))},
            {1941,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1941,2,11))},
            {1942,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1942,2,6))},
            {1943,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1943,2,13))},
            {1944,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1944,2,8))},
            {1945,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1945,2,14))},
            {1946,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1946,2,9))},
            {1947,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1947,2,16))},
            {1948,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1948,2,11))},
            {1949,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1949,2,5))},
            {1950,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1950,2,12))},
            {1951,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1951,2,7))},
            {1952,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1952,2,14))},
            {1953,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1953,2,8))},
            {1954,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1954,2,15))},
            {1955,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1955,2,10))},
            {1956,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1956,2,17))},
            {1957,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1957,2,11))},
            {1958,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1958,2,6))},
            {1959,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1959,2,13))},
            {1960,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1960,2,8))},
            {1961,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1961,2,14))},
            {1962,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1962,2,9))},
            {1963,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1963,2,16))},
            {1964,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1964,2,11))},
            {1965,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1965,2,5))},
            {1966,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1966,2,12))},
            {1967,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1967,2,7))},
            {1968,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1968,2,14))},
            {1969,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1969,2,8))},
            {1970,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1970,2,15))},
            {1971,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1971,2,10))},
            {1972,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1972,2,17))},
            {1973,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1973,2,11))},
            {1974,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1974,2,6))},
            {1975,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1975,2,13))},
            {1976,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1976,2,8))},
            {1977,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1977,2,14))},
            {1978,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1978,2,9))},
            {1979,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1979,2,16))},
            {1980,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1980,2,11))},
            {1981,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1981,2,5))},
            {1982,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1982,2,12))},
            {1983,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1983,2,7))},
            {1984,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1984,2,14))},
            {1985,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1985,2,8))},
            {1986,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1986,2,15))},
            {1987,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1987,2,10))},
            {1988,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1988,2,5))},
            {1989,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1989,2,11))},
            {1990,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(1990,2,6))},
            {1991,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1991,2,13))},
            {1992,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(1992,2,8))},
            {1993,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1993,2,14))},
            {1994,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(1994,2,9))},
            {1995,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1995,2,16))},
            {1996,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1996,2,11))},
            {1997,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(1997,2,5))},
            {1998,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1998,2,12))},
            {1999,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(1999,2,7))},
            {2000,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2000,2,14))},
            {2001,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2001,2,8))},
            {2002,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2002,2,15))},
            {2003,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2003,2,10))},
            {2004,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2004,2,5))},
            {2005,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2005,2,11))},
            {2006,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2006,2,6))},
            {2007,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2007,2,13))},
            {2008,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2008,2,8))},
            {2009,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2009,2,14))},
            {2010,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2010,2,9))},
            {2011,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2011,2,16))},
            {2012,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2012,2,11))},
            {2013,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2013,2,5))},
            {2014,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2014,2,12))},
            {2015,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2015,2,7))},
            {2016,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2016,2,14))},
            {2017,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2017,2,8))},
            {2018,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2018,2,15))},
            {2019,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2019,2,10))},
            {2020,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2020,2,5))},
            {2021,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2021,2,11))},
            {2022,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2022,2,6))},
            {2023,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2023,2,13))},
            {2024,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2024,2,8))},
            {2025,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2025,2,14))},
            {2026,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2026,2,9))},
            {2027,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2027,2,16))},
            {2028,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2028,2,11))},
            {2029,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2029,2,5))},
            {2030,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2030,2,12))},
            {2031,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2031,2,7))},
            {2032,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2032,2,14))},
            {2033,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2033,2,8))},
            {2034,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2034,2,15))},
            {2035,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2035,2,10))},
            {2036,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2036,2,5))},
            {2037,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2037,2,11))},
            {2038,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2038,2,6))},
            {2039,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2039,2,13))},
            {2040,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2040,2,8))},
            {2041,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2041,2,14))},
            {2042,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.戊寅,new DateTime(2042,2,9))},
            {2043,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2043,2,16))},
            {2044,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2044,2,11))},
            {2045,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.庚寅,new DateTime(2045,2,5))},
            {2046,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2046,2,12))},
            {2047,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.壬寅,new DateTime(2047,2,7))},
            {2048,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2048,2,14))},
            {2049,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.甲寅,new DateTime(2049,2,8))},
            {2050,new Tuple<LiuShiJiaZiType,DateTime>(LiuShiJiaZiType.丙寅,new DateTime(2050,2,15))}
        };

        /// <summary>
        /// 建除12星神重复坐上一建神节气
        /// </summary>
        private static readonly IList<JieQiEnum> 建除12神重复坐上一建神节气 = new List<JieQiEnum>()
        {
            JieQiEnum.清明,
            JieQiEnum.立夏,
            JieQiEnum.芒种,
            JieQiEnum.小暑,
            JieQiEnum.立秋,
            JieQiEnum.白露,
            JieQiEnum.寒露,
            JieQiEnum.立冬,
            JieQiEnum.大雪,
            JieQiEnum.小寒,
            JieQiEnum.立春,
            JieQiEnum.惊蛰,
        };

        #endregion 建除12神字典

        #region 月节气字典

        /// <summary>
        ///
        /// </summary>
        private static readonly Dictionary<YueJieQiType, Tuple<JieQiEnum, JieQiEnum>> 月节气字典 = new Dictionary<YueJieQiType, Tuple<JieQiEnum, JieQiEnum>>()
        {
            { YueJieQiType.孟春,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.立春,JieQiEnum.惊蛰) },
            { YueJieQiType.仲春,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.惊蛰,JieQiEnum.清明) },
            { YueJieQiType.季春,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.清明,JieQiEnum.立夏) },
            { YueJieQiType.孟夏,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.立夏,JieQiEnum.芒种) },
            { YueJieQiType.仲夏,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.芒种,JieQiEnum.小暑) },
            { YueJieQiType.季夏,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.小暑,JieQiEnum.立秋) },
            { YueJieQiType.孟秋,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.立秋,JieQiEnum.白露) },
            { YueJieQiType.仲秋,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.白露,JieQiEnum.寒露) },
            { YueJieQiType.季秋,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.寒露,JieQiEnum.立冬) },
            { YueJieQiType.孟冬,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.立冬,JieQiEnum.大雪) },
            { YueJieQiType.仲冬,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.大雪,JieQiEnum.小寒) },
            { YueJieQiType.季冬,new Tuple<JieQiEnum, JieQiEnum>(JieQiEnum.小寒,JieQiEnum.立春) },
        };

        #endregion 月节气字典

        #region 时辰凶喜字典

        /// <summary>
        ///
        /// </summary>
        private static readonly IDictionary<Tuple<LiuShiJiaZiType, LiuShiJiaZiType>, Tuple<HuoFuType, string>> 时辰凶喜字典 = new Dictionary<Tuple<LiuShiJiaZiType, LiuShiJiaZiType>, Tuple<HuoFuType, string>>()
        {
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂天开吉神") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武黑道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司令金星") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑孤辰") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲子,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀天讼") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.凶,"空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲寅,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"三合黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"黄道明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑五鬼") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲辰,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂天开吉神") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲午,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲申,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"三合黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"黄道明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢五鬼") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.甲戌,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天开玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙丑,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙卯,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"寡宿五鬼") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙巳,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙未,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.吉,"天贵黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙酉,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"寡宿五鬼") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.乙亥,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙子,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命太阴") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙寅,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙辰,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙午,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙申,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丙戌,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁丑,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙天贵") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁卯,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁巳,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁未,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"天贵青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁酉,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.丁亥,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊子,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊寅,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德六合") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊辰,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊午,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天官") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊申,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.戊戌,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天寡孤辰") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮福神") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.凶,"空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己丑,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己卯,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己巳,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己未,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己酉,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.甲子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.乙丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.丙寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.丁卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.戊辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.己巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.庚午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.辛未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.壬申),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.癸酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.甲戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.己亥,LiuShiJiaZiType.乙亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚子,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.凶,"空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.吉,"福德天官") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚寅,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天寡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚辰,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天寡天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚午,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚申,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.丙子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.丁丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.戊寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.己卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.庚辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.辛巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.壬午),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.癸未),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.甲申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.乙酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.丙戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.庚戌,LiuShiJiaZiType.丁亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德三合") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天官福星") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.凶,"黑煞") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命喜神") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛丑,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂三合") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路空亡") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛卯,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人黄道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛巳,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德三合") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天官贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.凶,"黑煞") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命喜神") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛未,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂三合") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛酉,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.戊子),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.己丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.庚寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.辛卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.壬辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.癸巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.甲午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.乙未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.丙申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.丁酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.戊戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.辛亥,LiuShiJiaZiType.己亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬子,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬寅,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬辰,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬午,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬申,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.庚子),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.辛丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.壬寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.癸卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.甲辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.乙巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.丙午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.丁未),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.戊申),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.己酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.庚戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.壬戌,LiuShiJiaZiType.辛亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸丑,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"天寡孤辰") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀黑道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮福德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸卯,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"黑道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"天乙玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸巳,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸未,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.凶,"天寡孤辰") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.吉,"贵人") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀黑道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮福德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.凶,"白虎") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸酉,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.凶,"黑道") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.壬子),new Tuple<HuoFuType,string>(HuoFuType.凶,"截路") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.癸丑),new Tuple<HuoFuType,string>(HuoFuType.吉,"玉堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.甲寅),new Tuple<HuoFuType,string>(HuoFuType.凶,"天牢") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.乙卯),new Tuple<HuoFuType,string>(HuoFuType.凶,"元武") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.丙辰),new Tuple<HuoFuType,string>(HuoFuType.吉,"司命") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.丁巳),new Tuple<HuoFuType,string>(HuoFuType.凶,"勾陈") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.戊午),new Tuple<HuoFuType,string>(HuoFuType.吉,"青龙") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.己未),new Tuple<HuoFuType,string>(HuoFuType.吉,"明堂") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.庚申),new Tuple<HuoFuType,string>(HuoFuType.凶,"天刑") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.辛酉),new Tuple<HuoFuType,string>(HuoFuType.凶,"朱雀") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.壬戌),new Tuple<HuoFuType,string>(HuoFuType.吉,"金匮") },
            { new Tuple<LiuShiJiaZiType,LiuShiJiaZiType>(LiuShiJiaZiType.癸亥,LiuShiJiaZiType.癸亥),new Tuple<HuoFuType,string>(HuoFuType.吉,"天德") }
        };

        #endregion 时辰凶喜字典

        #region 节气方法

        /// <summary>
        ///
        /// </summary>
        /// <param name="jieqi"></param>
        /// <returns></returns>
        private static int 修正节气索引(JieQiEnum jieqi)
        {
            if (jieqi < JieQiEnum.小寒)
                return (int)jieqi + 2;

            return ((int)jieqi + 2) % 24;
        }

        /// <summary>
        /// 某年第N个节气的交气日期 (数据索引是从1起小寒)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="节气"></param>
        /// <remarks>
        /// 数据索引是从1起小寒
        /// </remarks>
        /// <returns></returns>
        public static int 计算节气交气日(int year, JieQiEnum 节气)
        {
            时间合格性检查(year);

            int startYear = MinDateTime.Year;
            byte flag = 0x00;

            //24节气对应的值修改与索引相差2个值
            int n = 修正节气索引(节气);
            if (n % 2 == 0)//双数
            {
                flag = 节气数据[(year - startYear) * 12 + n / 2 - 1];
                return 15 + (Convert.ToInt32(flag) % 16);
            }
            //单数
            int weizhi = (year - startYear) * 12 + (n + 1) / 2 - 1;
            flag = 节气数据[weizhi];
            return 15 - Convert.ToInt32(decimal.Round(flag / 16, 0));
        }

        /// <summary>
        /// 某年第N个节气的交气日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="节气"></param>
        /// <returns></returns>
        public static DateTime 计算节气日期(int year, JieQiEnum 节气)
        {
            时间合格性检查(year);

            int month = 1;
            //24节气对应的值修改与索引相差2个值
            int n = 修正节气索引(节气);
            month = Convert.ToInt32((n + 1) / 2);
            return Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-" + 计算节气交气日(year, 节气).ToString());
        }

        /// <summary>
        /// 获取当前节气下一个节气
        /// </summary>
        /// <param name="当前节气"></param>
        /// <returns></returns>
        public static JieQiEnum 计算当前节气下一个节气(JieQiEnum 当前节气)
        {
            if (当前节气 == JieQiEnum.大寒)
                return JieQiEnum.立春;

            return (JieQiEnum)((int)当前节气 + 1);
        }

        /// <summary>
        /// 获取当前节气上一个节气
        /// </summary>
        /// <param name="当前节气"></param>
        /// <returns></returns>
        public static JieQiEnum 计算当前节气上一个节气(JieQiEnum 当前节气)
        {
            if (当前节气 == JieQiEnum.立春)
                return JieQiEnum.大寒;

            return (JieQiEnum)((int)当前节气 - 1);
        }

        /// <summary>
        /// 确定公历时间的节气
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static Tuple<JieQiEnum, int> 计算当前节气(DateTime 公历时间)
        {
            时间合格性检查(公历时间);

            for (int i = 1; i <= 24; i++)
            {
                var 节气 = (JieQiEnum)i;
                //第一个大于0的，表明这个时间是当前节气与上一个节气之间
                if ((计算节气日期(公历时间.Year, 节气) - 公历时间).Days > 0)
                    return new Tuple<JieQiEnum, int>(计算当前节气上一个节气(节气), 公历时间.Year);
            }

            return null;
        }

        #endregion 节气方法

        #region 计算某一年节气列表

        /// <summary>
        /// 获取某一年所有节气列表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static IList<Tuple<DateTime, JieQiEnum>> 获取某年所有节气列表(int year)
        {
            var list = new List<Tuple<DateTime, JieQiEnum>>(24);
            for (var i = 1; i <= 22; i++)
            {
                var jieqi = (JieQiEnum)i;
                list.Add(new Tuple<DateTime, JieQiEnum>(计算节气日期(year, jieqi), jieqi));
            }

            list.Add(new Tuple<DateTime, JieQiEnum>(计算节气日期(year + 1, JieQiEnum.小寒), JieQiEnum.小寒));
            list.Add(new Tuple<DateTime, JieQiEnum>(计算节气日期(year + 1, JieQiEnum.大寒), JieQiEnum.大寒));
            return list;
        }

        /// <summary>
        /// 获取某一年所有节气列表
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static IList<Tuple<DateTime, JieQiEnum>> 获取某年所有节气列表(DateTime 公历时间)
        {
            return 获取某年所有节气列表(公历时间.Year);
        }

        #endregion 计算某一年节气列表

        #region 计算月节气

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static IDictionary<YueJieQiType, Tuple<DateTime, DateTime>> 获取某年月节气时间(DateTime 公历时间)
        {
            return 获取某年月节气时间(公历时间.Year);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static IDictionary<YueJieQiType, Tuple<DateTime, DateTime>> 获取某年月节气时间(int year)
        {
            var list = 获取某年所有节气列表(year);
            var result = new Dictionary<YueJieQiType, Tuple<DateTime, DateTime>>(12);

            foreach (var i in 月节气字典)
            {
                Nullable<DateTime> beginTime, endTime;
                if (i.Key <= YueJieQiType.孟冬)
                {
                    beginTime = 计算节气日期(year, i.Value.Item1);
                    endTime = 计算节气日期(year, i.Value.Item2).AddDays(-1);
                }
                else
                {
                    if (i.Key == YueJieQiType.仲冬)
                    {
                        beginTime = 计算节气日期(year, i.Value.Item1);
                        endTime = 计算节气日期(year + 1, i.Value.Item2).AddDays(-1);
                    }
                    else
                    {
                        beginTime = 计算节气日期(year + 1, i.Value.Item1);
                        endTime = 计算节气日期(year + 1, i.Value.Item2).AddDays(-1);
                    }
                }
                result.Add(i.Key, new Tuple<DateTime, DateTime>(beginTime.Value, endTime.Value));
            }
            return result;
        }

        #endregion 计算月节气

        #region 计算纪日天干

        /// <summary>
        /// 计算天干
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static TianGanType 计算纪日天干(DateTime 公历时间)
        {
            int 世纪数 = 0, 年份后两位 = 0, 月份 = 0, 日数 = 0;
            if (new[] { 1, 2 }.Contains(公历时间.Month))
            {
                世纪数 = int.Parse(公历时间.AddYears(-1).Year.ToString().Substring(0, 2));
                年份后两位 = 公历时间.Month == 1 ? 13 : 14;
            }
            else
            {
                世纪数 = int.Parse(公历时间.Year.ToString().Substring(0, 2));
                年份后两位 = int.Parse(公历时间.Year.ToString().Substring(2, 2));
            }
            月份 = 公历时间.Month;
            日数 = 公历时间.Day;

            //const string math = "4 * 世纪数 + Math.Floor(世纪数/4) + 5 * 年份后两位 + Math.Floor(年份后两位 / 4) + Math.Floor(3 * ( 月份 + 1 ) / 5) + 日数 - 3";
            //天干
            var tiangan = 4 * 世纪数 + Math.Floor(世纪数 * 1.0 / 4) + 5 * 年份后两位 + Math.Floor(年份后两位 * 1.0 / 4) + Math.Floor(3 * 1.0 * (月份 + 1) / 5) + 日数 - 3;
            var result = int.Parse(tiangan.ToString().Substring(tiangan.ToString().Length - 1)) % 10;
            //是0是从癸开始的
            if (result == 0)
                return TianGanType.癸;

            return (TianGanType)result;
        }

        #endregion 计算纪日天干

        #region 计算纪日地支

        /// <summary>
        /// 计算纪日地支
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static DiZhiEnum 计算纪日地支(DateTime 公历时间)
        {
            int 世纪数 = 0, 年份后两位 = 0, 月份 = 0, 日数 = 0;
            if (new[] { 1, 2 }.Contains(公历时间.Month))
            {
                世纪数 = int.Parse(公历时间.AddYears(-1).Year.ToString().Substring(0, 2));
                年份后两位 = 公历时间.Month == 1 ? 13 : 14;
            }
            else
            {
                世纪数 = int.Parse(公历时间.Year.ToString().Substring(0, 2));
                年份后两位 = int.Parse(公历时间.Year.ToString().Substring(2, 2));
            }
            月份 = 公历时间.Month;
            日数 = 公历时间.Day;

            //const string math = "8 * 世纪数 + Math.Floor(世纪数/4) + 5 * 年份后两位 + Math.Floor(年份后两位 / 4) + Math.Ceil(3 * ( 月份 + 1 ) / 5) + 日数 + 7 + i";
            //地支
            var dizhi = 8 * 世纪数 + Math.Floor(世纪数 * 1.0 / 4) + 5 * 年份后两位 + Math.Floor(年份后两位 * 1.0 / 4) + Math.Floor(3 * 1.0 * (月份 + 1) / 5) + 日数 + 7 + (月份 % 2 != 0 ? 0 : 6);
            var result = (int)dizhi % 12;

            //是0是从亥开始的
            if (result == 0)
                return DiZhiEnum.亥;

            return (DiZhiEnum)result;
        }

        #endregion 计算纪日地支

        #region 计算纪日

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static LiuShiJiaZiType 计算纪日(DateTime 公历时间)
        {
            int 世纪数 = 0, 年份后两位 = 0, 月份 = 0, 日数 = 0;
            if (new[] { 1, 2 }.Contains(公历时间.Month))
            {
                var 上一年 = 公历时间.AddYears(-1);
                世纪数 = int.Parse(上一年.Year.ToString().Substring(0, 2));
                年份后两位 = int.Parse(上一年.Year.ToString().Substring(2, 2));
                月份 = 公历时间.Month == 1 ? 13 : 14;
            }
            else
            {
                世纪数 = int.Parse(公历时间.Year.ToString().Substring(0, 2));
                年份后两位 = int.Parse(公历时间.Year.ToString().Substring(2, 2));
                月份 = 公历时间.Month;
            }
            日数 = 公历时间.Day;

            var tiangan = 4 * 世纪数 + Math.Floor(世纪数 * 1.0 / 4) + 5 * 年份后两位 + Math.Floor(年份后两位 * 1.0 / 4) + Math.Floor(3 * 1.0 * (月份 + 1) / 5) + 日数 - 3;
            var dizhi = 8 * 世纪数 + Math.Floor(世纪数 * 1.0 / 4) + 5 * 年份后两位 + Math.Floor(年份后两位 * 1.0 / 4) + Math.Floor(3 * 1.0 * (月份 + 1) / 5) + 日数 + 7 + (月份 % 2 != 0 ? 0 : 6);

            var tResult = int.Parse(tiangan.ToString().Substring(tiangan.ToString().Length - 1)) % 10;
            var dResult = (int)dizhi % 12;

            // 天干是0是从癸开始的
            // 地支是0是从亥开始的
            return string.Concat(tResult == 0 ? TianGanType.癸.ToString() : ((TianGanType)(tResult)).ToString(),
                                 dResult == 0 ? DiZhiEnum.亥.ToString() : ((DiZhiEnum)(dResult)).ToString()
                                ).To60甲子();
        }

        #endregion 计算纪日

        #region 计算纪月

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static LiuShiJiaZiType 计算纪月(DateTime 公历时间)
        {
            var 纪年 = 计算纪年天干(公历时间);
            var key = new Tuple<int, TianGanType>(1, 纪年);
            var value = 纪月字典[key];
            var 立春 = 计算节气日期(公历时间.Year, JieQiEnum.立春);

            var val = (int)value;
            //用的是上一年的时间为计算
            if (公历时间 < 立春)
            {
                val = val + ((公历时间 - 立春.AddMonths(-1)).Days > 0 ? 11 : 10);
                if (val > 60)
                    val = val - 60;
                return (LiuShiJiaZiType)(val);
            }
            for (int i = 1; i <= 12; i++)
            {
                if ((立春.AddMonths(i) - 公历时间).Days >= 0)
                {
                    val = val + i - 1;
                    if (val > 60)
                        val = val - 60;
                    return (LiuShiJiaZiType)(val);
                }
            }

            throw new InvalidOperationException("没有找到该纪月信息");
        }

        #endregion 计算纪月

        #region 计算纪年

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static TianGanType 计算纪年天干(DateTime 公历时间)
        {
            时间合格性检查(公历时间);

            //要排除立春之后的数据
            var 立春 = 计算节气日期(公历时间.Year, JieQiEnum.立春);
            //立春前与立春后
            var year = (公历时间 - 立春).Days >= 0 ? 立春.Year : (立春.Year - 1);

            return 纪年天干字典[int.Parse(year.ToString().Substring(3, 1))];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static DiZhiEnum 计算纪年地支(DateTime 公历时间)
        {
            时间合格性检查(公历时间);

            //要排除立春之后的数据
            var 立春 = 计算节气日期(公历时间.Year, JieQiEnum.立春);
            //立春后用公历时间的地支，立春前要用公历前一年的地支（即生肖）
            int year = (公历时间 - 立春).Days >= 0 ? 公历时间.Year : (公历时间.Year - 1);

            int 前三位 = int.Parse(year.ToString().Substring(0, 3));
            int 最后一位 = int.Parse(year.ToString().Substring(3, 1));
            //公元后奇数公元纪年配偶数组，偶数公元纪年配奇数组。公元前则相反
            var 奇数组 = 最后一位 % 2 != 0;

            var key = new Tuple<int, int, bool>(最后一位, 前三位 % 6, 奇数组);

            if (纪年地支字典.ContainsKey(key))
                return 纪年地支字典[key];

            throw new ArgumentOutOfRangeException("没有找到相应的纪年地支");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static LiuShiJiaZiType 计算纪年(DateTime 公历时间)
        {
            return string.Concat(计算纪年天干(公历时间).ToString(), 计算纪年地支(公历时间).ToString()).To60甲子();
        }

        #endregion 计算纪年

        #region 计算纪时

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static LiuShiJiaZiType 计算纪时(DateTime 公历时间)
        {
            var 天干 = 计算纪日天干(公历时间);
            var key = new Tuple<int, TianGanType>(0, 天干);
            var value = 纪时时辰甲子字典[key];

            if (new[] { 23, 0 }.Contains(公历时间.Hour))
                return value;

            var hour = (int)Math.Ceiling(公历时间.Hour * 1.0 / 2);

            return (LiuShiJiaZiType)((int)value + hour);
        }

        #endregion 计算纪时

        #region 计算生肖

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static ShiErShengXiaoType 计算生肖(DateTime 公历时间)
        {
            时间合格性检查(公历时间);

            //要排除立春之后的数据
            var 立春 = 计算节气日期(公历时间.Year, JieQiEnum.立春);
            //立春后用公历时间的地支，立春前要用公历前一年的地支（即生肖）

            return (公历时间 - 立春).Days >= 0 ? 生肖字典[公历时间.Year % 12] : 生肖字典[(公历时间.Year - 1) % 12];
        }

        #endregion 计算生肖

        #region 计算农历

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static NongLiModel 计算农历(DateTime 公历时间)
        {
            var cal = 中国农历计算对象;

            var result = new NongLiModel(公历时间)
            {
                Year = cal.GetYear(公历时间),
                Month = cal.GetMonth(公历时间),
                Day = cal.GetDayOfMonth(公历时间),
            };
            result.LeapMonth = cal.GetLeapMonth(result.Year);
            result.IsLeapMonth = result.LeapMonth > 0 && result.Month == result.LeapMonth;
            result.Month = (result.LeapMonth > 0 && result.Month >= result.LeapMonth) ? (result.Month - 1) : (result.Month);
            result.IsLeapYear = cal.IsLeapYear(result.Year);

            result.YearDescn = result.Year.To中文年份();

            result.MonthDescn = ((MonthType)result.Month).ToString();
            result.DayDescn = ((RiQiType)result.Day).ToString();
            return result;
        }

        #endregion 计算农历

        #region 计算28星宿

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static ErShiBaXingXiuType 计算28星宿(DateTime 公历时间)
        {
            //要包括当天
            var 基本天数 = (公历时间.Year - 1) * 365 + (公历时间 - new DateTime(公历时间.Year, 1, 1)).Days + 1;
            //var 农历 = 计算农历(公历时间);
            var 闰日天数 = (int)Math.Floor(((公历时间.Year - 1) * 1.0) / 4); //+ (中国农历计算对象.IsLeapYear(公历时间.Year) && 公历时间.Month >=3 ? 1:0);  //(农历.IsLeapYear && 农历.Month >= 3 ? 1 : 0);
            if (公历时间.Year >= 1901 && 公历时间.Year < 2100)
                闰日天数 = 闰日天数 - 13;

            int mod = (23 + 基本天数 + 闰日天数) % 28;
            if (mod == 0)
                mod = 28;

            return (ErShiBaXingXiuType)mod;
        }

        #endregion 计算28星宿

        #region 计算建除12星神

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static JianChuShiErShenType 计算建除12神(DateTime 公历时间)
        {
            时间合格性检查(公历时间);

            //每年正月第一个寅日就是建日。
            var 今年正月 = 计算节气日期(公历时间.Year, JieQiEnum.立春);
            Nullable<DateTime> 上年正月 = null;
            if (今年正月 > 公历时间)
                上年正月 = 计算节气日期(公历时间.Year - 1, JieQiEnum.立春);

            var key = 上年正月 == null ? 公历时间.Year : 上年正月.Value.Year;
            var value = 每年正月第一个寅字列表字典[key];
            var 第一个寅字时间 = value.Item2;

            var 公历时间与第一个寅字时间差 = (公历时间 - 第一个寅字时间).Days;
            //第一个寅字是坐建神
            if (公历时间与第一个寅字时间差 == 0)
                return JianChuShiErShenType.建日;

            //算法1，逢节前一日都是坐前一天的12神
            //由于目前无法确定，每年这些节气都是重复坐的，所以叫算法1
            //从2开始，是表示从立春后（即雨水），从上面"每年正月第一个寅字列表字典"字典中可以得知，
            //第一个寅字总比立春后（逻辑也如此），因此是不能包括立春这一天的，立春这一天应该用上一年开始
            for (var i = 2; i <= 24; i++)
            {
                var 节气 = (JieQiEnum)i;
                var 节气时间 = 计算节气日期(key, 节气);
                if (节气时间 > 公历时间)
                    break;
                if (建除12神重复坐上一建神节气.Contains(节气))
                    公历时间与第一个寅字时间差--;
            }

            var 飞行圈数 = (int)JianChuShiErShenType.建日 + 公历时间与第一个寅字时间差 % 12;
            if (飞行圈数 >= 13)
                飞行圈数 = 飞行圈数 - 12;

            return (JianChuShiErShenType)飞行圈数;
        }

        #endregion 计算建除12星神

        #region 计算6任时

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static IDictionary<DiZhiEnum, LiuRenShiType> 计算六任时(DateTime 公历时间)
        {
            时间合格性检查(公历时间);

            var result = new Dictionary<DiZhiEnum, LiuRenShiType>(12);
            var 农历 = 计算农历(公历时间);
            var begin = (农历.Month + 农历.Day);

            var turn = 0;
            for (var i = 1; i <= 12; i++)
            {
                turn = (begin + i - 1 - 3) % 6;
                if (turn == 0)
                    turn = 6;

                result.Add((DiZhiEnum)i, (LiuRenShiType)turn);
            }

            return result;
        }

        #endregion 计算6任时

        #region 计算时辰凶吉

        /// <summary>
        ///
        /// </summary>
        /// <param name="公历时间"></param>
        /// <returns></returns>
        public static IDictionary<DiZhiEnum, HuoFuType> 计算时辰凶吉(DateTime 公历时间)
        {
            时间合格性检查(公历时间);
            var result = new Dictionary<DiZhiEnum, HuoFuType>(12);
            var 纪日 = 计算纪日(公历时间);
            var keys = new Dictionary<LiuShiJiaZiType, LiuShiJiaZiType>(12);
            for (var i = 1; i < 24; i = i + 2)
            {
                var time = new DateTime(公历时间.Year, 公历时间.Month, 公历时间.Day, i, 10, 0);
                keys.Add(计算纪时(time), 纪日);
            }

            foreach (var i in keys)
            {
                var key = new Tuple<LiuShiJiaZiType, LiuShiJiaZiType>(i.Value, i.Key);
                result.Add(i.Key.ToString().Substring(1, 1).To地支(), 时辰凶喜字典[key].Item1);
            }

            return result;
        }

        #endregion 计算时辰凶吉
    }
}