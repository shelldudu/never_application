using Never.EasySql;
using Never.IoC;
using B2C.Admin.Web.Permissions.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Repository
{
    [SingletonAutoInjecting("admin", typeof(DepartmentRepository))]
    public class DepartmentRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public DepartmentRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        /// <summary>
        /// 重建部门
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <returns></returns>
        public DepartmentAggregateRoot Rebuild(string aggregateId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId }).QueryForObject<DepartmentAggregateRoot>("qryDepartmentRoot");
        }

        /// <summary>
        /// 插入部门
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Save(DepartmentAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Insert<int>("insertDepartmentRoot");
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Change(DepartmentAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Update("updateDepartmentRoot");
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Remove(DepartmentAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Delete("deleteDepartmentRoot");
        }
    }
}