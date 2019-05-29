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
    public class GroupResourceAggregateRoot : _AggregateRoot<string>, IHandle<CreateGroupResouceEvnet>, IHandle<RemoveGroupResouceEvnet>
    {
        #region ctor

        public GroupResourceAggregateRoot() : this(string.Empty)
        {
        }

        public GroupResourceAggregateRoot(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; protected set; }

        /// <summary>
        /// 组Id
        /// </summary>
        public string GroupId { get; protected set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public string ResourceId { get; protected set; }

        #endregion prop

        #region register

        public static GroupResourceAggregateRoot Register(IWorkContext context, CreateGroupResourceCommand command, DepartmentAggregateRoot department)
        {
            return new GroupResourceAggregateRoot(command.AggregateId).Create(context, command, department);
        }

        public GroupResourceAggregateRoot Create(IWorkContext context, CreateGroupResourceCommand command, DepartmentAggregateRoot department)
        {
            this.ApplyEvent(new CreateGroupResouceEvnet
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                GroupId = command.GroupId,
                ResourceId = command.ResourceId,
                DepartmentId = department.AggregateId,
            });

            return this;
        }

        public void Handle(CreateGroupResouceEvnet e)
        {
            if (!IsContextCall)
                return;

            this.CreateDate = e.CreateDate;
            this.Creator = e.Creator;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;

            this.DepartmentId = e.DepartmentId;
            this.GroupId = e.GroupId;
            this.ResourceId = e.ResourceId;
        }

        #endregion register

        #region delete

        public void Remove(IWorkContext context)
        {
            this.ApplyEvent(new RemoveGroupResouceEvnet
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                GroupId = this.GroupId,
                ResourceId = this.ResourceId,
            });
        }

        public void Handle(RemoveGroupResouceEvnet e)
        {
            if (!IsContextCall)
                return;


            this.GroupId = e.GroupId;
            this.ResourceId = e.ResourceId;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion change
    }
}
