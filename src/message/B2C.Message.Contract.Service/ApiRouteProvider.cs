using Never;
using Never.Configuration;
using Never.Deployment;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Message.Contract.Services
{
    /// <summary>
    /// 路由提供者
    /// </summary>
    /// <seealso cref="DefaultApiRouteProvider" />
    [Never.Attributes.Summary(Descn = "读取配置文件中\"MessageA10\": {\"url\": [ \"http://127.0.0.1/api/\", \"http://127.0.0.1/api/\" ],\"ping\": [ \"http://127.0.0.1/a10\", \"http://127.0.0.1/a10\" ]}")]
    public class ApiRouteProvider : DefaultApiRouteProvider
    {
        private readonly IConfigReader configReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRouteProvider"/> class.
        /// </summary>
        public ApiRouteProvider(IConfigReader configReader)
        {
            this.configReader = configReader;
        }

        /// <summary>
        /// 获取A10资源信息
        /// </summary>
        public override IEnumerable<ApiUrlA10Element> ApiUrlA10Elements
        {
            get
            {
                var urls = new List<string>();
                var pings = new List<string>();
                for (var i = 0; i <= 100; i++)
                {
                    var url = this.configReader[string.Concat("MessageA10:url:", i.ToString())];
                    if (string.IsNullOrWhiteSpace(url))
                        break;

                    var ping = this.configReader[string.Concat("MessageA10:ping:", i.ToString())];
                    if (string.IsNullOrWhiteSpace(ping))
                        throw new System.Exception("ping与url条数不一致");

                    urls.Add(string.Concat(url.Trim().TrimEnd('/'),'/'));
                    pings.Add(ping.Trim());
                }

                if (urls.IsNullOrEmpty())
                    throw new Exception("当前config没有找到MessageA10的配置项目");

                for (var i = 0; i < urls.Count; i++)
                    yield return new ApiUrlA10Element() { ApiUrl = urls[i], A10Url = pings[i] };
            }
        }
    }
}
