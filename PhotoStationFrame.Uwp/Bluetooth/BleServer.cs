using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace PhotoStationFrame.Uwp.Bluetooth
{
    public class BleServer : IBleServer
    {
        public static readonly Guid FrameServiceUuid = Guid.Parse("D42F0914-70BE-47DF-926B-D78F90DF39F0");

        public static readonly Guid AddressCharacteristicUuid = Guid.Parse("2EB5BCF1-11F5-4661-B942-BB57C0596B10");

        public static readonly GattLocalCharacteristicParameters GattAddressParameters = new GattLocalCharacteristicParameters
        {
            CharacteristicProperties = GattCharacteristicProperties.Write |
                                       GattCharacteristicProperties.WriteWithoutResponse,
            WriteProtectionLevel = GattProtectionLevel.Plain,
            UserDescription = "Address Characteristic"
        };
        private GattServiceProvider serviceProvider;
        private GattLocalCharacteristic addressCharacteristic;

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

        public async Task<bool> ServiceProviderInitAsync()
        {
            // BT_Code: Initialize and starting a custom GATT Service using GattServiceProvider.
            GattServiceProviderResult serviceResult = await GattServiceProvider.CreateAsync(FrameServiceUuid);
            if (serviceResult.Error == BluetoothError.Success)
            {
                serviceProvider = serviceResult.ServiceProvider;
            }
            else
            {
                Debug.WriteLine($"Could not create service provider: {serviceResult.Error}");
                return false;
            }

            GattLocalCharacteristicResult result = await serviceProvider.Service.CreateCharacteristicAsync(AddressCharacteristicUuid, GattAddressParameters);
            if (result.Error == BluetoothError.Success)
            {
                addressCharacteristic = result.Characteristic;
            }
            else
            {
                Debug.WriteLine($"Could not create operand1 characteristic: {result.Error}");
                return false;
            }

            addressCharacteristic.WriteRequested += AddressCharacteristic_WriteRequestedAsync; ;


            return true;
        }

        private async void AddressCharacteristic_WriteRequestedAsync(GattLocalCharacteristic sender, GattWriteRequestedEventArgs args)
        {
            // BT_Code: Processing a write request.
            using (args.GetDeferral())
            {
                // Get the request information.  This requires device access before an app can access the device's request.
                GattWriteRequest request = await args.GetRequestAsync();
                if (request == null)
                {
                    // No access allowed to the device. Application should indicate this to the user.
                    return;
                }

                ProcessWriteCharacteristic(request);
            }
        }

        private void ProcessWriteCharacteristic(GattWriteRequest request)
        {
            if (request.Value.Length != 4)
            {
                // Input is the wrong length. Respond with a protocol error if requested.
                if (request.Option == GattWriteOption.WriteWithResponse)
                {
                    request.RespondWithProtocolError(GattProtocolError.InvalidAttributeValueLength);
                }
                return;
            }

            var reader = DataReader.FromBuffer(request.Value);
            reader.ByteOrder = ByteOrder.LittleEndian;
            int val = reader.ReadInt32();

            Debug.WriteLine($"Write {val}");

            // Complete the request if needed
            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }
        }
    }
}
