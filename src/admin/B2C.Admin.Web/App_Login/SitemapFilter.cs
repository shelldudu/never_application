using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web
{
    public class SitemapFilter : IAuthorizationFilter
    {
        #region ctor

        /// <summary>
        ///
        /// </summary>
        private static Sitemap sitemap = null;

        /// <summary>
        ///
        /// </summary>
        static SitemapFilter()
        {
            sitemap = new Sitemap();
            sitemap.Startup();
        }

        #endregion ctor

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.RouteData.Values["B2CMenu"] = sitemap.Menu;
        }
    }
}