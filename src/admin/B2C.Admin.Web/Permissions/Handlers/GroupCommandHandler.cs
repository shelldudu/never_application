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
    public class GroupCommandHandler : ICommandHandler<CreateGroupCommand>, ICommandHandler<ChangeGroupInfoCommand>, ICommandHandler<RemoveGroupCommand>
    {
        #region field

        private readonly DepartmentRepository departmentRepository = null;
        private readonly GroupRepository groupRepository = null;
        private readonly EmployeeGroupRepository employeeGroupRepository = null;
        private readonly GroupResourceRepository groupResourceRepository = null;

        #endregion field

        #region ctor

        public GroupCommandHandler(DepartmentRepository departmentRepository,
            GroupRepository groupRepository,
            EmployeeGroupRepository employeeGroupRepository,
            GroupResourceRepository groupResourceRepository)
        {
            this.departmentRepository = departmentRepository;
            this.groupRepository = groupRepository;
            this.employeeGroupRepository = employeeGroupRepository;
            this.groupResourceRepository = groupResourceRepository;
        }

        #endregion ctor

        public ICommandHandlerResult Execute(ICommandContext context, CreateGroupCommand command)
        {
            var department = this.departmentRepository.Rebuild(command.DepartmentId);
            if (department == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            var root = context.GetAggregateRoot(command.AggregateId, () => GroupAggregateRoot.Register(context, command, department));
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.Fail);

            if (groupRepository.Save(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, ChangeGroupInfoCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => groupRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.ChangeInfo(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            if (groupRepository.Change(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, RemoveGroupCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => groupRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.Remove(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            //删除组的权限
            var groupResources = this.groupResourceRepository.RebuildRootUsingGroupId(command.AggregateId);
            if (groupResources != null && groupResources.Any())
            {
                var deletes = new List<GroupResourceAggregateRoot>(groupResources.Count());
                foreach (var r in groupResources)
                {
                    r.Remove(context);
                    if (r.CanCommit())
                        deletes.Add(r);
                }

                if (deletes.Any())
                    this.groupResourceRepository.Remove(deletes);
            }

            //删除组的员工
            var groupEmployees = this.employeeGroupRepository.RebuildRootUsingGroupId(command.AggregateId);
            if (groupResources != null && groupResources.Any())
            {
                var deletes = new List<EmployeeGroupAggregateRoot>(groupEmployees.Count());
                foreach (var r in groupEmployees)
                {
                    r.Remove(context);
                    if (r.CanCommit())
                        deletes.Add(r);
                }

                if (deletes.Any())
                    this.employeeGroupRepository.Remove(deletes);
            }

            if (groupRepository.Remove(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }
    }
}