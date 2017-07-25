using System;
using System.Threading.Tasks;
using SDKClassicalLib.Commands;

namespace SDKClassicalLib.Interfaces
{
    public interface ICommandHandler: IDisposable
    {
        Task Handle(CommandBase command);
    }
}