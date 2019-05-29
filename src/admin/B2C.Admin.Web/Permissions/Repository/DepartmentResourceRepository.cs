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
    [SingletonAutoInjecting("admin", typeof(DepartmentResourceRepository))]
    public class DepartmentResourceRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public DepartmentResourceRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        /// <summary>
        /// 对部门分配权限
        /// </summary>
        /// <param name="roots"></param>
        public IEnumerable<DepartmentResourceAggregateRoot> RebuildRootUsingDepartId(string departmentId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { DepartmentId = departmentId }).QueryForEnumerable<DepartmentResourceAggregateRoot>("qryDepartmentResourceRoot");
        }

        /// <summary>
        /// 对部门分配权限
        /// </summary>
        /// <param name="roots"></param>
        public void Save(IEnumerable<DepartmentResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return;

            foreach (var split in roots.Split(20))
                this.daoBuilder.Build().ToEasyXmlDao(split).Insert("batchInsertDepartmentResourceRoot");
        }

        /// <summary>
        /// 删除部门的权限
        /// </summary>
        /// <param name="roots"></param>
        public int Remove(IEnumerable<DepartmentResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = split.Select(t => t.AggregateId) }).Delete("batchDeleteDepartmentResourceRoot");

            return count;
        }

        public int RemoveNotExistsDepartmentResource(IEnumerable<ResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(new { ResourceId = split.Select(t => t.AggregateId) }).Delete("batchDeleteDepartmentResourceRoot");

            return count;
        }
    }
}