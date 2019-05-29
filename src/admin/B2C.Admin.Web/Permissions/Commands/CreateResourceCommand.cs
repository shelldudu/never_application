using Never.CommandStreams;
using B2C.Admin.Web.Permissions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Commands
{
    [Serializable, CommandDomain(Domain = "admin")]
    public class CreateResourceCommand : Never.Domains.StringAggregateCommand
    {
        #region prop

        /// <summary>
        /// 资源列表
        /// </summary>
        public ActionResourceDescriptor Resource { get; set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResourceCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        public CreateResourceCommand(string aggregateId)
            : base(aggregateId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResourceCommand"/> class.
        /// </summary>
        private CreateResourceCommand()
            : this(string.Empty)
        {
        }

        #endregion ctor
    }
}