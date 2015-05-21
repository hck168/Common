
namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 定义接口用于事件发布总线的统一入口
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 往事件总线发布命令
        /// </summary>
        /// <typeparam name="TEvent">必须是继承 Event 基类的派生类事件</typeparam>
        /// <param name="e">事件数据包</param>
        void Publish<TEvent>(TEvent e) where TEvent : Event;
    }
}
