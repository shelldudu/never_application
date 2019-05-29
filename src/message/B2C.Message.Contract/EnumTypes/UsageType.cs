using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Message.Contract.EnumTypes
{
    /// <summary>
    /// 使用类型
    /// </summary>
    public enum UsageType : byte
    {
        /// <summary>
        /// 注册
        /// </summary>
        注册 = 1,

        /// <summary>
        /// 登录
        /// </summary>
        登录 = 2,

        /// <summary>
        /// 找回登录密码
        /// </summary>
        找回登录密码 = 3,

        /// <summary>
        /// 找回交易密码
        /// </summary>
        找回交易密码 = 4,

        /// <summary>
        /// 修改手机号码
        /// </summary>
        修改手机号码 = 5,

        /// <summary>
        /// 余额提现
        /// </summary>
        余额提现 = 6
    }
}