using B2C.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Models
{
    public class ResourceModel : _AggregateModel<string>
    {
        #region prop

        /// <summary>
        /// 资源描述
        /// </summary>
        public string ActionDescn { get;  set; }

        #endregion prop
    }
}
