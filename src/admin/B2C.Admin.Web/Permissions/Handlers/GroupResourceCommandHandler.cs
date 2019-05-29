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
    public class GroupResourceCommandHandler : ICommandHandler<DistributeGroupResourceCommand>
    {
        #region field

        private readonly DepartmentRepository departmentRepository = null;
        private readonly GroupRepository groupRepository = null;
        private readonly GroupResourceRepository groupResourceRepository = null;

        #endregion field

        #region ctor

        public GroupResourceCommandHandler(DepartmentRepository departmentRepository,
            GroupRepository groupRepository,
            GroupResourceRepository groupResourceRepository)
        {
            this.departmentRepository = departmentRepository;
            this.groupRepository = groupRepository;
            this.groupResourceRepository = groupResourceRepository;
        }

        #endregion ctor

        #region execute

        public ICommandHandlerResult Execute(ICommandContext context, DistributeGroupResourceCommand command)
        {
            var group = this.groupRepository.Rebuild(command.GroupId);
            if (group == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            var department = this.departmentRepository.Rebuild(group.DepartmentId);
            if (department == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            var groupResources = this.groupResourceRepository.RebuildRootUsingGroupId(command.GroupId);
            if (groupResources.IsNullOrEmpty())
            {
                var list = new List<GroupResourceAggregateRoot>(100);
                foreach (var r in command.ResourceId)
                {
                    var cmd = new CreateGroupResourceCommand(NewId.GenerateNumber(NewId.StringLength.L24))
                    {
                        GroupId = command.GroupId,
                        ResourceId = r,
                    };

                    var root = context.GetAggregateRoot(cmd.AggregateId, () => GroupResourceAggregateRoot.Register(context, cmd, department));
                    if (root.CanCommit())
                        list.Add(root);
                }

                this.groupResourceRepository.Save(list);
                return context.CreateResult(CommandHandlerStatus.Success);
            }

            //表明这些是新增的，在新加的记录中删除已经存在的
            var news = new List<GroupResourceAggregateRoot>(100);
            foreach (var r in command.ResourceId)
            {
                if (groupResources.Any(t => t.ResourceId == r))
                    continue;

                var cmd = new CreateGroupResourceCommand(NewId.GenerateNumber(NewId.StringLength.L24))
                {
                    GroupId = command.GroupId,
                    ResourceId = r,
                };

                var root = context.GetAggregateRoot(cmd.AggregateId, () => GroupResourceAggregateRoot.Register(context, cmd, department));
                if (root.CanCommit())
                    news.Add(root);
            }

            if (news.Any())
                this.groupResourceRepository.Save(news);

            //表明这些是已经存在，在已经存在的记录中删除不是新的记录中
            var deletes = new List<GroupResourceAggregateRoot>(100);
            foreach (var r in groupResources)
            {
                if (command.ResourceId.Any(t => t == r.ResourceId))
                    continue;

                r.Remove(context);
                if (r.CanCommit())
                    deletes.Add(r);
            }
            if (deletes.Any())
                this.groupResourceRepository.Remove(deletes);

            return context.CreateResult(CommandHandlerStatus.Success);
        }

        #endregion execute
    }
}