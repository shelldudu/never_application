using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace B2C.App.Api
{
    /// <summary>
    /// 后台用户验证，主要是操作验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class UserPrincipalAttribute : Never.Web.WebApi.Security.UserPrincipalAttribute
    {
        #region onauth

        protected override void OnAuthorizeCompleted(AuthorizationFilterContext context, IUser user)
        {
            base.OnAuthorizeCompleted(context, user);
        }

        #endregion onauth

        #region user

        public override IUser GetUser(AuthorizationFilterContext context, HttpRequest request)
        {
            if (request.Headers != null && request.Headers.ContainsKey("userid"))
            {
                var userid = request.Headers["userid"].FirstOrDefault().AsLong();
                if (userid > 0)
                    return new AppUser() { UserId = userid };
            }

            if (request.Query != null && request.Query.ContainsKey("userid"))
            {
                var userid = request.Query["userid"].FirstOrDefault().AsLong();
                if (userid > 0)
                    return new AppUser() { UserId = userid };
            }

            //return new AppUser() { UserId = 25769803782 };
            return null;
        }

        #endregion user
    }
}