using B2C.Message.Models;
using B2C.Message.Contract.EnumTypes;
using B2C.Message.Contract.Models;
using B2C.Infrastructure;
using System;
using System.Collections.Generic;

namespace B2C.Message.Query
{
    /// <summary>
    /// 邮箱验证码查询接口
    /// </summary>
    public interface IEmailCodeQuery
    {
        /// <summary>
        /// 查询邮箱验证码列表
        /// </summary>
        /// <param name="email"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        IEnumerable<EmailCodeInfo> GetList(string email, int topCount = 10);


        /// <summary>
        /// 找出最后一个验证码
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        EmailCodeInfo Max(string email);
    }
}