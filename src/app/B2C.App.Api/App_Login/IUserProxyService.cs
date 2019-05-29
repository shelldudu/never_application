using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.App.Api
{
    public interface IUserProxyService: IUserService
    {
        IUserService Service { get; }
    }
}
