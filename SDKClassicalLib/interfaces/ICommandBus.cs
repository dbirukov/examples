using System;
using System.Threading.Tasks;
using SDKClassicalLib.Commands;

namespace SDKClassicalLib.Interfaces
{
    public interface ICommandBus
    {
        Task Send<TCommand>(TCommand command) where TCommand : CommandBase;
        void Handle<TCommand>(Func<CommandBase, Task> handler) where TCommand : CommandBase;
    }
}