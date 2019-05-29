using Never.Aop;
using Never.Commands;
using Never.Events;
using Never.Exceptions;
using B2C.Admin.Web.Permissions.Commands;
using B2C.Admin.Web.Permissions.Repository;
using B2C.Admin.Web.Permissions.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2C.Admin.Web.Permissions.Handlers
{
    [Logger]
    public class EmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand>,
        ICommandHandler<ChangeEmployeeNickNameCommand>, ICommandHandler<ChangeEmployeeOwnerCommand>,
        ICommandHandler<ChangeEmployeePasswordCommand>, ICommandHandler<RemoveEmployeeCommand>
    {
        #region field

        private readonly EmployeeRepository employeeRepository = null;
        private readonly DepartmentRepository departmentRepository = null;

        #endregion field

        #region ctor

        public EmployeeCommandHandler(EmployeeRepository employeeRepository,
            DepartmentRepository departmentRepository)
        {
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;
        }

        #endregion ctor

        public ICommandHandlerResult Execute(ICommandContext context, CreateEmployeeCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => EmployeeAggregateRoot.Register(context, command));
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.Fail);

            var rowId = employeeRepository.Save(root);
            if (rowId <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, ChangeEmployeeOwnerCommand command)
        {
            var department = this.departmentRepository.Rebuild(command.DepartmentId);
            if (department == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            var root = context.GetAggregateRoot(command.AggregateId, () => employeeRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.ChangeOwner(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            if (employeeRepository.Change(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, ChangeEmployeeNickNameCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => employeeRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.ChangeNickName(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            if (employeeRepository.Change(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, ChangeEmployeePasswordCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => employeeRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.ChangePassword(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            if (employeeRepository.Change(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, RemoveEmployeeCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => employeeRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.Remove(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            if (employeeRepository.Remove(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }
    }
}