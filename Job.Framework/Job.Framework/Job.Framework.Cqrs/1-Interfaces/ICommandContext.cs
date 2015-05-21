using System;

namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 定义一个聚合溯源操作接口
    /// </summary>
    public interface ICommandContext
    {
        /// <summary>
        /// 保存一个聚合根
        /// </summary>
        /// <param name="aggregate">一个聚合根</param>
        /// <param name="expectedVersion">预期的版本号</param>
        void Add(IAggregateRoot aggregate, int expectedVersion);

        /// <summary>
        /// 获取一个聚合根
        /// </summary>
        /// <typeparam name="T">抽象聚合根的派生类</typeparam>
        /// <param name="aggregateId">聚合根Id</param>
        /// <returns>返回一个聚合根</returns>
        T Get<T>(Guid aggregateId) where T : class, IAggregateRoot;
    }
}
