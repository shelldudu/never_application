using Never;
using Never.Attributes;
using Never.Deployment;
using Never.IoC;
using Never.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace B2C.Message.Contract.Services
{
    /// <summary>
    /// 扩展信息
    /// </summary>
    public static class StartupExtension
    {
        /// <summary>
        /// 自动生成代理
        /// </summary>
        /// <param name="startup"></param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static ApplicationStartup UseHttpProxyGenerateMessageApi(this ApplicationStartup startup, IJsonSerializer jsonSerializer = null, Action<HttpServiceProxyFactory.OnCallingEventArgs> callback = null)
        {
            return startup.RegisterStartService(110, (x) =>
            {
                var assembly = x.FilteringAssemblyProvider.GetAssemblies().FirstOrDefault(t => t.FullName == typeof(IVCodeService).Assembly.FullName);
                var provider = new Func<IApiUriDispatcher>(() => startup.ServiceLocator.Resolve<ApiUriDispatcher<ApiRouteProvider>>());
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsInterface && type.Name.Contains("Service"))
                    {
                        if (type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Any(t => t.GetAttribute<ApiActionRemarkAttribute>() != null))
                        {
                            var gtype = HttpServiceProxyFactory.CreateProxy(type, provider, jsonSerializer ?? SerializeEnvironment.JsonSerializer, callback);
                            x.ServiceRegister.RegisterType(gtype, type, string.Empty, ComponentLifeStyle.Singleton);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 自动生成代理,带熔断机制
        /// </summary>
        /// <param name="startup"></param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static ApplicationStartup UseHttpProxyGenerateMessageCircuitBreakerApi(this ApplicationStartup startup, IJsonSerializer jsonSerializer = null, Action<HttpServiceProxyFactory.OnCallingEventArgs> callback = null)
        {
            return startup.RegisterStartService(110, (x) =>
            {
                var assembly = x.FilteringAssemblyProvider.GetAssemblies().FirstOrDefault(t => t.FullName == typeof(IVCodeService).Assembly.FullName);
                var provider = new Func<IApiUriDispatcher>(() => new CircuitBreakApiRouteProvider(startup.ServiceLocator.Resolve<ApiUriDispatcher<ApiRouteProvider>>()));
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsInterface && type.Name.Contains("Service"))
                    {
                        if (type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Any(t => t.GetAttribute<ApiActionRemarkAttribute>() != null))
                        {
                            var gtype = HttpServiceProxyFactory.CreateProxy(type, provider, jsonSerializer ?? SerializeEnvironment.JsonSerializer, callback);
                            x.ServiceRegister.RegisterType(gtype, type, string.Empty, ComponentLifeStyle.Singleton);
                        }
                    }
                }
            });
        }
    }
}
