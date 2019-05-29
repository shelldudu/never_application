using Never;
using Never.EasySql;
using Never.IoC;
using B2C.Message.Models;
using B2C.Message.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Message.SqlData.Query
{
    [SingletonAutoInjecting("message", typeof(IMobileCodeQuery))]
    public class MobileCodeQuery : IMobileCodeQuery
    {
        private readonly IDaoBuilder daoBuilder = null;
        public MobileCodeQuery(QueryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        public int Count(string ip, DateTime beginTime, DateTime endTime)
        {
            var para = new
            {
                ClientIP = ip,
                BeginTime = beginTime,
                EndTime = endTime,
            };

            return this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<int>("qryMobileCodeInfoCount");
        }

        public IEnumerable<MobileCodeInfo> GetList(long mobile, int topCount = 10)
        {
            var para = new
            {
                Mobile = mobile,
                StartIndex = 0,
                EndIndex = topCount,
                PageSize = topCount,
                PageNow = 1,
            };

            return this.daoBuilder.Build().ToEasyXmlDao(para).QueryForEnumerable<MobileCodeInfo>("qry_MobileCodeInfo");

        }

        public MobileCodeInfo Max(long mobile)
        {
            var para = new
            {
                Mobile = mobile,
                StartIndex = 0,
                EndIndex = 1,
                PageSize = 1,
                PageNow = 1,
            };

            return this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<MobileCodeInfo>("qry_MobileCodeInfo");
        }
    }
}
