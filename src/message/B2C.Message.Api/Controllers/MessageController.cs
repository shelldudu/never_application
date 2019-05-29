using Microsoft.AspNetCore.Mvc;
using Never;
using Never.Attributes;
using Never.Commands;
using Never.Logging;
using Never.Utils;
using Never.Web.WebApi.Controllers;
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
    public class MessageController : BasicController
    {
        #region field and ctor
        private readonly ICommandBus commandBus = null;
        private readonly ILoggerBuilder loggerBuilder = null;

        public MessageController(ICommandBus commandBus, ILoggerBuilder loggerBuilder)
        {
            this.commandBus = commandBus;
            this.loggerBuilder = loggerBuilder;
        }
        #endregion

        /// <summary>
        /// 短信验证码服务
        /// </summary>
        [ApiActionRemark("a9f40162ff31", "HttpPost"), HttpPost]
        public ApiResult<string> SendMobileVCode(ShortVCodeReqs reqs)
        {
            if (!this.TryValidateModel(reqs))
            {
                return Anonymous.NewApiResult(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
            }

            return this.ExecuteSendMobileVCode(reqs);
        }

        [NonAction]
        public ApiResult<string> ExecuteSendMobileVCode(ShortVCodeReqs reqs)
        {
            return Anonymous.NewApiResult(ApiStatus.Success, string.Empty);
        }
    }
}