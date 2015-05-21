using System;

namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 派生类命令数据的封装都必须继承此抽象类
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// 获取或者保护设置聚合标识
        /// </summary>
        public Guid AggregateId { get; private set; }
        /// <summary>
        /// 获取或保护设置聚合版本号
        /// </summary>
        public int Version { get; private set; }

        protected Command(Guid aggregateId, int version)
        {
            this.AggregateId = aggregateId;
            this.Version = version;
        }
    }
}
