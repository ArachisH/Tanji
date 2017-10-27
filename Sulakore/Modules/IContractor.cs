using Sulakore.Habbo;
using Sulakore.Habbo.Web;
using Sulakore.Communication;
using Sulakore.Habbo.Messages;

namespace Sulakore.Modules
{
    public interface IContractor
    {
        Incoming In { get; }
        Outgoing Out { get; }

        HHotel Hotel { get; }
        HGameData GameData { get; }
        IHConnection Connection { get; }
    }
}