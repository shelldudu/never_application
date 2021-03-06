﻿using Never.EventStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Events
{
    [Serializable, EventDomain(Domain = "admin")]
    public class ChangeGroupInfoEvent : Never.Events.AggregateRootChangeEvent<string>
    {
        #region prop

        public string Name { get; set; }

        public string Descn { get; set; }

        #endregion prop
    }
}
