using System;
using System.Threading.Tasks;
using SDKClassicalLib.Commands;

namespace SDKClassicalLib.CommandBus
{
    /// <summary>
    /// Used to deliver command for execution. Command sent to the bus should be delivered only to a single handler 
    /// and executed only once.
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Request that a command should be executed.
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <typeparam name="TCommand">Type of command to execute</typeparam>
        /// <returns>Task which completes once the command has been delivered</returns>
        /// <para>
        /// The actual execution of an command can be done anywhere at any time. Do not expect the command to be 
        /// executed just because this method returns. That just means that the command have been successfully 
        /// delivered (to a queue or another process etc) for execution.
        /// </para>
        Task Send<TCommand>(TCommand command) where TCommand : CommandBase;
        
        /// <summary>
        /// Add handler for particular command type. It is possible to assign only single handler for each command.
        /// </summary>
        /// <param name="handler">Async function that have command input parameter return Task</param>
        /// <typeparam name="TCommand">Type of command to which handler should be binded</typeparam>
        void Handle<TCommand>(Func<TCommand, Task> handler) where TCommand : CommandBase;
    }
}