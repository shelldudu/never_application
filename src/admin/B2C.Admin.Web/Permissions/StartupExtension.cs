using Microsoft.AspNetCore.Authorization;
using Never;
using Never.Commands;
using Never.Startups;
using Never.Web.Mvc.Controllers;
using B2C.Admin.Web.Permissions.Attributes;
using B2C.Admin.Web.Permissions.Commands;
using B2C.Admin.Web.Permissions.EnumTypes;
using B2C.Admin.Web.Permissions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions
{
    /// <summary>
    ///
    /// </summary>
    public static class StartupExtension
    {
        /// <summary>
        /// 所有的资源描述
        /// </summary>
        public static IEnumerable<ActionDesciptor> ActionDesciptors = null;

        /// <summary>
        /// 使用权限管理系统
        /// </summary>
        /// <param name="startup">The startup.</param>
        /// <returns></returns>
        public static ApplicationStartup UseMvcPermission(this ApplicationStartup startup)
        {
            return startup.RegisterStartService(true, (context) =>
             {
                 var list = new List<ActionResourceAttribute>(300);
                 ActionDesciptors = new List<ActionDesciptor>(100);
                 foreach (var assembly in context.FilteringAssemblyProvider.GetAssemblies())
                 {
                     if (assembly == null)
                         continue;

                     var controllers = from k in assembly.GetTypes()
                                       where typeof(BasicController).IsAssignableFrom(k)
                                        && !k.IsAbstract
                                        && k.IsClass && k.Name.EndsWith("Controller", StringComparison.Ordinal)
                                       select k;

                     if (controllers == null || !controllers.Any())
                         continue;

                     var temp = FindAllResourceModule(controllers, (List<ActionDesciptor>)ActionDesciptors);
                     if (temp != null && temp.Any())
                         list.AddRange(temp);
                 }

                 var command = new InitResourceCommand()
                 {
                     Resources = from n in list
                                 let leader = n as LeaderActionResourceAttribute
                                 let super = n as SuperActionResourceAttribute
                                 select new ActionResourceDescriptor()
                                 {
                                     AggregateId = n.AggregateId,
                                     ActionDescn = n.ActionDescn,
                                     GroupSort = super != null ? GroupSort.Super : (leader != null ? GroupSort.Leader : GroupSort.Muggle)
                                 }
                 };

                 using (var scope = context.ServiceLocator.BeginLifetimeScope())
                 {
                     var bus = scope.Resolve<ICommandBus>();
                     bus.Send(command);
                 }
             });

            //查询资源
            IEnumerable<ActionResourceAttribute> FindAllResourceModule(IEnumerable<Type> controllers, List<ActionDesciptor> actionDesciptors)
            {
                if (controllers == null || controllers.Count() == 0)
                    return null;

                var list = new List<ActionResourceAttribute>(controllers.Count() * 10);
                foreach (var controller in controllers)
                {
                    foreach (var methodInfo in controller.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                    {
                        /*判断匿名访问资源*/
                        var allowAnonymousAttributes = methodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
                        if (allowAnonymousAttributes != null && allowAnonymousAttributes.Any())
                            continue;

                        var attributes = methodInfo.GetCustomAttributes(typeof(ActionResourceAttribute), true) as IEnumerable<ActionResourceAttribute>;
                        //没有属性，则不受管理
                        if (attributes == null || attributes.Count() <= 0)
                            continue;

                        var attribute = attributes.FirstOrDefault();
                        if (list.FirstOrDefault(o => o.AggregateId.Equals(attribute.AggregateId)) != null)
                            throw new Exception(string.Format("存在多个相同的id:{0}，会引起分配权限混乱", attribute.AggregateId));

                        actionDesciptors.Add(new ActionDesciptor()
                        {
                            ActionResult = methodInfo.Name,
                            Controller = controller.Name,
                            AggregateId = attribute.AggregateId,
                            Area = Regex.Match(controller.FullName, "\\.Areas\\.(?<name>\\w+)\\.Controllers", RegexOptions.IgnoreCase).Groups["name"].Value,
                        });

                        list.Add(attribute);
                    }
                }

                return list;
            }
        }

        public static IEnumerable<ActionDesciptor> ToActionDesciptors(this IEnumerable<ResourceModel> resources)
        {
            if (resources.IsNullOrEmpty() || ActionDesciptors.IsNullOrEmpty())
                return Anonymous.NewEnumerable<ActionDesciptor>();

            return from n in resources
                   let contains = ActionDesciptors.Any(g => g.AggregateId == n.AggregateId)
                   let tor = contains ? ActionDesciptors.FirstOrDefault(g => g.AggregateId == n.AggregateId) : new ActionDesciptor()
                   where contains == true
                   select new ActionDesciptor()
                   {
                       AggregateId = n.AggregateId,
                       ActionResult = tor.ActionResult,
                       Area = tor.Area,
                       Controller = tor.Controller
                   };
        }
    }
}