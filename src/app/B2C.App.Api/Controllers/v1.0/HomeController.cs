using Microsoft.AspNetCore.Mvc;
using Never.Attributes;
using Never.Configuration;
using Never.Logging;

namespace B2C.App.Api.Controllers.v1._0
{
    [Route("api/v1.0/")]
    [ApiController, UserPrincipal]
    public class HomeController : AppController
    {
        #region field and ctor

        private readonly ILoggerBuilder loggerBuilder = null;
        private readonly IConfigReader configReader = null;
        public HomeController(ILoggerBuilder loggerBuilder, IConfigReader configReader)
        {
            this.loggerBuilder = loggerBuilder; this.configReader = configReader;
        }

        #endregion


        /// <summary>
        /// 首页信息
        /// </summary>
        [HttpPost, ApiActionRemark("a9ac0187a2a1", "HttpPost")]
        public IActionResult Home()
        {
            return this.Json("0000", string.Empty);
        }
    }
}