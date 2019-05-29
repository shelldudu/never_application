using Never.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.App.Api
{
    public class AppUser : IUser
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public bool Equals(IUser other)
        {
            return this.UserId == other.UserId;
        }
    }
}