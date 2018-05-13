using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace PhotoStationFrame.Uwp.Bluetooth
{
    public static class BleExtensions
    {
        public static bool Success(this GattServiceProviderResult result)
        {
            return result.Error == BluetoothError.Success;
        }

        public static bool Success(this GattLocalCharacteristicResult result)
        {
            return result.Error == BluetoothError.Success;
        }
    }
}
