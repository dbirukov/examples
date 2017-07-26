using System;
using System.Threading.Tasks;
using SDKClassicalLib.CommandBus;
using SDKClassicalLib.Commands;

namespace SDK_InMemory_Lib.CommandBus
{
    public class CommandHandler<TCommand> : ICommandHandler where TCommand: CommandBase
    {
        private readonly Func<TCommand, Task> _handler; 
        public CommandHandler(Func<TCommand, Task> handler)
        {
            _handler = handler;
        }

        public Task Handle(CommandBase command)
        {
            return _handler.Invoke((TCommand) command);
        }
        
        public void Dispose()
        {
        }
    }
}