using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SDKClassicalLib.CommandBus;
using SDKClassicalLib.Commands;

namespace SDK_InMemory_Lib.CommandBus
{
    public class CommandBus: ICommandBus
    {
        private readonly Dictionary<Type, ICommandHandler> _commandHandlers = 
            new Dictionary<Type, ICommandHandler>();

        public async Task Send<TCommand>(TCommand command) where TCommand : CommandBase
        {
            if (_commandHandlers.ContainsKey(typeof(TCommand)))
            {
                await _commandHandlers[typeof(TCommand)].Handle(command);
            }
        }

        public void Handle<TCommand>(Func<TCommand, Task> handler) where TCommand : CommandBase
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (_commandHandlers.ContainsKey(typeof(TCommand)))
            {
                throw new Exception($"Handler for {typeof(TCommand)} exist");
            }

            _commandHandlers.Add(typeof(TCommand), new CommandHandler<TCommand>(handler));
        }
    }
}