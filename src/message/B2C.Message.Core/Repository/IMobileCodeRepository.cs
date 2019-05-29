using B2C.Message.Contract.EnumTypes;
using B2C.Message.Core.Domains;
using System;

namespace B2C.Message.Core.Repository
{
    /// <summary>
    /// 手机验证码仓库
    /// </summary>
    public interface IMobileCodeRepository
    {
        /// <summary>
        /// 重建聚合根
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <returns></returns>
        MobileCodeAggregateRoot Rebuild(Guid aggregateId);

        /// <summary>
        /// 重建聚合根
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        MobileCodeAggregateRoot Rebuild(long mobile, UsageType usageType, UsageStatus UsageStatus = UsageStatus.未使用);

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        int Save(MobileCodeAggregateRoot root);

        /// <summary>
        /// 销毁验证码
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        int Destroy(MobileCodeAggregateRoot root);
    }
}