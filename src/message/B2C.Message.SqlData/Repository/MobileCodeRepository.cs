using Never;
using Never.EasySql;
using Never.IoC;
using B2C.Message.Contract.EnumTypes;
using B2C.Message.Core.Domains;
using B2C.Message.Core.Repository;
using System;
using System.Collections;

namespace B2C.Message.SqlData.Repository
{
    [SingletonAutoInjecting("message", typeof(IMobileCodeRepository))]
    public class MobileCodeRepository : IMobileCodeRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public MobileCodeRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        public int Destroy(MobileCodeAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Update("updateMobileCodeUsed");
        }

        public MobileCodeAggregateRoot Rebuild(Guid aggregateId)
        {
            if (aggregateId.IsEmpty())
                return default(MobileCodeAggregateRoot);

            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId }).QueryForObject<MobileCodeAggregateRoot>("qryMobileCodeRoot");
        }

        public MobileCodeAggregateRoot Rebuild(long mobile, UsageType usageType, UsageStatus usageStatus = UsageStatus.未使用)
        {
            if (mobile<=0)
                return default(MobileCodeAggregateRoot);

            var para = new
            {
                Mobile = mobile,
                UsageType = usageType,
                UsageStatus = usageStatus,
            };

            return this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<MobileCodeAggregateRoot>("qryMobileCodeRoot");
        }

        public int Save(MobileCodeAggregateRoot root)
        {
            if (root == null)
                return 0;

            return this.daoBuilder.Build().ToEasyXmlDao(root).Insert<int>("insertMobileCodeRoot");
        }
    }
}