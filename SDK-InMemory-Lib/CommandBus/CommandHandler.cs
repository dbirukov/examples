using System;
using System.Threading.Tasks;
using SDKClassicalLib.Commands;
using SDKClassicalLib.Interfaces;

namespace SDK_InMemory_Lib.CommandBus
{
    public class CommandHandler : ICommandHandler
    {
        private readonly Func<CommandBase, Task> _handler; 
        public CommandHandler(Func<CommandBase, Task> handler)
        {
            _handler = handler;
        }

        public Task Handle(CommandBase command)
        {
            return _handler(command);
        }
        
        public void Dispose()
        {
        }
    }
}