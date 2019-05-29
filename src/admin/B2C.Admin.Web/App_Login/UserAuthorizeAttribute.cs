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
using Never.Mappers;
using Never.Security;
using Never.Web.Mvc;
using Never.Web.Mvc.Security;
using B2C.Admin.Web.Permissions;
using B2C.Admin.Web.Permissions.EnumTypes;
using B2C.Admin.Web.Permissions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace B2C.Admin.Web
{
    /// <summary>
    /// 用户验证
    /// </summary>
    [Logger]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class UserAuthorizeAttribute : UserPrincipalAttribute
    {
        #region ctor

        /// <summary>
        /// 缓存字典
        /// </summary>
        private static readonly CounterDictCache<string, AppUser> counterDict = null;

        /// <summary>
        ///
        /// </summary>
        static UserAuthorizeAttribute()
        {
            counterDict = new CounterDictCache<string, AppUser>(200);
        }

        #endregion ctor

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

            if (this.IsAjaxRequest(context.HttpContext.Request))
            {
                context.HttpContext.Response.Clear();
                var api = new
                {
                    status = -2,
                    result = new { Action = "去登录", PageUrl = string.Concat(context.HttpContext.Request.Scheme, "://", context.HttpContext.Request.Host, "/Login/login") },
                    message = "请先登录"
                };

                context.Result = new JsonResult(api);
                return;
            }

            context.Result = new RedirectResult("~/Login/login");
        }

        public virtual bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                return false;
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }

            return false;
        }

        #endregion onauth

        #region user

        public override IUser GetUser(AuthorizationFilterContext context, HttpRequest request)
        {
            var query = context.HttpContext.RequestServices.GetService(typeof(IPermissionQuery)) as IPermissionQuery;
            if (query == null)
            {
                return null;
            }

            var task = context.HttpContext.AuthenticateAsync(FormsAuthenticationService.FormsScheme);
            if (task == null)
            {
                return null;
            }

            var userName = this.GetUserNameFromTicket(context, request, task);
            if (userName.IsNullOrEmpty())
            {
                return null;
            }

            var ticket = this.GetUserTicketFromTicket(context, request, task);
            if (!FormsAuthenticationService.MatchFormTicket(userName, ticket))
            {
                return null;
            }

            if (counterDict.ContainsKey(userName))
            {
                return counterDict[userName] as AppUser;
            }

            var u = this.GetUser(userName, query);
            if (u != null)
            {
                counterDict[userName] = u;
            }

            return u;
        }

        private AppUser GetUser(string userName, IPermissionQuery query)
        {
            var u = query.GetEmployeeUsingName(userName);
            if (u == null)
            {
                return null;
            }

            switch (u.GroupSort)
            {
                case GroupSort.Muggle:
                    {
                        u.Resources = (query.GetEmployeeResource(u.AggregateId) ?? new ResourceModel[] { }).ToActionDesciptors();
                    }
                    break;
                case GroupSort.Leader:
                    {
                        u.Resources = ((query.GetEmployeeResource(u.AggregateId) ?? new ResourceModel[] { }).Union(query.GetAllResource(GroupSort.Leader) ?? new ResourceModel[] { })).ToActionDesciptors();

                    }
                    break;
                case GroupSort.Super:
                    {
                        u.Resources = (query.GetAllResource(new[] { GroupSort.Muggle, GroupSort.Leader, GroupSort.Super }) ?? new ResourceModel[] { }).ToActionDesciptors();
                    }
                    break;
            }

            return EasyMapper.Map(u, new AppUser(), (x, y) => { y.Resources = x.Resources; });
        }

        private AppUser GetUserOnDebug(string userName)
        {
            return new AppUser()
            {
                UserName = userName,
                NickName = userName,
                AggregateId = userName,
                GroupSort = GroupSort.Muggle,
                Resources = B2C.Admin.Web.Permissions.StartupExtension.ActionDesciptors ?? Anonymous.NewEnumerable<ActionDesciptor>()
            };
        }

        private string GetUserNameFromTicket(AuthorizationFilterContext context, HttpRequest request, Task<AuthenticateResult> task)
        {
            var authen = task.GetAwaiter().GetResult();
            if (authen == null || authen.Properties == null || authen.Properties.Items == null || !authen.Properties.Items.ContainsKey("UserName"))
            {
                return string.Empty;
            }

            return authen.Properties.Items["UserName"];
        }

        private string GetUserTicketFromTicket(AuthorizationFilterContext context, HttpRequest request, Task<AuthenticateResult> task)
        {
            return task.Result.Properties.Items["UserTicket"];
        }

        #endregion user
    }
}