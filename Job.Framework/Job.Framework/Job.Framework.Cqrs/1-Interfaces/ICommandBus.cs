
using System.Threading.Tasks;
namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 定义接口用于命令执行总线的统一入口
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// 往命令总线发送命令，返回命令执行的结果
        /// </summary>
        /// <typeparam name="TCommand">必须是继承 Command 基类的派生类命令</typeparam>
        /// <param name="command">命令封装数据消息</param>
        /// <returns>返回命令执行的结果</returns>
        ICommandResult Send<TCommand>(TCommand command) where TCommand : Command;
        Task<ICommandResult> SendAsync<TCommand>(TCommand command) where TCommand : Command;
    }
}
