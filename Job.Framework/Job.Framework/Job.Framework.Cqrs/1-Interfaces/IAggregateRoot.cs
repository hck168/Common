using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 定义一个用于聚合根的事件版本接口
    /// </summary>
    public interface IAggregateRoot
    {
        /// <summary>
        /// 获取当前聚合根Id
        /// </summary>
        Guid AggregateId { get; }
        /// <summary>
        /// 获取当前聚合根版本号
        /// </summary>
        int Version { get; }
        /// <summary>
        /// 获取当前聚合根事件历史
        /// </summary>
        ConcurrentQueue<IEvent> GetChanges();
        /// <summary>
        /// 设置聚合根版本号
        /// </summary>
        /// <param name="version"></param>
        void SetVersion(int version);
        /// <summary>
        /// 装载聚合根事件历史
        /// </summary>
        /// <param name="history">聚合根事件集合</param>
        void LoadFromHistory(IEnumerable<IEvent> history);
    }
}
