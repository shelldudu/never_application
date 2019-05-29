using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Never;
using Never.Security;
using Never.Utils;
using B2C.Admin.Web.Models;
using B2C.Admin.Web.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Controllers
{
    public class LoginController : AppController
    {
        #region field

        /// <summary>
        /// 权限
        /// </summary>
        private readonly IPermissionQuery permissionQuery = null;

        /// <summary>
        /// 登陆
        /// </summary>
        private readonly IAuthenticationService authenticationService = null;

        #endregion field

        #region ctor

        public LoginController(IPermissionQuery permissionQuery,
            IAuthenticationService authenticationService)
        {
            this.permissionQuery = permissionQuery;
            this.authenticationService = authenticationService;
        }

        #endregion ctor

        #region 登录

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        [NonAction, AllowAnonymous]
        protected async Task SignIn(IUser user)
        {
            await this.authenticationService.SignIn(this.HttpContext, user);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="returnUrl">返回地址</param>
        /// <returns></returns>
        public ActionResult Login(string returnUrl)
        {
            return this.View(new LoginViewModel());
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="captchaValid"></param>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!this.TryValidateModel(model))
                return this.View(model);

            string img = this.HttpContext.Session.GetString("LoginRandCode");
            //登录验证码，因为已有防伪标识
            if (model.RandCode.IsNotEquals(img))
            {
                this.ModelState.AddModelError("RandCode", "验证码不正确");
                return this.View(model);
            }

            var user = this.permissionQuery.GetEmployeeUsingName(model.UserName);
            if (user == null)
            {
                this.ModelState.AddModelError("UserName", "登录信息不正确");
                return this.View(model);
            }

            if (!user.Password.Equals(model.Password.Trim().ToSHA256()))
            {
                this.ModelState.AddModelError("UserName", "登录信息不正确");
                return this.View(model);
            }

            //登录了
            await this.SignIn(this.Map(user, new AppUser()));

            return this.RedirectToAction("Index", "Home");
        }

        #endregion 登录

        #region 创建图形验证码

        /// <summary>
        /// 注册时生成验证码图片
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public ActionResult RandCode(int height, int width)
        {
            if (height > 100)
                height = 100;

            if (width > 200)
                width = 200;

            string randCode = string.Join("", Randomizer.RandomArrary(10, 4));
            this.HttpContext.Session.SetString("LoginRandCode", randCode);
            byte[] imgs = null;
            using (var ms = DrawingHelper.CreateImage(randCode, height, width))
                imgs = ms.ToArray();

            return new FileContentResult(imgs, "image/gif");
        }

        #endregion 创建图形验证码

        #region 退出

        [NonAction]
        protected async Task SingOut()
        {
            await this.authenticationService.SignOut(this.HttpContext, this.CurrentUser);
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Logout()
        {
            await this.SingOut();
            return RedirectToAction("login");
        }

        #endregion 退出

        #region 404,405,401

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Status401()
        {
            return this.View();
        }
        #endregion
    }
}