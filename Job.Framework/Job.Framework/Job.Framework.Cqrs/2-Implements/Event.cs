using System;

namespace Job.Framework.Cqrs
{
    [Serializable]
    public abstract class Event : IEvent
    {
        public Guid AggregateId { get; protected set; }

        public int Version { get; protected set; }

        public void AcceptChanges(Guid aggregateId, int version)
        {
            this.AggregateId = aggregateId;
            this.Version = version;
        }
    }
}
