using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Models
{
    /// <summary>
    /// 用户搜索
    /// </summary>
    public class UserScoutViewModel : Never.PagedSearch
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
    }
}