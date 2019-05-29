using B2C.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.App.Api
{
    /// <summary>
    /// 用户模型
    /// </summary>
    /// <seealso cref="B2C.Infrastructure._AggregateModel{System.Guid}" />
    public class UserModel : _AggregateModel<Guid>
    {
        #region prop

        /// <summary>
        /// 用户注册Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 如果用户身份状态是在锁定
        /// </summary>
        public DateTime? LockTime { get;  set; }

        /// <summary>
        /// 客户端平台
        /// </summary>
        public OperatePlatform Registeform { get; set; }

        #endregion prop
    }
}
