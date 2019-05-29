using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Models
{
    /// <summary>
    /// 状态搜索
    /// </summary>
    public class StatusScoutViewModel : Never.PagedSearch
    {
        public long? UserId { get; set; }

        public int Status { get; set; }

    }
}
