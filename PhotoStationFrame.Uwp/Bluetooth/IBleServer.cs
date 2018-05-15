using System.Threading.Tasks;

namespace PhotoStationFrame.Uwp.Bluetooth
{
    public interface IBleServer
    {
        Task<bool> CheckPeripheralRoleSupportAsync();
        Task<bool> ServiceProviderInitAsync();
    }
}