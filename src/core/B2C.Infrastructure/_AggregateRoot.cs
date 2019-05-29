using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 聚合对象
    /// </summary>
    public abstract class _AggregateRoot<T> : Never.Domains.AggregateRoot<T>
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="_AggregateRoot{T}"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregateid.</param>
        protected _AggregateRoot(T aggregateId) : base(aggregateId)
        {
            this.CreateDate = this.EditDate = DateTime.Now;
            this.Creator = this.Editor = string.Empty;
        }
        #endregion

        #region prop
        /// <summary>
        /// 任务Id
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; protected set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public virtual string Creator { get; protected set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public virtual DateTime EditDate { get; protected set; }

        /// <summary>
        /// 编辑者
        /// </summary>
        public virtual string Editor { get; protected set; }
        #endregion
    }
}
