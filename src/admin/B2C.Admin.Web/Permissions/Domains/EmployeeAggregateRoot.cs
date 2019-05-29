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
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class EmployeeAggregateRoot : _AggregateRoot<string>, IHandle<CreateEmployeeEvent>,
        IHandle<ChangeEmployeePasswordEvent>, IHandle<ChangeEmployeeOwnerEvent>, IHandle<RemoveEmployeeEvent>, IHandle<ChangeEmployeeNickNameEvent>
    {
        #region prop

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string UserName { get; protected set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; protected set; }

        /// <summary>
        /// 用户名字
        /// </summary>
        public string NickName { get; protected set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; protected set; }

        /// <summary>
        /// 级的分类，分配的时候不能越级
        /// </summary>
        public GroupSort GroupSort { get; protected set; }

        #endregion prop

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        private EmployeeAggregateRoot()
            : this(string.Empty)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uniqueId"></param>
        private EmployeeAggregateRoot(string uniqueId) : base(uniqueId)
        {
        }

        #endregion ctor

        #region register

        public static EmployeeAggregateRoot Register(IWorkContext context, CreateEmployeeCommand command)
        {
            var user = new EmployeeAggregateRoot(command.AggregateId);
            user.Create(context, command);
            return user;
        }

        protected void Create(IWorkContext context, CreateEmployeeCommand command)
        {
            this.ApplyEvent(new CreateEmployeeEvent()
            {
                AggregateId = this.AggregateId,
                UserName = command.UserName,
                NickName = command.NickName,
                Version = this.Version,
                CreateDate = context.WorkTime,
                Password = command.Password,
                Creator = context.GetWorkerName(),
                DepartmentId = command.DepartmentId,
                GroupSort = command.GroupSort
            });
        }

        public void Handle(CreateEmployeeEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.CreateDate = e.CreateDate;
            this.EditDate = e.CreateDate;
            this.Creator = e.Creator;
            this.Editor = e.Creator;

            this.GroupSort = e.GroupSort;
            this.UserName = e.UserName;
            this.NickName = e.NickName;
            this.Password = e.Password;
            this.DepartmentId = e.DepartmentId;
        }

        public void ChangePassword(IWorkContext context, ChangeEmployeePasswordCommand command)
        {
            if (this.Password.IsEquals(command.Password))
                return;

            this.ApplyEvent(new ChangeEmployeePasswordEvent()
            {
                AggregateId = this.AggregateId,
                Creator = context.GetWorkerName(),
                CreateDate = context.WorkTime,
                Version = this.Version,

                Password = command.Password,
            });
        }

        public void Handle(ChangeEmployeePasswordEvent e)
        {
            this.Password = e.Password;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        public void ChangeNickName(IWorkContext context, ChangeEmployeeNickNameCommand command)
        {
            if (this.NickName.IsEquals(command.NickName))
                return;

            this.ApplyEvent(new ChangeEmployeeNickNameEvent()
            {
                AggregateId = this.AggregateId,
                Creator = context.GetWorkerName(),
                CreateDate = context.WorkTime,
                Version = this.Version,

                NickName = command.NickName,
            });
        }

        public void Handle(ChangeEmployeeNickNameEvent e)
        {
            this.NickName = e.NickName;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        public void ChangeOwner(IWorkContext context, ChangeEmployeeOwnerCommand command)
        {
            //if (command.GroupSort >= current.GroupSort)
            //    throw new DomainException("你当前权限不能分配更高级的权限给别人");

            if (command.GroupSort >= GroupSort.Super)
                throw new DomainException("你当前权限不能分配更高级的权限给别人");

            this.ApplyEvent(new ChangeEmployeeOwnerEvent()
            {
                AggregateId = this.AggregateId,
                Creator = context.GetWorkerName(),
                CreateDate = context.WorkTime,
                Version = this.Version,
                DepartmentId = command.DepartmentId,
                GroupSort = command.GroupSort,
            });
        }

        public void Handle(ChangeEmployeeOwnerEvent e)
        {
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;

            this.GroupSort = e.GroupSort;
            this.DepartmentId = e.DepartmentId;
        }

        #endregion register

        #region remove

        public void Remove(IWorkContext context, RemoveEmployeeCommand command)
        {
            this.ApplyEvent(new RemoveEmployeeEvent()
            {
                AggregateId = this.AggregateId,

                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Version = this.Version,
                UserName = this.UserName,
                DepartmentId = this.DepartmentId,
                GroupSort = this.GroupSort,
            });
        }

        public void Handle(RemoveEmployeeEvent e)
        {
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;
        }

        #endregion remove
    }
}