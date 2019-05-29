using Never.Security;
using B2C.Admin.Web.Permissions.EnumTypes;
using B2C.Admin.Web.Permissions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web
{
    /// <summary>
    /// 登陆用户
    /// </summary>
    public class AppUser: EmployeeModel, IUser, IWorker
    {
        #region iuser

        long IUser.UserId => this.Id;

        string IUser.UserName => this.UserName;

        string IWorker.WorkerName => this.UserName;

        long IWorker.WorkerId => this.Id;

        bool IEquatable<IUser>.Equals(IUser other)
        {
            return this.UserName == other.UserName;
        }

        #endregion iuser
    }
}
