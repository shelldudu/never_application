using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Never;
using Never.Aop;
using Never.Caching;
using Never.Events;
using Never.Exceptions;
using Never.IoC;
using Never.Logging;
using Never.Security;
using Never.Web.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace B2C.App.Api
{
    /// <summary>
    /// 用户验证
    /// </summary>
    [Logger]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class UserAuthorizeAttribute : UserPrincipalAttribute
    {
        #region onauth

        protected override void OnAuthorizeCompleted(AuthorizationFilterContext context, IUser user)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            if (context.HttpContext.User is UserPrincipal)
            {
                return;
            }

            /*重新跳到登陆页面*/
            context.Result = new JsonResult(new Never.Web.WebApi.Controllers.BasicController.ResponseResult<string>("0001", string.Empty, "请先登录"));
        }

        #endregion onauth
    }
}