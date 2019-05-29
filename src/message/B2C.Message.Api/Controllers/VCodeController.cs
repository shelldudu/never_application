using Microsoft.AspNetCore.Mvc;
using Never;
using Never.Attributes;
using Never.Commands;
using Never.Logging;
using Never.Mappers;
using Never.Serialization;
using Never.Utils;
using Never.Web.WebApi.Controllers;
using B2C.Message.Contract.Commands;
using B2C.Message.Contract.EnumTypes;
using B2C.Message.Contract.Request;
using B2C.Message.Contract.Services;
using B2C.Message.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Message.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class VCodeController : BasicController, IVCodeService
    {
        #region field and ctor

        private readonly IEmailCodeQuery emailCodeQuery = null;
        private readonly IMobileCodeQuery mobileCodeQuery = null;
        private readonly ICommandBus commandBus = null;
        private readonly ILoggerBuilder loggerBuilder = null;
        private readonly IJsonSerializer jsonSerializer = null;

        public VCodeController(ICommandBus commandBus,
            ILoggerBuilder loggerBuilder,
            IJsonSerializer jsonSerializer,
            IEmailCodeQuery emailCodeQuery,
            IMobileCodeQuery mobileCodeQuery)
        {
            this.commandBus = commandBus;
            this.loggerBuilder = loggerBuilder;
            this.jsonSerializer = jsonSerializer;
            this.emailCodeQuery = emailCodeQuery;
            this.mobileCodeQuery = mobileCodeQuery;
        }

        #endregion field and ctor

        /// <summary>
        /// 校验邮箱验证码
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aee8c6", "HttpPost"), HttpPost]
        public ApiResult<string> CheckEmailValidateCode(CheckEmailValidateCodeReqs reqs)
        {
            if (!this.TryValidateModel(reqs))
            {
                return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
            }

            try
            {
                var handler = this.commandBus.Send(new DestroyEmailCodeCommand(NewId.GenerateGuid())
                {
                    Email = reqs.Email,
                    UsageType = reqs.UsageType,
                    VCode = reqs.VCode,
                });

                if (handler == null)
                {
                    return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, "验证失败");
                }

                if (handler.Status != CommandHandlerStatus.Success)
                {
                    return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, this.HandlerMerssage(handler));
                }

                return Anonymous.NewApiResult(ApiStatus.Success, string.Empty);
            }
            catch (Exception ex)
            {
                this.loggerBuilder.Build(typeof(VCodeController)).Error("check email code error", ex);
                return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, ex.GetMessage());

            }
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="reqs">The request.</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aee071", "HttpPost"), HttpPost]
        public ApiResult<string> CheckMobileValidateCode(CheckMobileValidateCodeReqs reqs)
        {
            if (!this.TryValidateModel(reqs))
            {
                return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
            }

            try
            {
                var handler = this.commandBus.Send(new DestroyMobileCodeCommand(NewId.GenerateGuid())
                {
                    Mobile = reqs.Mobile,
                    UsageType = reqs.UsageType,
                    VCode = reqs.VCode,
                });

                if (handler == null)
                {
                    return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, "验证失败");
                }

                if (handler.Status != CommandHandlerStatus.Success)
                {
                    return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, this.HandlerMerssage(handler));
                }

                return Anonymous.NewApiResult(ApiStatus.Success, string.Empty);
            }
            catch (Exception ex)
            {
                this.loggerBuilder.Build(typeof(VCodeController)).Error("check email code error", ex);
                return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, ex.GetMessage());
            }
        }

        /// <summary>
        /// 创建邮箱验证码
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aee49a", "HttpPost"), HttpPost]
        public ApiResult<string> CreateEmailValidateCode(CreateEmailValidateCodeReqs reqs)
        {
            if (!this.TryValidateModel(reqs))
            {
                return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
            }

            try
            {
                var handler = this.commandBus.Send(new CreateEmailCodeCommand(NewId.GenerateGuid())
                {
                    Email = reqs.Email,
                    UsageType = reqs.UsageType <= 0 ? UsageType.登录 : reqs.UsageType,
                    ClientIP = reqs.ClientIP,
                    Length = reqs.Length,
                    Platform = reqs.Platform,
                });

                if (handler == null)
                {
                    return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, "获取失败");
                }

                if (handler.Status != CommandHandlerStatus.Success)
                {
                    return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, this.HandlerMerssage(handler));
                }

                return Anonymous.NewApiResult(ApiStatus.Success, string.Empty);
            }
            catch (Exception ex)
            {
                this.loggerBuilder.Build(typeof(VCodeController)).Error("get email code error", ex);
                return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, ex.GetMessage());
            };
        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="reqs">The request.</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aedbb5", "HttpPost"), HttpPost]
        public ApiResult<string> CreateMobileValidateCode(CreateMobileValidateCodeReqs reqs)
        {
            if (!this.TryValidateModel(reqs))
            {
                return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
            }

            var max = this.mobileCodeQuery.Max(reqs.Mobile.AsLong());
            if (max != null && max.UsageStatus == UsageStatus.未使用 && max.ExpireTime > DateTime.Now)
            {
                return Anonymous.NewApiResult(ApiStatus.Success, string.Empty);
            }

            try
            {
                var handler = this.commandBus.Send(new CreateMobileCodeCommand(NewId.GenerateGuid())
                {
                    Mobile = reqs.Mobile,
                    UsageType = reqs.UsageType <= 0 ? UsageType.登录 : reqs.UsageType,
                    ClientIP = reqs.ClientIP,
                    Length = reqs.Length,
                    Platform = reqs.Platform,
                });

                if (handler == null)
                {
                    return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, "获取失败");
                }

                if (handler.Status != CommandHandlerStatus.Success)
                {
                    return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, this.HandlerMerssage(handler));
                }

                return Anonymous.NewApiResult(ApiStatus.Success, string.Empty);
            }
            catch (Exception ex)
            {
                this.loggerBuilder.Build(typeof(VCodeController)).Error("get email code error", ex);
                return Anonymous.NewApiResult(ApiStatus.Error, string.Empty, ex.GetMessage());
            }
        }
    }
}