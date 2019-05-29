using Never;
using Never.Domains;
using Never.Security;
using B2C.Infrastructure;
using B2C.Admin.Web.Permissions.Commands;
using B2C.Admin.Web.Permissions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Domains
{
    public class GroupAggregateRoot : _AggregateRoot<string>, IHandle<CreateGroupEvnet>,
        IHandle<ChangeGroupInfoEvent>, IHandle<RemoveGroupEvent>
    {
        #region ctor

        private GroupAggregateRoot() : this(string.Empty)
        {
        }

        private GroupAggregateRoot(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; protected set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        public string Descn { get; protected set; }

        #endregion prop

        #region register

        public static GroupAggregateRoot Register(IWorkContext context, CreateGroupCommand command, DepartmentAggregateRoot department)
        {
            return new GroupAggregateRoot(command.AggregateId).Create(context, command, department);
        }

        public GroupAggregateRoot Create(IWorkContext context, CreateGroupCommand command, DepartmentAggregateRoot department)
        {
            this.ApplyEvent(new CreateGroupEvnet
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                DepartmentId = department.AggregateId,
                Name = command.Name,
                Descn = command.Descn,
            });

            return this;
        }

        public void Handle(CreateGroupEvnet e)
        {
            if (!IsContextCall)
                return;

            this.CreateDate = e.CreateDate;
            this.Creator = e.Creator;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;

            this.DepartmentId = e.DepartmentId;
            this.Name = e.Name;
            this.Descn = e.Descn;
        }

        #endregion register

        #region change

        public void ChangeInfo(IWorkContext context, ChangeGroupInfoCommand command)
        {
            this.ApplyEvent(new ChangeGroupInfoEvent
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                Name = command.Name,
                Descn = command.Descn,
            });
        }

        public void Handle(ChangeGroupInfoEvent e)
        {
            if (!IsContextCall)
                return;

            this.Name = e.Name;
            this.Descn = e.Descn;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion change

        #region remove

        public void Remove(IWorkContext context, RemoveGroupCommand command)
        {
            this.ApplyEvent(new RemoveGroupEvent()
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                DepartmentId = this.DepartmentId,
            });
        }

        public void Handle(RemoveGroupEvent e)
        {
            if (!IsContextCall)
                return;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion remove
    }
}