using B2C.App.Api.Models;
using B2C.Message.Contract.Services;
using Microsoft.AspNetCore.Mvc;
using Never;
using Never.Attributes;

namespace B2C.App.Api.Controllers.v1._0
{
    [Route("api/v1.0/")]
    [ApiController, UserPrincipal]
    public class MessageController : AppController
    {
        #region field and ctor

        private readonly IUserProxyService userService = null;

        private readonly IVCodeService validateCodeService = null;

        public MessageController(IUserProxyService userService,IVCodeService validateCodeService)
        {
            this.userService = userService;
            this.validateCodeService = validateCodeService;
        }

        #endregion field and ctor

        /// <summary>
        /// 注册发送验证码
        /// </summary>
        [HttpPost, ApiActionRemark("SendRegisterVCode", "HttpPost")]
        public IActionResult Register(UserViewModel model)
        {
            if (!this.TryValidateModel(model))
            {
                return this.Json("0002", this.ModelErrorMessage);
            }

            //先检查用户是否存在
            if (this.UserExists(model.UserName))
                return this.Json("0002", "手机号码存在异常，发送失败");

            var api = this.validateCodeService.CreateMobileValidateCode(new Message.Contract.Request.CreateMobileValidateCodeReqs()
            {
                Mobile = model.UserName,
                ClientIP = this.GetAppIP(),
                Platform = this.GetAppPlatform(),
                Length = 4,
                UsageType = Message.Contract.EnumTypes.UsageType.注册,
            });

            if (api.Status != ApiStatus.Success)
            {
                return this.Json("0002", api.Status == ApiStatus.Fail ? api.Message : "发送失败");
            }

            return this.Json("0000", "发送成功");
        }

        /// <summary>
        /// 忘记发送验证码
        /// </summary>
        [HttpPost, ApiActionRemark("ForgetPwdVCode", "HttpPost")]
        public IActionResult Forget(UserViewModel model)
        {
            if (!this.TryValidateModel(model))
            {
                return this.Json("0002", this.ModelErrorMessage);
            }

            //先检查用户是否存在
            if (!this.UserExists(model.UserName))
                return this.Json("0002", "手机号码存在异常，发送失败");

            var api = this.validateCodeService.CreateMobileValidateCode(new Message.Contract.Request.CreateMobileValidateCodeReqs()
            {
                Mobile = model.UserName,
                ClientIP = this.GetAppIP(),
                Platform = this.GetAppPlatform(),
                Length = 4,
                UsageType = Message.Contract.EnumTypes.UsageType.找回登录密码,
            });

            if (api.Status != ApiStatus.Success)
            {
                return this.Json("0002", api.Status == ApiStatus.Fail ? api.Message : "发送失败");
            }

            return this.Json("0000", "发送成功");
        }

        /// <summary>
        /// 检查用户个数
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>

        public bool UserExists(string mobile)
        {
            return this.userService.GetUser(mobile.AsLong()).Status == ApiStatus.Success;
        }
    }
}