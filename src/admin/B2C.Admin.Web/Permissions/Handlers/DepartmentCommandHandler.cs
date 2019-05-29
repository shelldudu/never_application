using Never.Commands;
using Never.Exceptions;
using B2C.Admin.Web.Permissions.Commands;
using B2C.Admin.Web.Permissions.Repository;
using B2C.Admin.Web.Permissions.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Handlers
{
    public class DepartmentCommandHandler : ICommandHandler<CreateDepartmentCommand>,
        ICommandHandler<ChangeDepartmentInfoCommand>, ICommandHandler<RemoveDepartmentCommand>
    {
        #region field

        private readonly DepartmentRepository departmentRepository = null;
        private readonly GroupRepository groupRepository = null;
        private readonly DepartmentResourceRepository departmentResourceRepository = null;

        #endregion field

        #region ctor

        public DepartmentCommandHandler(DepartmentRepository departmentRepository,
            GroupRepository groupRepository,
            DepartmentResourceRepository departmentResourceRepository)
        {
            this.departmentRepository = departmentRepository;
            this.groupRepository = groupRepository;
            this.departmentResourceRepository = departmentResourceRepository;
        }

        #endregion ctor

        public ICommandHandlerResult Execute(ICommandContext context, CreateDepartmentCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => DepartmentAggregateRoot.Register(context, command));
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.Fail);

            var rowId = departmentRepository.Save(root);
            if (rowId <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, ChangeDepartmentInfoCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => departmentRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.ChangeInfo(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            if (departmentRepository.Change(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        public ICommandHandlerResult Execute(ICommandContext context, RemoveDepartmentCommand command)
        {
            var root = context.GetAggregateRoot(command.AggregateId, () => departmentRepository.Rebuild(command.AggregateId));
            if (root == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            root.Remove(context, command);
            if (root.CanNotCommit())
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            //删除部门的组
            var groups = this.groupRepository.RebuildRoots(command.AggregateId);
            if (groups != null && groups.Any())
            {
                var deletes = new List<GroupAggregateRoot>(groups.Count());
                foreach (var r in groups)
                {
                    r.Remove(context, null);
                    if (r.CanCommit())
                        deletes.Add(r);
                }

                if (deletes.Any())
                    this.groupRepository.Remove(deletes);
            }

            //删除部门的权限
            var resources = this.departmentResourceRepository.RebuildRootUsingDepartId(command.AggregateId);
            if (resources != null && resources.Any())
            {
                var deletes = new List<DepartmentResourceAggregateRoot>(resources.Count());
                foreach (var r in resources)
                {
                    r.Remove(context);
                    if (r.CanCommit())
                        deletes.Add(r);
                }

                if (deletes.Any())
                    this.departmentResourceRepository.Remove(deletes);
            }

            if (departmentRepository.Remove(root) <= 0)
                throw new RepositoryExcutingException("执行失败,请稍后再试");

            return context.CreateResult(CommandHandlerStatus.Success);
        }
    }
}