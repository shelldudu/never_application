using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace B2C.Infrastructure
{
    /// <summary>
    /// 聚合对象
    /// </summary>
    public abstract class _AggregateModel<T>
    {
        #region prop

        /// <summary>
        /// 任务Id
        /// </summary>
        [DisplayName("任务Id")]
        public virtual int Id { get; set; }

        /// <summary>
        /// 聚合Id
        /// </summary>
        [DisplayName("聚合Id")]
        public virtual T AggregateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [DisplayName("创建者")]
        public virtual string Creator { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        [DisplayName("编辑时间")]
        public virtual DateTime EditDate { get; set; }

        /// <summary>
        /// 编辑者
        /// </summary>
        [DisplayName("编辑者")]
        public virtual string Editor { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [DisplayName("版本号")]
        public virtual int Version { get; set; }
        #endregion
    }
}
