
namespace Job.Framework.Cqrs
{
    public interface IHandle<TEvent> where TEvent : Event
    {
        void Apply(TEvent handle);
    }
}
