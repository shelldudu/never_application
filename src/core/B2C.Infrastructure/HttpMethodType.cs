using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// http方法类型
    /// </summary>
    public enum HttpMethodType
    {
        /// <summary>
        /// HttpGet
        /// </summary>
        HttpGet = 1,

        /// <summary>
        /// HttpPost
        /// </summary>
        HttpPost = 2,

        /// <summary>
        /// HttpDelete
        /// </summary>
        HttpDelete = 4,

        /// <summary>
        /// HttpHead
        /// </summary>
        HttpHead = 8,

        /// <summary>
        /// HttpOptions
        /// </summary>
        HttpOptions = 16,

        /// <summary>
        /// HttpPatch
        /// </summary>
        HttpPatch = 32,

        /// <summary>
        /// HttpPut
        /// </summary>
        HttpPut = 64
    }
}
