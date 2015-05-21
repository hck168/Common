
namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 用于命令执行状态的结果封装
    /// </summary>
    public sealed class CommandResult : ICommandResult
    {
        #region 相关属性

        /// <summary>
        /// 获取命令执行结果状态
        /// </summary>
        public CommandStatus Status { get; private set; }
        /// <summary>
        /// 获取命令执行结果消息
        /// </summary>
        public string Message { get; private set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 实例化 CommandSendResult 对象
        /// </summary>
        /// <param name="status">执行结果状态</param>
        /// <param name="message">执行消息状态</param>
        private CommandResult(CommandStatus status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

        #endregion

        #region 相关方法

        /// <summary>
        /// 创建命令执行状态的结果封装
        /// </summary>
        /// <param name="status">执行结果状态</param>
        /// <param name="message">执行消息状态</param>
        /// <returns>返回当前 CommandSendResult 对象</returns>
        public static ICommandResult Create(CommandStatus status, string message)
        {
            return new CommandResult
            (
                status: status,
                message: message
            );
        }

        #endregion
    }
}
