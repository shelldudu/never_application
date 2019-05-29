using Never;
using Never.EasySql;
using Never.IoC;
using B2C.Message.Contract.EnumTypes;
using B2C.Message.Core.Domains;
using B2C.Message.Core.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Message.SqlData.Repository
{
    [SingletonAutoInjecting("message", typeof(IEmailCodeRepository))]
    public class EmailCodeRepository : IEmailCodeRepository
    {
        private readonly IDaoBuilder daoBuilder = null;
        public EmailCodeRepository(RepositoryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        public int Destroy(EmailCodeAggregateRoot root)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(root).Update("updateEmailCodeUsed");
        }

        public EmailCodeAggregateRoot Rebuild(Guid aggregateId)
        {
            if (aggregateId.IsEmpty())
                return default(EmailCodeAggregateRoot);

            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId }).QueryForObject<EmailCodeAggregateRoot>("qryEmailCodeRoot");
        }

        public EmailCodeAggregateRoot Rebuild(string email, UsageType usageType, UsageStatus usageStatus = UsageStatus.未使用)
        {
            if (email.IsNullOrEmpty())
                return default(EmailCodeAggregateRoot);

            var para = new
            {
                Email = email,
                UsageType = usageType,
                UsageStatus = usageStatus,
            };

            return this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<EmailCodeAggregateRoot>("qryEmailCodeRoot");
        }

        public int Save(EmailCodeAggregateRoot root)
        {
            if (root == null)
                return 0;

            return this.daoBuilder.Build().ToEasyXmlDao(root).Insert<int>("insertEmailCodeRoot");
        }
    }
}