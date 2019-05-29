using Never.EasySql;
using Never.IoC;
using B2C.Admin.Web.Permissions.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions.Repository
{
    [SingletonAutoInjecting("admin", typeof(EmployeeRepository))]
    public class EmployeeRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public EmployeeRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        /// <summary>
        /// 重建员工
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <returns></returns>
        public EmployeeAggregateRoot Rebuild(string aggregateId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId }).QueryForObject<EmployeeAggregateRoot>("qryEmployeeRoot");
        }

        /// <summary>
        /// 插入员工
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Save(EmployeeAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Insert<int>("insertEmployeeRoot");
        }

        /// <summary>
        /// 更新员工
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Change(EmployeeAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Update("updateEmployeeRoot");
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Remove(EmployeeAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Delete("deleteEmployeeRoot");
        }
    }
}