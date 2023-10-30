using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Habbo.Network;

namespace Tanji.Core.API;

public interface IInstaller
{
    IGame Game { get; }
    IHConnection Connection { get; }
}