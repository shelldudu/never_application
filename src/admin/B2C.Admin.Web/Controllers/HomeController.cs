using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Never;
using Never.Commands;
using Never.Web.Mvc;
using B2C.Admin.Web.Models;
using B2C.Admin.Web.Permissions.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Controllers
{
    [UserOperationAuthorize]
    public class HomeController : AppController
    {
        #region ctor

        private readonly ICommandBus commandBus = null;

        public HomeController(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        #endregion ctor

        #region sys

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        #endregion sys
    }
}