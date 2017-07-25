using SDKClassicalLib.Commands;

namespace SDKClassicalLib.Interfaces
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand data) where TCommand : CommandBase;
        void Handle<TCommand, THandler>(ICommandHandler handler) where TCommand : CommandBase;
    }
}