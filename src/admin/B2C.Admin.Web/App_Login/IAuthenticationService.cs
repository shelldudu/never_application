using Microsoft.AspNetCore.Http;
using Never.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web
{
    /// <summary>
    /// 用户身份验证
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="isPersistent">票证将存储在持久性 Cookie 中（跨浏览器会话保存）</param>
        System.Threading.Tasks.Task SignIn(HttpContext context, IUser customer);

        /// <summary>
        /// 退出
        /// </summary>
        System.Threading.Tasks.Task SignOut(HttpContext context, IUser customer);

        /// <summary>
        /// 获取会员
        /// </summary>
        /// <returns></returns>
        UserPrincipal GetUserPrincipal(HttpContext context);
    }
}