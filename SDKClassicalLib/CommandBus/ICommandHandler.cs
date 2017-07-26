using System;
using System.Threading.Tasks;
using SDKClassicalLib.Commands;

namespace SDKClassicalLib.CommandBus
{
    /// <summary>
    /// Contains logic that should be executed for particular command
    /// </summary>
    public interface ICommandHandler: IDisposable
    {
        /// <summary>
        /// Executes partucular command
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>Task which completes once command has been executed</returns>
        Task Handle(CommandBase command);
    }
}