using B2C.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Models
{
    public class OwnerGroupModel : GroupModel
    {
        public string OwnerId { get; set; }
    }
}
