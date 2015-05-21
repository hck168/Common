
namespace Job.Framework.Cqrs
{
    public interface IEventHandler<in TEvent> where TEvent : Event
    {
        void Execute(TEvent handle);
    }
}
