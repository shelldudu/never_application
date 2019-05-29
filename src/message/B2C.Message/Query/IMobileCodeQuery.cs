using B2C.Message.Models;
using B2C.Message.Contract.EnumTypes;
using B2C.Message.Contract.Models;
using B2C.Infrastructure;
using System;
using System.Collections.Generic;

namespace B2C.Message.Query
{
    /// <summary>
    /// 手机验证码查询接口
    /// </summary>
    public interface IMobileCodeQuery
    {
        /// <summary>
        /// 查询手机验证码列表
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        IEnumerable<MobileCodeInfo> GetList(long mobile, int topCount = 10);

        /// <summary>
        /// 以时间去查询某一个IP的条数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        int Count(string ip, DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 找出最后一个验证码
        /// </summary>
        /// <param name="mobile">The mobile.</param>
        /// <returns></returns>
        MobileCodeInfo Max(long mobile);
    }
}