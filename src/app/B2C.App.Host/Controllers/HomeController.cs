using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace B2C.App.Host.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet(""), HttpGet("api/home")]
        public string Index()
        {
            return "app";
        }

        [HttpGet("a10"), HttpGet("a10.html")]
        public string A10(string path)
        {
            try
            {
                return System.IO.File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "A10.html"));
            }
            catch
            {
                return "stop";
            }
        }

        [HttpGet(".well-known/pki-validation/fileauth.txt")]
        public ActionResult SSLTest()
        {
            return this.Content("201902121015215y949r32pl2qcc236yhtuujc49mfdcs8he4ya9tn77oj0j4qs5");
        }

    }
}
