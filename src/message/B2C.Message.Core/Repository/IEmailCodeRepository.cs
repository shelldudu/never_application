using B2C.Message.Contract.EnumTypes;
using B2C.Message.Core.Domains;
using System;

namespace B2C.Message.Core.Repository
{
    /// <summary>
    /// 邮箱验证码仓库
    /// </summary>
    public interface IEmailCodeRepository
    {
        /// <summary>
        /// 重建聚合根
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <returns></returns>
        EmailCodeAggregateRoot Rebuild(Guid aggregateId);

        /// <summary>
        /// 重建聚合根
        /// </summary>
        /// <param name="email"></param>
        /// <param name="usageType"></param>
        /// <returns></returns>
        EmailCodeAggregateRoot Rebuild(string email, UsageType usageType, UsageStatus usageStatus = UsageStatus.未使用);

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        int Save(EmailCodeAggregateRoot root);

        /// <summary>
        /// 销毁验证码
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        int Destroy(EmailCodeAggregateRoot root);
    }
}