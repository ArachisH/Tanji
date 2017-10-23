using Sulakore.Habbo;
using Sulakore.Habbo.Web;
using Sulakore.Communication;

namespace Sulakore.Modules
{
    public interface IContractor
    {
        HHotel Hotel { get; }
        HGameData GameData { get; }
        IHConnection Connection { get; }
    }
}