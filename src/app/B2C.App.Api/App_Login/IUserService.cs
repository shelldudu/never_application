using Never;
using Never.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.App.Api
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900adc600", "HttpGet")]
        ApiResult<UserModel> GetUser(long userId);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        [ApiActionRemark("aa3900fce2d4", "HttpGet")]
        ApiResult<int> GetCount(string mobile);

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900adca93", "HttpGet")]
        ApiResult<string> LockUser(long userId);

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900adce0a", "HttpGet")]
        ApiResult<string> UnlockUser(long userId);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900add212", "HttpPost")]
        ApiResult<UserModel> Login(LoginUserReqs reqs);

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="reqs">The reqs.</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900add64b", "HttpPost")]
        ApiResult<UserModel> Register(RegisterUserReqs reqs);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="reqs">The reqs.</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900adda72", "HttpPost")]
        ApiResult<string> ChangePassword(ChangePwdReqs reqs);
    }
}