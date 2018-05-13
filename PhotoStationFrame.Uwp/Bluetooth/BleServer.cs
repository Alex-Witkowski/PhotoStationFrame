using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace PhotoStationFrame.Uwp.Bluetooth
{
    public class BleServer
    {
        public async Task<bool> CheckPeripheralRoleSupportAsync()
        {
            // BT_Code: New for Creator's Update - Bluetooth adapter has properties of the local BT radio.
            var localAdapter = await BluetoothAdapter.GetDefaultAsync();

            if (localAdapter != null)
            {
                return localAdapter.IsPeripheralRoleSupported;
            }
            else
            {
                // Bluetooth is not turned on 
                return false;
            }
        }
    }
}
