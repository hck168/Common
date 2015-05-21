using System.ComponentModel.Composition;

namespace Job.Framework.Cqrs
{
    [Export(typeof(IEventBus)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class EventBus : IEventBus
    {
        public void Publish<TEvent>(TEvent handle) where TEvent : Event
        {
            foreach (var eventHandler in MefComposition.Container.GetExportedValues<IEventHandler<TEvent>>())
            {
                eventHandler.Execute(handle);
            }
        }
    }
}
