using Never;
using Never.EasySql;
using Never.IoC;
using B2C.Admin.Web.Permissions.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Repository
{
    [SingletonAutoInjecting("admin", typeof(GroupResourceRepository))]
    public class GroupResourceRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public GroupResourceRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        /// <summary>
        /// 对部门下的小组分配权限
        /// </summary>
        /// <param name="groupId"></param>
        public IEnumerable<GroupResourceAggregateRoot> RebuildRootUsingGroupId(string groupId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { GroupId = groupId }).QueryForEnumerable<GroupResourceAggregateRoot>("qryGroupResourceRoot");
        }

        /// <summary>
        /// 对部门下的小组分配权限
        /// </summary>
        /// <param name="roots"></param>
        public void Save(IEnumerable<GroupResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return;

            foreach (var split in roots.Split(20))
                this.daoBuilder.Build().ToEasyXmlDao(split).Insert("batchInsertGroupResourceRoot");
        }

        /// <summary>
        /// 删除部门下的小组分配权限
        /// </summary>
        /// <param name="roots"></param>
        public int Remove(IEnumerable<GroupResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = split.Select(t => t.AggregateId) }).Delete("batchDeleteGroupResourceRoot");

            return count;
        }

        public int RemoveNotExistsGroupResource(IEnumerable<ResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(new { ResourceId = split.Select(t => t.AggregateId) }).Delete("batchDeleteGroupResourceRoot");

            return count;
        }
    }
}