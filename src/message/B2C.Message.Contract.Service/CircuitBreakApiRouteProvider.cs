using Never.Deployment;
using Never.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Message.Contract.Services
{
    /// <summary>
    /// 熔断机制，当url返回没有可用的时候，直接返回false信息
    /// </summary>
    public class CircuitBreakApiRouteProvider : ApiUriDispatcher<ApiRouteProvider>, IApiUriDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        public CircuitBreakApiRouteProvider(ApiUriDispatcher<ApiRouteProvider> dispatcher) : base(dispatcher.ApiRouteProvider, dispatcher.A10HealthReport)
        {
        }

        /// <summary>
        /// 阻止至少返回一个数据（如果可以的话）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="matchA10Content"></param>
        /// <returns></returns>
        protected override IApiUriHealthElement[] Select(IEnumerable<IApiUriHealthElement> source, Func<string, bool> matchA10Content)
        {
            var array = (from n in source where matchA10Content(n.ReportText) select n).ToArray();
            this.Write(source, array);
            return array;
        }

        /// <summary>
        /// 如果服务不可用，则返回保护状态
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="url"></param>
        /// <param name="requestPara"></param>
        /// <param name="headerParams"></param>
        /// <param name="timeout"></param>
        public override void Get(IJsonSerializer jsonSerializer, UrlConcat url, IDictionary<string, string> requestPara, IDictionary<string, string> headerParams, int timeout = 30000)
        {
            if (string.IsNullOrEmpty(url.Host))
                return;

            base.Get(jsonSerializer, url, requestPara, headerParams, timeout);
        }

        /// <summary>
        /// 如果服务不可用，则返回保护状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="url"></param>
        /// <param name="requestPara"></param>
        /// <param name="headerParams"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override T Get<T>(IJsonSerializer jsonSerializer, UrlConcat url, IDictionary<string, string> requestPara, IDictionary<string, string> headerParams, int timeout = 30000)
        {
            if (string.IsNullOrEmpty(url.Host))
                return default(T);

            return base.Get<T>(jsonSerializer, url, requestPara, headerParams, timeout);
        }

        /// <summary>
        /// 如果服务不可用，则返回保护状态
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="url"></param>
        /// <param name="jsonDate"></param>
        /// <param name="headerParams"></param>
        /// <param name="timeout"></param>
        public override void Post(IJsonSerializer jsonSerializer, UrlConcat url, string jsonDate, IDictionary<string, string> headerParams, int timeout = 30000)
        {
            if (string.IsNullOrEmpty(url.Host))
                return;

            base.Post(jsonSerializer, url, jsonDate, headerParams, timeout);
        }

        /// <summary>
        /// 如果服务不可用，则返回保护状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="url"></param>
        /// <param name="jsonDate"></param>
        /// <param name="headerParams"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override T Post<T>(IJsonSerializer jsonSerializer, UrlConcat url, string jsonDate, IDictionary<string, string> headerParams, int timeout = 30000)
        {
            if (string.IsNullOrEmpty(url.Host))
                return default(T);

            return base.Post<T>(jsonSerializer, url, jsonDate, headerParams, timeout);
        }
    }
}
