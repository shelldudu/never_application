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
    [SingletonAutoInjecting("admin", typeof(GroupRepository))]
    public class GroupRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public GroupRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        /// <summary>
        /// 重建小组
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <returns></returns>
        public GroupAggregateRoot Rebuild(string aggregateId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId }).QueryForObject<GroupAggregateRoot>("qryGroupRoot");
        }

        /// <summary>
        /// 获取部门的所有组
        /// </summary>
        /// <param name="roots"></param>
        public IEnumerable<GroupAggregateRoot> RebuildRoots(string departmentId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { DepartmentId = departmentId }).QueryForEnumerable<GroupAggregateRoot>("qryGroupRoot");
        }

        /// <summary>
        /// 创建小组
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Save(GroupAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Insert<int>("insertGroupRoot");
        }

        /// <summary>
        /// 更新小组
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Change(GroupAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Update("updateGroupRoot");
        }

        /// <summary>
        /// 删除小组
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Remove(GroupAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Delete("deleteGroupRoot");
        }

        /// <summary>
        /// 删除小组
        /// </summary>
        /// <param name="roots"></param>
        /// <returns></returns>
        public int Remove(IEnumerable<GroupAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(split).Delete("batchDeleteGroupRoot");

            return count;
        }
    }
}