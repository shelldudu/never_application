using Never;
using Never.DataAnnotations;
using Never.Domains;
using Never.Exceptions;
using Never.Security;
using Never.Utils;
using B2C.Infrastructure;
using B2C.Message.Contract.Commands;
using B2C.Message.Contract.EnumTypes;
using B2C.Message.Contract.Events;
using System;

namespace B2C.Message.Core.Domains
{
    /// <summary>
    /// 手机验证码聚合根
    /// </summary>
    public class MobileCodeAggregateRoot : _AggregateRoot<Guid>, IHandle<CreateMobileCodeEvent>,
        IHandle<DestroyMobileCodeEvent>
    {
        #region prop

        /// <summary>
        /// 手机号码
        /// </summary>
        public long Mobile { get; protected set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public UsageType UsageType { get; protected set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string UsageCode { get; protected set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; protected set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public UsageStatus UsageStatus { get; protected set; }

        /// <summary>
        /// 生成客户端IP
        /// </summary>
        public string ClientIP { get; protected set; }

        /// <summary>
        /// 客户端平台
        /// </summary>
        public OperatePlatform Platform { get; protected set; }

        #endregion prop

        #region ctor

        private MobileCodeAggregateRoot()
            : this(Guid.Empty)
        {
        }

        private MobileCodeAggregateRoot(Guid uniqueId)
            : base(uniqueId)
        {
            this.UsageStatus = UsageStatus.未使用;
            this.UsageType = UsageType.注册;
        }

        #endregion ctor

        #region Register

        public static MobileCodeAggregateRoot Register(IWorkContext context, CreateMobileCodeCommand command)
        {
            return new MobileCodeAggregateRoot(command.AggregateId).Create(context, command);
        }

        protected MobileCodeAggregateRoot Create(IWorkContext context, CreateMobileCodeCommand command)
        {
            var length = command.Length;
            if (length <= 0)
                length = 4;
            else if (length > 10)
                length = 10;

            this.ApplyEvent(new CreateMobileCodeEvent()
            {
                AggregateId = this.AggregateId,
                Version = this.Version,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),

                Mobile = command.Mobile.AsLong(),
                UsageType = command.UsageType,
                Message = string.Join("", Randomizer.PokerArray(10, length)),
                ClientIP = command.ClientIP,
                Platform = command.Platform,
                ExpireTime = this.CreateDate.AddMinutes(5),
            });

            return this;
        }

        public void Handle(CreateMobileCodeEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.CreateDate = e.CreateDate;
            this.Creator = e.Creator;
            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;

            this.Mobile = e.Mobile;
            this.UsageType = e.UsageType;
            this.UsageCode = e.Message;

            //TODO 发布的时候要删除
            //this.UsageCode = "2233";
            this.ClientIP = e.ClientIP;
            this.ExpireTime = e.ExpireTime;
            this.Platform = e.Platform;
        }

        #endregion Register

        #region Destroy

        public ValidationFailure? Destroy(IWorkContext context, DestroyMobileCodeCommand command)
        {
            if (this.UsageStatus == UsageStatus.已使用)
                return this.RejectBreak(this,m => m.ExpireTime, "验证码已使用");

            if (this.ExpireTime < context.WorkTime)
                return this.RejectBreak(this,m => m.ExpireTime, "验证码已过期");

            if (this.UsageType != command.UsageType)
                return this.RejectBreak(this,m => m.UsageType, "验证码不正确");

            if (this.UsageCode.IsNotEquals(command.VCode))
                return this.RejectBreak(this,m => m.UsageCode, "验证码错误");

            this.ApplyEvent(new DestroyMobileCodeEvent()
            {
                AggregateId = this.AggregateId,
                Version = this.Version,
                CreateDate = context.WorkTime,
                Creator = context.GetWorkerName(),
                Mobile = this.Mobile,
                UsageStatus = UsageStatus.已使用,
                UsageType = this.UsageType,
                Message = this.UsageCode,
            });

            return null;
        }

        public void Handle(DestroyMobileCodeEvent e)
        {
            if (!this.IsContextCall)
                return;

            this.EditDate = e.CreateDate;
            this.Editor = e.Creator;

            this.UsageStatus = e.UsageStatus;
        }

        #endregion Destroy
    }
}