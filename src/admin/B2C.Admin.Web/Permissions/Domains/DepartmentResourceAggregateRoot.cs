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
    public class DepartmentResourceAggregateRoot : _AggregateRoot<string>, IHandle<CreateDepartmentResouceEvnet>, IHandle<RemoveDepartmentResouceEvnet>
    {
        #region ctor

        public DepartmentResourceAggregateRoot() : this(string.Empty)
        {
        }

        public DepartmentResourceAggregateRoot(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; protected set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public string ResourceId { get; protected set; }

        #endregion prop

        #region register

        public static DepartmentResourceAggregateRoot Register(IWorkContext context, CreateDepartmentResourceCommand command)
        {
            return new DepartmentResourceAggregateRoot(command.AggregateId).Create(context, command);
        }

        public DepartmentResourceAggregateRoot Create(IWorkContext context, CreateDepartmentResourceCommand command)
        {
            this.ApplyEvent(new CreateDepartmentResouceEvnet
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                DepartmentId = command.DepartmentId,
                ResourceId = command.ResourceId,
            });

            return this;
        }

        public void Handle(CreateDepartmentResouceEvnet e)
        {
            if (!IsContextCall)
                return;

            this.CreateDate = e.CreateDate;
            this.Creator = e.Creator;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;

            this.DepartmentId = e.DepartmentId;
            this.ResourceId = e.ResourceId;
        }

        #endregion register

        #region delete

        public void Remove(IWorkContext context)
        {
            this.ApplyEvent(new RemoveDepartmentResouceEvnet
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                DepartmentId = this.DepartmentId,
                ResourceId = this.ResourceId,
            });
        }

        public void Handle(RemoveDepartmentResouceEvnet e)
        {
            if (!IsContextCall)
                return;


            this.DepartmentId = e.DepartmentId;
            this.ResourceId = e.ResourceId;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion change
    }
}
