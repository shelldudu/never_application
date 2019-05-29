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
    public class DepartmentAggregateRoot : _AggregateRoot<string>, IHandle<CreateDepartmentEvnet>,
        IHandle<ChangeDepartmentEvent>, IHandle<RemoveDepartmentEvent>
    {
        #region ctor

        public DepartmentAggregateRoot() : this(string.Empty)
        {
        }

        public DepartmentAggregateRoot(string aggregateId) : base(aggregateId)
        {
        }

        #endregion ctor

        #region prop

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

        public static DepartmentAggregateRoot Register(IWorkContext context, CreateDepartmentCommand command)
        {
            return new DepartmentAggregateRoot(command.AggregateId).Create(context, command);
        }

        public DepartmentAggregateRoot Create(IWorkContext context, CreateDepartmentCommand command)
        {
            this.ApplyEvent(new CreateDepartmentEvnet
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                Name = command.Name,
                Descn = command.Descn,
            });

            return this;
        }

        public void Handle(CreateDepartmentEvnet e)
        {
            if (!IsContextCall)
                return;

            this.CreateDate = e.CreateDate;
            this.Creator = e.Creator;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;

            this.Name = e.Name;
            this.Descn = e.Descn;
        }

        #endregion register

        #region change

        public void ChangeInfo(IWorkContext context, ChangeDepartmentInfoCommand command)
        {
            this.ApplyEvent(new ChangeDepartmentEvent
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,

                Name = command.Name,
                Descn = command.Descn,
            });
        }

        public void Handle(ChangeDepartmentEvent e)
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

        public void Remove(IWorkContext context, RemoveDepartmentCommand command)
        {
            //Never.StructTuple
            this.ApplyEvent(new RemoveDepartmentEvent
            {
                AggregateId = this.AggregateId,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,
            });
        }

        public void Handle(RemoveDepartmentEvent e)
        {
            if (!IsContextCall)
                return;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion
    }
}
