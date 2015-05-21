
namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 定义命令执行的状态
    /// </summary>
    public enum CommandStatus
    {
        None = 0,
        /// <summary>
        /// 命令执行成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 命令执行失败
        /// </summary>
        Failed = 2
    }

    /// <summary>
    /// 定义一个命令执行的结果接口
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// 获取命令执行状态
        /// </summary>
        CommandStatus Status { get; }
        /// <summary>
        /// 获取命令结果消息
        /// </summary>
        string Message { get; }
    }
}
