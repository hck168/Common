using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 派生类聚合根的封装都必须继承此抽象类
    /// </summary>
    [Serializable]
    public abstract class AggregateRoot : IAggregateRoot
    {
        #region 相关属性

        /// <summary>
        /// 获取当前聚合根Id
        /// </summary>
        public Guid AggregateId { get; private set; }
        /// <summary>
        /// 获取当前聚合根版本号
        /// </summary>
        public int Version { get; private set; }
        /// <summary>
        /// 获取当前聚合根事件历史
        /// </summary>
        [NonSerialized]
        private ConcurrentQueue<IEvent> Changes = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 实例化 AggregateRoot 对象
        /// </summary>
        protected AggregateRoot(Guid aggregateId, int version)
        {
            this.AggregateId = aggregateId;
            this.Version = version;

            this.Changes = new ConcurrentQueue<IEvent>();
        }

        #endregion

        #region 相关方法

        /// <summary>
        /// 获取当前聚合根事件历史
        /// </summary>
        /// <returns></returns>
        public ConcurrentQueue<IEvent> GetChanges()
        {
            if (this.Changes == null)
            {
                this.Changes = new ConcurrentQueue<IEvent>();
            }

            return this.Changes;
        }

        /// <summary>
        /// 设置聚合根版本号
        /// </summary>
        /// <param name="version"></param>
        public void SetVersion(int version)
        {
            if (this.Version + 1 != version)
            {
                throw new Exception(string.Format("非法的版本号：{0}，期待的版本号：{1}", version, this.Version + 1));
            }

            this.Version = version;
        }

        /// <summary>
        /// 装载聚合根事件历史
        /// </summary>
        /// <param name="historys">聚合根事件历史集合</param>
        public void LoadFromHistory(IEnumerable<IEvent> historys)
        {
            foreach (var handle in historys)
            {
                ApplyEvent(handle, false);
            }

            if (historys.Any())
            {
                this.AggregateId = historys.Last().AggregateId;
                this.Version = historys.Last().Version;
            }
        }

        /// <summary>
        /// 应用事件历史变更
        /// </summary>
        /// <param name="handle">要处理的事件</param>
        /// <param name="isNew">是否更新到事件集合</param>
        protected void ApplyEvent(IEvent handle, bool isNew = true)
        {
            dynamic d = this;

            d.Apply(handle as dynamic);  //利用 dynamic 让参数自动找到相应的 Apply 方法执行

            if (isNew)
            {
                if (this.Changes == null)
                {
                    this.Changes = new ConcurrentQueue<IEvent>();
                }

                this.Changes.Enqueue(handle);
            }
        }

        #endregion
    }
}
