using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Job.Framework.Cqrs
{
    /// <summary>
    /// 用于命令执行总线的统一入口
    /// </summary>
    [Export(typeof(ICommandBus)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal sealed class CommandBus : ICommandBus
    {
        /// <summary>
        /// 往命令总线发送命令，返回命令执行的结果
        /// </summary>
        /// <typeparam name="TCommand">必须是继承 Command 基类的派生类命令</typeparam>
        /// <param name="command">命令封装数据消息</param>
        /// <returns>返回命令执行的结果</returns>
        public ICommandResult Send<TCommand>(TCommand command) where TCommand : Command
        {
            try
            {
                var handler = MefComposition.Container.GetExportedValueOrDefault<ICommandHandler<TCommand>>();

                if (handler != null)
                {
                    handler.Execute(command);
                }
                else
                {
                    throw new ArgumentNullException("未将 ICommandHandler<{0}> 接口实现", typeof(TCommand).Name);
                }

                return CommandResult.Create(CommandStatus.Success, "操作成功");
            }
            catch (Exception ex)
            {
                return CommandResult.Create(CommandStatus.Failed, ex.Message);
            }
        }

        public Task<ICommandResult> SendAsync<TCommand>(TCommand command) where TCommand : Command
        {
            try
            {
                var handler = MefComposition.Container.GetExportedValueOrDefault<ICommandHandler<TCommand>>();

                if (handler != null)
                {
                    handler.Execute(command);
                }
                else
                {
                    throw new ArgumentNullException("未将 ICommandHandler<{0}> 接口实现", typeof(TCommand).Name);
                }

                return Task.FromResult(CommandResult.Create(CommandStatus.Success, "操作成功"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(CommandResult.Create(CommandStatus.Failed, ex.Message));
            }
        }
    }
}
