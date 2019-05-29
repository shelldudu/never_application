using Microsoft.AspNetCore.Mvc;
using Never;
using Never.Security;
using Never.Web.WebApi;
using Never.Web.WebApi.Results;
using B2C.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.App.Api
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class AppController : Never.Web.WebApi.Controllers.BasicController
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <returns></returns>
        public AppUser GetAppUser()
        {
            if (this.HttpContext.User is UserPrincipal)
            {
                return ((UserPrincipal)this.HttpContext.User).CurrentUser as AppUser;
            }

            if (this.Request.Headers.ContainsKey("userid"))
            {
                return new AppUser() { UserId = this.Request.Headers["userid"].FirstOrDefault().AsLong() };
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime GetAppTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        public string GetAppIP()
        {
            if (this.Request.Headers.ContainsKey("ip"))
            {
                return this.Request.Headers["ip"].FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// 获取操作平台
        /// </summary>
        public OperatePlatform GetAppPlatform()
        {
            if (this.Request.Headers.ContainsKey("platform"))
            {
                return this.Request.Headers["platform"].FirstOrDefault().AsEnum(OperatePlatform.Android);
            }

            return OperatePlatform.Android;
        }
    }
}