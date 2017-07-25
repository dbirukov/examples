using System;
using System.Threading.Tasks;
using SDKClassicalLib.Commands;
using SDKClassicalLib.Interfaces;

namespace SKDClassicalExample.CommandBus
{
    public class CommandHandler<TCommand> : ICommandHandler where TCommand: CommandBase
    {
        public CommandHandler()
        {
        }

        public Task Handle(CommandBase command)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}