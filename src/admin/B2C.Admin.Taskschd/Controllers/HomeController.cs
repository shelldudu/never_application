using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Taskschd.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet(""), HttpGet("api/home")]
        public string Index()
        {
            return "ok";
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
    }
}