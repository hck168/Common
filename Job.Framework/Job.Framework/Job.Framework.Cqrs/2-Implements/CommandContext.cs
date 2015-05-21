using Job.Framework.Common;
using Job.Framework.Config;
using Job.Framework.Cqrs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Job.Framework.CqrsStore
{
    /// <summary>
    /// 用于领域事件仓储
    /// </summary>
    [Export(typeof(ICommandContext)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class CommandContext : ICommandContext
    {
        #region 相关属性

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus eventBus;

        private static ConcurrentDictionary<Guid, IAggregateRoot> aggregateStore = new ConcurrentDictionary<Guid, IAggregateRoot>();

        #endregion

        #region 构造函数

        [ImportingConstructor]
        public CommandContext(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        #endregion

        /// <summary>
        /// 保存一个聚合根
        /// </summary>
        /// <param name="aggregate">一个聚合根</param>
        /// <param name="expectedVersion">预期的版本号</param>
        public void Add(IAggregateRoot aggregate, int expectedVersion)
        {
            if (aggregate.Version != expectedVersion)
            {
                throw new ArgumentException("当前数据已过期，请重新加载再次尝试");
            }

            IEvent handle = null;

            while (aggregate.GetChanges().TryDequeue(out handle))
            {
                aggregate.SetVersion(aggregate.Version + 1);
                handle.AcceptChanges(aggregate.AggregateId, aggregate.Version);

                using (var dbContext = DbContext.BeginConnect(AppConfigManager.ConnectionSection.WriteDb))
                {
                    if (aggregate.Version > 2 && aggregate.Version % 3 == 0)
                    {
                        dbContext.Insert("T_DomainSnapshot", false).Values(new
                        {
                            AggregateId = aggregate.AggregateId,
                            AggregateName = aggregate.GetType().Name,
                            AggregateSnapshot = GlobalHelper.Serialize(aggregate),
                            Version = aggregate.Version,
                            CreateTime = DateTime.Now
                        });
                    }

                    dbContext.Insert("T_DomainEvent", false).Values(new
                    {
                        AggregateId = aggregate.AggregateId,
                        AggregateName = aggregate.GetType().Name,
                        AggregateEvent = GlobalHelper.Serialize(handle),
                        Version = handle.Version,
                        CreateTime = DateTime.Now
                    });
                }

                eventBus.Publish(handle as dynamic);
            }

            aggregateStore.AddOrUpdate(aggregate.AggregateId, aggregate, (k, v) =>
            {
                return v;
            });
        }

        /// <summary>
        /// 获取一个聚合根
        /// </summary>
        /// <typeparam name="T">抽象聚合根的派生类</typeparam>
        /// <param name="aggregateId">聚合根Id</param>
        /// <returns>返回一个聚合根</returns>
        public T Get<T>(Guid aggregateId) where T : class, IAggregateRoot
        {
            IAggregateRoot aggregate = null;

            if (!aggregateStore.TryGetValue(aggregateId, out aggregate))
            {
                aggregate = GlobalHelper.CreateInstance<IAggregateRoot>(typeof(T));

                using (var dbContext = DbContext.BeginConnect(AppConfigManager.ConnectionSection.WriteDb))
                {
                    var snapshot = dbContext.Select("T_DomainSnapshot", "Version DESC").Columns("AggregateSnapshot").Where(new
                    {
                        AggregateId = aggregateId
                    });

                    if (snapshot != null)
                    {
                        aggregate = GlobalHelper.Deserialize(snapshot.AggregateSnapshot) as IAggregateRoot;
                    }

                    var history = dbContext.Query("T_DomainEvent", "Version ASC").Columns("AggregateEvent").Where("AggregateId = @AggregateId AND Version > @Version", new
                    {
                        AggregateId = aggregateId,
                        Version = aggregate.Version
                    });

                    aggregate.LoadFromHistory(history.Select(his => { return GlobalHelper.Deserialize(his.AggregateEvent) as IEvent; }));
                }

                aggregateStore.AddOrUpdate(aggregate.AggregateId, aggregate, (k, v) =>
                {
                    return v;
                });
            }

            return aggregate as T;
        }
    }
}
