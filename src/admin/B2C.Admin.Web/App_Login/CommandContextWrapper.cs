using Microsoft.AspNetCore.Http;
using Never;
using Never.Commands;
using Never.Domains;
using Never.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2C.Admin.Web
{
    public class CommandContextWrapper : DefaultCommandContext
    {
        #region ctor

        public CommandContextWrapper()
        {
        }

        #endregion ctor

        #region init

        /// <summary>
        /// 验证信息
        /// </summary>
        /// <param name="root">聚合根</param>
        /// <param name="command">命令</param>
        public override void Validate(IAggregateRoot root, ICommand command)
        {
            base.Validate(root, command);
        }

        public override void OnInit(HandlerCommunication communication, ICommand command)
        {
            base.OnInit(communication, command);
            if (this.Consigner != null && this.Worker != null)
                return;

            UserPrincipal principal = System.Threading.Thread.CurrentPrincipal as UserPrincipal;
            if (principal == null)
            {
                try
                {
                    using (var sc = Never.IoC.ContainerContext.Current.ServiceLocator.BeginLifetimeScope())
                    {
                        var accessor = sc.Resolve<IHttpContextAccessor>();
                        var authservice = sc.Resolve<IAuthenticationService>();
                        principal = authservice.GetUserPrincipal(accessor.HttpContext);
                    }
                }
                catch
                {

                }
            }

            if (principal == null)
                return;

            this.Consigner = principal.CurrentUser;
            this.Worker = principal.CurrentUser as IWorker;
        }

        #endregion init
    }
}