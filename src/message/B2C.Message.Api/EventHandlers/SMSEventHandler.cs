using Never;
using Never.Events;
using B2C.Message.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Message.Api.EventHandlers
{
    public class SMSEventHandler : IEventHandler<CreateMobileCodeEvent>
    {
        public void Execute(IEventContext context, CreateMobileCodeEvent e)
        {
            if (Never.IoC.ContainerContext.Current == null || e.Mobile <= 0 || e.Mobile.ToString().IsChineseMobile() == false || e.Message.IsNullOrWhiteSpace())
                return;

            using (var sc = Never.IoC.ContainerContext.Current.ServiceLocator.BeginLifetimeScope())
            {
                var service = sc.Resolve<Controllers.MessageController>(string.Empty);
                service.ExecuteSendMobileVCode(new Contract.Request.ShortVCodeReqs() { Mobile = e.Mobile.ToString(), VCode = e.Message });
            }
        }
    }
}
