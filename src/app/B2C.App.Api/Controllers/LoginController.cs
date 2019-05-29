using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Never;
using Never.Attributes;
using Never.Security;
using Never.Utils;
using B2C.App.Api.Models;
using B2C.Message.Contract.EnumTypes;
using B2C.Message.Contract.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.App.Api.Controllers
{
    [Route("api/login/")]
    [ApiController]
    public class LoginController : AppController
    {
        #region field and ctor

        private readonly IUserProxyService userService = null;

        private readonly IVCodeService validateCodeService = null;

        public LoginController(IUserProxyService userService, IVCodeService validateCodeService)
        {
            this.userService = userService;
            this.validateCodeService = validateCodeService;
        }

        #endregion field and ctor

        #region 注册与登录

        /// <summary>
        /// 注册
        /// </summary>
        [HttpPost, ApiActionRemark("Register", "HttpPost")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!this.TryValidateModel(model))
            {
                return this.Json("0002", this.ModelErrorMessage);
            }

            var api = this.validateCodeService.CheckMobileValidateCode(new Message.Contract.Request.CheckMobileValidateCodeReqs
            {
                UsageType = UsageType.注册,
                Mobile = model.UserName,
                Platform = this.GetAppPlatform(),
                VCode = model.VCode,
            });

            if (api.Status != ApiStatus.Success)
            {
                return this.Json("0002", api.Status == ApiStatus.Fail ? api.Message : "验证码不正确");
            }

            var user = this.userService.Register(new RegisterUserReqs()
            {
                Mobile = model.UserName,
                Password = model.Password,
                RegisteIP = this.GetAppIP(),
                Platform = this.GetAppPlatform(),
            });

            if (user.Status != ApiStatus.Success)
            {
                return this.Json("0002", new { UserId = -1L }, user.Status == ApiStatus.Fail ? user.Message : "注册失败");
            }

            return this.Json("0000", new { UserId = user.Result.UserId }, string.Empty);
        }

        /// <summary>
        /// 登录，由于注册和登陆需要返回token,而token是host那边根据api~long这样的数据结构，所以这里不返回空对象
        /// </summary>
        [ApiActionRemark("Login", "HttpPost")]
        public IActionResult Login(LoginViewModel model)
        {
            if (!this.TryValidateModel(model))
            {
                return this.Json("0002", string.Empty, this.ModelErrorMessage);
            }

            var api = this.userService.Login(new LoginUserReqs()
            {
                Mobile = model.UserName,
                Password = model.Password,
                LoginIP = this.GetAppIP(),
                LoginTime = this.GetAppTime(),
                Platform = this.GetAppPlatform(),
            });

            if (api == null)
            {
                return this.Json("0002", string.Empty, "找不到手机号码");
            }

            if (api.Status != ApiStatus.Success)
            {
                return this.Json("0002", api.Status == ApiStatus.Fail ? api.Message : "登陆账号与密码出错");
            }

            if (api.Result == null)
            {
                return this.Json("0002", string.Empty, "找不到手机号码");
            }

            return this.Json("0000", new { UserId = api.Result.UserId }, string.Empty);
        }

        /// <summary>
        /// 登录，由于注册和登陆需要返回token,而token是host那边根据api~long这样的数据结构，所以这里不返回空对象
        /// </summary>
        [ApiActionRemark("Logout", "HttpPost"), UserPrincipal]
        public IActionResult Logout(LoginViewModel model)
        {
            return this.Json("0000", string.Empty);
        }
        #endregion 注册与登录

        #region 程序启动

        /// <summary>
        /// 程序启动
        /// </summary>
        [HttpPost, ApiActionRemark("Startup", "HttpPost"), UserPrincipal]
        public IActionResult AppStartup()
        {
            var platform = this.GetAppPlatform();
            var user = this.GetAppUser();
            var userid = user != null ? user.UserId : 0;

            return this.Json("0000", string.Empty);
        }

        #endregion 程序启动

        #region 忘记密码

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPost, ApiActionRemark("ForgetPwd", "HttpPost")]
        public IActionResult ForgetPwd(RegisterViewModel model)
        {
            if (!this.TryValidateModel(model))
            {
                return this.Json("0002", this.ModelErrorMessage);
            }

            var api = this.validateCodeService.CheckMobileValidateCode(new Message.Contract.Request.CheckMobileValidateCodeReqs
            {
                UsageType = UsageType.找回登录密码,
                Mobile = model.UserName,
                Platform = this.GetAppPlatform(),
                VCode = model.VCode,
            });

            if (api.Status != ApiStatus.Success)
            {
                return this.Json("0002", api.Status == ApiStatus.Fail ? api.Message : "验证码不正确");
            }

            api = this.userService.ChangePassword(new ChangePwdReqs()
            {
                Mobile = model.UserName,
                Password = model.Password,
            });

            if (api.Status != ApiStatus.Success)
            {
                return this.Json("0002", api.Status == ApiStatus.Fail ? api.Message : "修改密码错误");
            }

            return this.Json("0000", "修改成功");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPost, ApiActionRemark("ChangePwd", "HttpPost"), UserAuthorize]
        public IActionResult ChangePwd(ChangePwdViewModel model)
        {
            if (!this.TryValidateModel(model))
            {
                return this.Json("0002", this.ModelErrorMessage);
            }

            var api = this.userService.ChangePassword(new ChangePwdReqs()
            {
                UserId = this.GetAppUser().UserId,
                Password = model.Password,
                OriginalPwd = model.Original,
            });

            if (api.Status != ApiStatus.Success)
            {
                return this.Json("0002", api.Status == ApiStatus.Fail ? api.Message : "修改密码错误");
            }

            return this.Json("0000", "修改成功");
        }
        #endregion

        #region 客户端IP和其他信息

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <returns></returns>
        [UserPrincipal, HttpPost, ApiActionRemark("MobileInfo", "HttpPost")]
        public IActionResult MobileInfo()
        {
            return this.Json("0000", new
            {
                UserId = this.GetAppUser()?.UserId,
                UserName = this.GetAppUser()?.UserName,
                Platform = this.GetAppPlatform(),
                IPAddress = this.GetAppIP(),
                DateTime = this.GetAppTime(),
            });
        }
        #endregion
    }
}