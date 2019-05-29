using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Never;
using Never.Security;
using B2C.Admin.Web.Permissions;
using B2C.Admin.Web.Permissions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Admin.Web
{
    /// <summary>
    /// 后台用户验证，主要是操作验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class UserOperationAuthorizeAttribute : UserAuthorizeAttribute
    {
        #region utils

        public void Unauthorized(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                return;
            }

            if (this.IsAjaxRequest(context.HttpContext.Request))
            {
                context.HttpContext.Response.Clear();
                var api = new
                {
                    status = -2,
                    result = string.Empty,
                    message = "你没有权限操作该资源"
                };

                context.Result = new JsonResult(api);
                return;
            }

            context.Result = new RedirectToActionResult("Status401", "Login", new { @area = "" });
            return;
        }

        public void RegirectToLoginUrl(AuthorizationFilterContext context)
        {
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

        protected override void OnAuthorizeCompleted(AuthorizationFilterContext context, IUser user)
        {
            //拿用户信息
            var member = user as AppUser;
            /*不是登陆状态，没权限*/
            if (member == null)
            {
                this.RegirectToLoginUrl(context);
                return;
            }

            var descriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            if (descriptor == null)
            {
                this.Unauthorized(context);
                return;
            }

            /*判断匿名访问资源*/
            var allowAnonymousAttributes = descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            if (allowAnonymousAttributes != null && allowAnonymousAttributes.Any())
            {
                return;
            }

            allowAnonymousAttributes = descriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            if (allowAnonymousAttributes != null && allowAnonymousAttributes.Any())
            {
                return;
            }

            var actionResultNameAttributes = descriptor.MethodInfo.GetAttributes<ActionResourceAttribute>();
            //没权限
            if (member == null || member.Resources == null || member.Resources.Count() == 0)
            {
                this.Unauthorized(context);
                return;
            }

            //没标记有这个ActionResultNameAttributes属性的统一更改为没有权限
            if (actionResultNameAttributes == null || actionResultNameAttributes.Count() == 0)
            {
                this.Unauthorized(context);
                return;
            }

            //虽然用户有登录后台的权限，但没该资源访问的权限
            if (!this.CheckOperation(context, actionResultNameAttributes.FirstOrDefault(), member.Resources))
            {
                this.Unauthorized(context);
                return;
            }
        }

        public bool CheckOperation(AuthorizationFilterContext filterContext, ActionResourceAttribute actionResultNameAttribute, IEnumerable<ActionDesciptor> roleList)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext不能为空");
            }

            if (actionResultNameAttribute == null)
            {
                throw new ArgumentNullException("actionResultNameAttribute不能为空");
            }

            if (roleList == null)
            {
                throw new ArgumentNullException("roleList不能为空");
            }

            var descriptor = filterContext.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            string areaName = descriptor.RouteValues != null && descriptor.RouteValues.ContainsKey("area") ? (descriptor.RouteValues["area"] as string) : "";
            if (areaName == null)
            {
                areaName = string.Empty;
            }

            //查询是否有权限
            foreach (var role in roleList.Where(o => (o.Controller ?? "").Equals(descriptor.ControllerName + "Controller", StringComparison.OrdinalIgnoreCase)
                && (o.ActionResult ?? "").Equals(descriptor.ActionName, StringComparison.OrdinalIgnoreCase)
                && (o.Area ?? "").Equals(areaName, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        #endregion utils
    }
}