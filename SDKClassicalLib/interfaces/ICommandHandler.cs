using System.Threading.Tasks;
using SDKClassicalLib.Commands;

namespace SDKClassicalLib.Interfaces
{
    public interface ICommandHandler<in TCommand>: ICommandHandler where TCommand: CommandBase
    {
        Task Handle(TCommand @event);
    }

    public interface ICommandHandler
    {
    }
}