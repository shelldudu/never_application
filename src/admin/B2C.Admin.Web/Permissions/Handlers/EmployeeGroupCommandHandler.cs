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
    public class EmployeeGroupCommandHandler : ICommandHandler<DistributeEmployeeGroupCommand>
    {
        #region field

        private readonly EmployeeRepository employeeRepository = null;
        private readonly GroupRepository groupRepository = null;
        private readonly EmployeeGroupRepository employeeGroupRepository = null;

        #endregion field

        #region ctor

        public EmployeeGroupCommandHandler(EmployeeRepository employeeRepository,
            GroupRepository groupRepository,
            EmployeeGroupRepository employeeGroupRepository)
        {
            this.employeeRepository = employeeRepository;
            this.groupRepository = groupRepository;
            this.employeeGroupRepository = employeeGroupRepository;
        }

        #endregion ctor

        #region execute

        public ICommandHandlerResult Execute(ICommandContext context, DistributeEmployeeGroupCommand command)
        {
            var employee = this.employeeRepository.Rebuild(command.EmployeeId);
            if (employee == null)
                return context.CreateResult(CommandHandlerStatus.NotExists);

            var employeeGroups = this.employeeGroupRepository.RebuildRootUsingEmployeeId(command.EmployeeId);
            if (employeeGroups.IsNullOrEmpty())
            {
                var list = new List<EmployeeGroupAggregateRoot>(100);
                foreach (var r in command.GroupId)
                {
                    var cmd = new CreateEmployeeGroupCommand(NewId.GenerateNumber(NewId.StringLength.L24))
                    {
                        EmployeeId = command.EmployeeId,
                        GroupId = r,
                    };

                    var root = context.GetAggregateRoot(cmd.AggregateId, () => EmployeeGroupAggregateRoot.Register(context, cmd));
                    if (root.CanCommit())
                        list.Add(root);
                }

                this.employeeGroupRepository.Save(list);
                return context.CreateResult(CommandHandlerStatus.Success);
            }

            //表明这些是新增的，在新加的记录中删除已经存在的
            var news = new List<EmployeeGroupAggregateRoot>(100);
            foreach (var r in command.GroupId)
            {
                if (employeeGroups.Any(t => t.GroupId == r))
                    continue;

                var cmd = new CreateEmployeeGroupCommand(NewId.GenerateNumber(NewId.StringLength.L24))
                {
                    EmployeeId = command.EmployeeId,
                    GroupId = r,
                };

                var root = context.GetAggregateRoot(cmd.AggregateId, () => EmployeeGroupAggregateRoot.Register(context, cmd));
                if (root.CanCommit())
                    news.Add(root);
            }

            if (news.Any())
                this.employeeGroupRepository.Save(news);

            //表明这些是已经存在，在已经存在的记录中删除不是新的记录中
            var deletes = new List<EmployeeGroupAggregateRoot>(100);
            foreach (var r in employeeGroups)
            {
                if (command.GroupId.Any(t => t == r.GroupId))
                    continue;

                r.Remove(context);
                if (r.CanCommit())
                    deletes.Add(r);
            }
            if (deletes.Any())
                this.employeeGroupRepository.Remove(deletes);

            return context.CreateResult(CommandHandlerStatus.Success);
        }
 
        #endregion execute
    }
}