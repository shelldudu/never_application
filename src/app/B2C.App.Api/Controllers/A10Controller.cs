using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.App.Api.Controllers
{
    [ApiController]
    public class A10Controller : ControllerBase
    {
        #region a10

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

        #endregion a10
    }
}