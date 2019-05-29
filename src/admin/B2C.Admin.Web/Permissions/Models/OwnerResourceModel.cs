using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Models
{
    /// <summary>
    /// 带授权的资源列表
    /// </summary>
    public class OwnerResourceModel: ResourceModel
    {
        public string OwnerId { get; set; }
    }
}
