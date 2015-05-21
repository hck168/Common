using System;

namespace Job.Framework.Cqrs
{
    public interface IEvent
    {
        Guid AggregateId { get; }
        int Version { get; }

        void AcceptChanges(Guid aggregateId, int version);
    }
}
