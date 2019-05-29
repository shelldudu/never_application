using Never;
using Never.Aop;
using Never.Commands;
using B2C.Admin.Web.Models;
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
    public class ResourceCommandHandler : ICommandHandler<InitResourceCommand>
    {
        #region field

        private readonly ResourceRepository resourceRepository = null;
        private readonly GroupResourceRepository groupResourceRepository = null;
        private readonly DepartmentResourceRepository departmentResourceRepository = null;

        #endregion field

        #region ctor

        /// <summary>
        ///
        /// </summary>
        /// <param name="repository"></param>
        public ResourceCommandHandler(ResourceRepository resourceRepository,
             GroupResourceRepository groupResourceRepository,
             DepartmentResourceRepository departmentResourceRepository)
        {
            this.resourceRepository = resourceRepository;
            this.groupResourceRepository = groupResourceRepository;
            this.departmentResourceRepository = departmentResourceRepository;
        }

        #endregion ctor

        /// <summary>
        /// 因为是启动的事件，而且资源修改与发布事件，在这个系统内通过事件总线发布没多大意义（但在webservice这些设定中就有意义了）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="communication"></param>
        public ICommandHandlerResult Execute(ICommandContext context, InitResourceCommand command)
        {
            if (command.Resources == null)
                return context.CreateResult(CommandHandlerStatus.NothingChanged);

            var list = resourceRepository.RebuildRoots();

            if (list.IsNullOrEmpty())
            {
                //第一次初始化，实际上要不要将部门与组的权限一起删除呢？因为有可能是在运营过程中删除的
                var roots = new List<ResourceAggregateRoot>(command.Resources.Count());
                foreach (var r in command.Resources)
                {
                    var cmd = new CreateResourceCommand(r.AggregateId)
                    {
                        Resource = new ActionResourceDescriptor()
                        {
                            ActionDescn = r.ActionDescn,
                            GroupSort = r.GroupSort,
                            AggregateId = r.AggregateId
                        }
                    };

                    var root = context.GetAggregateRoot(cmd.AggregateId, () => ResourceAggregateRoot.Register(context, cmd));
                    if (root.CanCommit())
                        roots.Add(root);
                }

                //保存权限
                this.resourceRepository.Save(roots);

                return context.CreateResult(CommandHandlerStatus.Success);
            }

            //表明这些是新增的，在新加的记录中删除已经存在的
            var news = command.Resources.ToList();
            news.RemoveAll(n => list.Any(o => o.AggregateId == n.AggregateId));

            //表明这些是已经存在，在已经存在的记录中删除不是新的记录中
            var deletes = list.ToList();
            deletes.RemoveAll(n => command.Resources.Any(o => o.AggregateId == n.AggregateId));

            //原来那些已经存在的
            var exists = list.ToList().FindAll(n => command.Resources.Any(o => o.AggregateId == n.AggregateId));

            if (deletes != null && deletes.Count > 0)
            {
                var targets = new List<ResourceAggregateRoot>(deletes.Count);
                foreach (var r in deletes)
                {
                    r.Remove(context);
                    if (r.CanCommit())
                        targets.Add(r);
                }

                this.resourceRepository.Delete(targets);
                this.groupResourceRepository.RemoveNotExistsGroupResource(targets);
                this.departmentResourceRepository.RemoveNotExistsDepartmentResource(targets);
            }

            if (news != null && news.Count > 0)
            {
                var targets = new List<ResourceAggregateRoot>(news.Count);
                foreach (var r in news)
                {
                    var cmd = new CreateResourceCommand(r.AggregateId) { Resource = new ActionResourceDescriptor() { AggregateId = r.AggregateId, ActionDescn = r.ActionDescn, GroupSort = r.GroupSort } };
                    var root = ResourceAggregateRoot.Register(context, cmd);
                    if (root.CanCommit())
                        targets.Add(root);
                }

                this.resourceRepository.Save(targets);
            }

            if (exists != null && exists.Count > 0)
            {
                var targets = new List<ResourceAggregateRoot>(exists.Count);
                foreach (var r in exists)
                {
                    var res = command.Resources.FirstOrDefault(ta => ta.AggregateId == r.AggregateId);
                    r.ChangeInfo(context, res.ActionDescn, res.GroupSort);
                    if (r.CanCommit())
                        targets.Add(r);
                }

                if(targets.Any())
                    resourceRepository.Change(targets);
            }

            return context.CreateResult(CommandHandlerStatus.Success);
        }
    }
}