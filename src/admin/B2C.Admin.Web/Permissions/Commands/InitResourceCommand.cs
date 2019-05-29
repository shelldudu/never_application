using Never.CommandStreams;
using B2C.Admin.Web.Permissions.Attributes;
using B2C.Admin.Web.Permissions.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Commands
{
    [Serializable, CommandDomain(Domain = "admin")]
    public class InitResourceCommand : Never.Domains.StringAggregateCommand
    {
        #region prop

        /// <summary>
        /// 资源列表
        /// </summary>
        public IEnumerable<ActionResourceDescriptor> Resources { get; set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="InitResourceCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public InitResourceCommand()
            : base(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitResourceCommand"/> class.
        /// </summary>
        private InitResourceCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        #endregion ctor
    }

    /// <summary>
    ///
    /// </summary>
    public struct ActionResourceDescriptor
    {
        /// <summary>
        /// 排序
        /// </summary>
        public string AggregateId { get; set; }

        /// <summary>
        /// action名称
        /// </summary>
        public string ActionDescn { get; set; }

        /// <summary>
        ///
        /// </summary>
        public GroupSort GroupSort { get; set; }
    }
}