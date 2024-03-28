using Tanji.Core.Net;
using Tanji.Core.Canvas;

namespace Tanji.Core.API;

public interface IInstaller
{
    IGame Game { get; }
    IHConnection Connection { get; }
}