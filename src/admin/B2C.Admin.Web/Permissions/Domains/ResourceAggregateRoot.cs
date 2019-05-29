using Never;
using Never.Domains;
using Never.Exceptions;
using Never.Security;
using B2C.Infrastructure;
using B2C.Admin.Web.Permissions.Commands;
using B2C.Admin.Web.Permissions.EnumTypes;
using B2C.Admin.Web.Permissions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Domains
{
    public class ResourceAggregateRoot : _AggregateRoot<string>, IHandle<CreateResourceEvent>,
        IHandle<RemoveResourceEvent>,
        IHandle<ChangeResourceActionDescnEvent>
    {
        #region prop

        /// <summary>
        /// 资源描述
        /// </summary>
        public string ActionDescn { get; protected set; }

        /// <summary>
        /// 级的分类
        /// </summary>
        public GroupSort GroupSort { get; protected set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// orm
        /// </summary>
        private ResourceAggregateRoot()
            : base(string.Empty)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uniqueId"></param>
        private ResourceAggregateRoot(string uniqueId)
            : base(uniqueId)
        {
        }

        #endregion ctor

        #region regiseter

        public static ResourceAggregateRoot Register(IWorkContext context, CreateResourceCommand command)
        {
            return new ResourceAggregateRoot(command.AggregateId).Create(context, command);
        }

        public ResourceAggregateRoot Create(IWorkContext context, CreateResourceCommand command)
        {
            this.ApplyEvent(new CreateResourceEvent()
            {
                AggregateId = this.AggregateId,
                Version = this.Version,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),

                ActionDescn = command.Resource.ActionDescn,
                GroupSort = command.Resource.GroupSort
            });

            return this;
        }

        public void Handle(CreateResourceEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
            this.CreateDate = e.CreateDate;
            this.Creator = e.Creator;

            this.GroupSort = e.GroupSort;
            this.ActionDescn = e.ActionDescn;
        }

        #endregion regiseter

        #region delete

        public void Remove(IWorkContext context)
        {
            this.ApplyEvent(new RemoveResourceEvent()
            {
                AggregateId = this.AggregateId,
                Creator = context.GetWorkerName(),
                CreateDate = context.WorkTime,
                Version = this.Version
            });
        }

        public void Handle(RemoveResourceEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion delete

        #region change action

        public void ChangeInfo(IWorkContext context, string descn, GroupSort groupSort)
        {
            if (this.ActionDescn.IsEquals(descn) && this.GroupSort == groupSort)
                return;

            this.ApplyEvent(new ChangeResourceActionDescnEvent()
            {
                AggregateId = this.AggregateId,
                Creator = context.GetWorkerName(),
                CreateDate = context.WorkTime,
                Version = this.Version,
                Descn = descn,
                GroupSort = groupSort
            });
        }

        public void Handle(ChangeResourceActionDescnEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.ActionDescn = e.Descn;
            this.GroupSort = e.GroupSort;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion change action
    }
}