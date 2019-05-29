using Never;
using Never.EasySql;
using Never.IoC;
using B2C.Message.Models;
using B2C.Message.Query;
using System;
using System.Collections.Generic;

namespace B2C.Message.SqlData.Query
{
    [SingletonAutoInjecting("message",typeof(IEmailCodeQuery))]
    public class EmailCodeQuery : IEmailCodeQuery
    {
        private readonly IDaoBuilder daoBuilder = null;
        public EmailCodeQuery(QueryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        public IEnumerable<EmailCodeInfo> GetList(string email, int topCount = 10)
        {
            var para = new
            {
                Email = email,
                StartIndex = 0,
                EndIndex = topCount,
                PageSize = topCount,
                PageNow = 1,
            };

            return this.daoBuilder.Build().ToEasyXmlDao(para).QueryForEnumerable<EmailCodeInfo>("qry_EmailCodeInfo");
        }

        public EmailCodeInfo Max(string email)
        {
            var para = new
            {
                Email = email,
                StartIndex = 0,
                EndIndex = 1,
                PageSize = 1,
                PageNow = 1,
            };

            return this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<EmailCodeInfo>("qry_EmailCodeInfo");
        }
    }
}
