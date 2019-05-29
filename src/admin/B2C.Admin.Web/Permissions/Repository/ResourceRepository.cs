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
    [SingletonAutoInjecting("admin", typeof(ResourceRepository))]
    public class ResourceRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public ResourceRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        public IEnumerable<ResourceAggregateRoot> RebuildRoots()
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { }).QueryForEnumerable<ResourceAggregateRoot>("qryResourceRoot");
        }

        public ResourceAggregateRoot Rebuild(string aggregateId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId }).QueryForObject<ResourceAggregateRoot>("qryResourceRoot");
        }

        public void Save(IEnumerable<ResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return;

            foreach (var split in roots.Split(20))
                this.daoBuilder.Build().ToEasyXmlDao(split).Insert("batchInsertResourceRoot");
        }


        public int Change(IEnumerable<ResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(split).Update("batchUpdateResourceRoot");

            return count;
        }


        public int Delete(IEnumerable<ResourceAggregateRoot> roots)
        {
            if (roots.IsNullOrEmpty())
                return 0;

            var count = 0;
            foreach (var split in roots.Split(20))
                count += this.daoBuilder.Build().ToEasyXmlDao(split.Select(t => new { AggregateId = t.AggregateId })).Delete("batchDeleteResourceRoot");

            return count;
        }
    }
}