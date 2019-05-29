using Never;
using Never.Aop;
using Never.Commands;
using Never.Events;
using Never.Exceptions;
using Never.Utils;
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
    public class DepartmentResourceCommandHandler : ICommandHandler<DistributeDepartmentResourceCommand>
    {
        #region field

        private readonly DepartmentRepository departmentRepository = null;
        private readonly GroupRepository groupRepository = null;
        private readonly DepartmentResourceRepository departmentResourceRepository = null;

        #endregion field

        #region ctor

        public DepartmentResourceCommandHandler(DepartmentRepository departmentRepository,
            GroupRepository groupRepository,
            DepartmentResourceRepository departmentResourceRepository)
        {
            this.departmentRepository = departmentRepository;
            this.groupRepository = groupRepository;
            this.departmentResourceRepository = departmentResourceRepository;
        }

        #endregion ctor

        #region distribute

        public ICommandHandlerResult Execute(ICommandContext context, DistributeDepartmentResourceCommand command)
        {
            var department = this.departmentRepository.Rebuild(command.DepartmentId);
            if (department == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            var departmentResources = this.departmentResourceRepository.RebuildRootUsingDepartId(command.DepartmentId);
            if (departmentResources.IsNullOrEmpty())
            {
                var list = new List<DepartmentResourceAggregateRoot>(100);
                foreach (var r in command.ResourceId)
                {
                    var cmd = new CreateDepartmentResourceCommand(NewId.GenerateNumber(NewId.StringLength.L24))
                    {
                        DepartmentId = command.DepartmentId,
                        ResourceId = r,
                    };

                    var root = context.GetAggregateRoot(cmd.AggregateId, () => DepartmentResourceAggregateRoot.Register(context, cmd));
                    if (root.CanCommit())
                        list.Add(root);
                }

                this.departmentResourceRepository.Save(list);
                return context.CreateResult(CommandHandlerStatus.Success);
            }

            //表明这些是新增的，在新加的记录中删除已经存在的
            var news = new List<DepartmentResourceAggregateRoot>(100);
            foreach (var r in command.ResourceId)
            {
                if (departmentResources.Any(t => t.ResourceId == r))
                    continue;

                var cmd = new CreateDepartmentResourceCommand(NewId.GenerateNumber(NewId.StringLength.L24))
                {
                    DepartmentId = command.DepartmentId,
                    ResourceId = r,
                };

                var root = context.GetAggregateRoot(cmd.AggregateId, () => DepartmentResourceAggregateRoot.Register(context, cmd));
                if (root.CanCommit())
                    news.Add(root);
            }

            if (news.Any())
                this.departmentResourceRepository.Save(news);

            //表明这些是已经存在，在已经存在的记录中删除不是新的记录中
            var deletes = new List<DepartmentResourceAggregateRoot>(100);
            foreach (var r in departmentResources)
            {
                if (command.ResourceId.Any(t => t == r.ResourceId))
                    continue;

                r.Remove(context);
                if (r.CanCommit())
                    deletes.Add(r);
            }

            if (deletes.Any())
                this.departmentResourceRepository.Remove(deletes);

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        #endregion add
    }
}