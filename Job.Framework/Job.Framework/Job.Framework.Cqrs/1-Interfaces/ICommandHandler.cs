
namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 定义一个对应的 ICommand 处理的接口
    /// </summary>
    /// <typeparam name="TCommand">可执行命令的数据封装</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : Command
    {
        /// <summary>
        /// 开始执行命令
        /// </summary>
        /// <param name="command">可执行命令的数据封装</param>
        void Execute(TCommand command);
    }
}
