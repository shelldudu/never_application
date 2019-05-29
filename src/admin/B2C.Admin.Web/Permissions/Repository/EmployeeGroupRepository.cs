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
    [SingletonAutoInjecting("admin", typeof(EmployeeGroupRepository))]
    public class EmployeeGroupRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public EmployeeGroupRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        /// <summary>
        /// 对部门下的小组里面员工分配权限
        /// </summary>
        /// <param name="roots"></param>
        public void Save(IEnumerable<EmployeeGroupAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return;

            foreach (var split in roots.Split(20))
                this.daoBuilder.Build().ToEasyXmlDao(split).Insert("batchInsertEmployeeGroupRoot");
        }

        /// <summary>
        /// 对部门下的小组分配权限
        /// </summary>
        /// <param name="groupId"></param>
        public IEnumerable<EmployeeGroupAggregateRoot> RebuildRootUsingGroupId(string groupId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { GroupId = groupId }).QueryForEnumerable<EmployeeGroupAggregateRoot>("qryEmployeeGroupRoot");
        }

        /// <summary>
        /// 对某个人的小组分配权限
        /// </summary>
        public IEnumerable<EmployeeGroupAggregateRoot> RebuildRootUsingEmployeeId(string employeeId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { EmployeeId = employeeId }).QueryForEnumerable<EmployeeGroupAggregateRoot>("qryEmployeeGroupRoot");
        }

        /// <summary>
        /// 删除部门下的小组里面员工分配权限
        /// </summary>
        /// <param name="roots"></param>
        public int Remove(IEnumerable<EmployeeGroupAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(split).Delete("batchDeleteEmployeeGroupRoot");

            return count;
        }
    }
}