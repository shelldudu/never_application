using Never;
using Never.Domains;
using Never.Exceptions;
using Never.Security;
using B2C.Infrastructure;
using B2C.Admin.Web.Permissions.Commands;
using B2C.Admin.Web.Permissions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Domains
{
    public class EmployeeGroupAggregateRoot : _AggregateRoot<string>, IHandle<CreateEmployeeGroupEvent>,
        IHandle<RemoveEmployeeGroupEvent>
    {
        #region prop

        /// <summary>
        ///
        /// </summary>
        public string GroupId { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public string EmployeeId { get; protected set; }

        #endregion prop

        #region ctor

        private EmployeeGroupAggregateRoot()
            : this(string.Empty)
        {
        }

        private EmployeeGroupAggregateRoot(string uniqueId)
            : base(uniqueId)
        {
        }

        #endregion ctor

        #region register

        public static EmployeeGroupAggregateRoot Register(IWorkContext context, CreateEmployeeGroupCommand command)
        {
            return new EmployeeGroupAggregateRoot(command.AggregateId).Create(context, command);
        }

        public EmployeeGroupAggregateRoot Create(IWorkContext context, CreateEmployeeGroupCommand commmand)
        {
            this.ApplyEvent(new CreateEmployeeGroupEvent()
            {
                Creator = context.GetWorkerName(),
                AggregateId = this.AggregateId,
                Version = this.Version,
                CreateDate = context.WorkTime,
                EmployeeId = commmand.EmployeeId,
                GroupId = commmand.GroupId,
            });

            return this;
        }

        public void Handle(CreateEmployeeGroupEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.CreateDate = e.CreateDate;
            this.EditDate = e.CreateDate;
            this.Creator = e.Creator;
            this.Editor = e.Creator;

            this.EmployeeId = e.EmployeeId;
            this.GroupId = e.GroupId;
        }

        public void Remove(IWorkContext context)
        {
            this.ApplyEvent(new RemoveEmployeeGroupEvent()
            {
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,
                AggregateId = this.AggregateId,

                EmployeeId = this.EmployeeId,
                GroupId = this.GroupId,

            });
        }

        public void Handle(RemoveEmployeeGroupEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.GroupId = e.GroupId;
            this.EmployeeId = e.EmployeeId;
        }

        #endregion add/remove resource
    }
}