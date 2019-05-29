using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 行为类型
    /// </summary>
    public enum ActionType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        系统 = 0,

        /// <summary>
        /// 
        /// </summary>
        用户 = 1,

        /// <summary>
        /// 
        /// </summary>
        手工 = 5,
    }
}