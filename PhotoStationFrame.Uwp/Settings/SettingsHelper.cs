using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PhotoStationFrame.Uwp.Settings
{
    public class SettingsHelper : ISettingsHelper
    {
        private const string UserNameKey = "UserName";
        private const string PasswordKey = "Password";
        private const string AddressKey = "Address";
        private const string HttpsKey = "Https";
        private const string SortTypeKey = "SortType";
        private const string SortDirectionKey = "SortDirection";
        private const string AlbumNameKey = "AlbumName";
        private const string AlbumIdKey = "AlbumId";
        private const string UseSmartAlbumKey = "UseSmartAlbum";

        public async Task SaveAsync(PhotoFrameSettings settings)
        {
            Preferences.Set(AddressKey, settings.Address);
            await SecureStorage.SetAsync(UserNameKey, settings.Username).ConfigureAwait(false);
            await SecureStorage.SetAsync(PasswordKey, settings.Password).ConfigureAwait(false);
            Preferences.Set(HttpsKey, settings.UseHttps);
            Preferences.Set(UseSmartAlbumKey, settings.UseSmartAlbum);
            Preferences.Set(AlbumNameKey, settings.AlbumName);
            Preferences.Set(AlbumIdKey, settings.AlbumId);
        }

        public async Task<PhotoFrameSettings> LoadAsync()
        {
            var address = Preferences.Get(AddressKey, null);
            var username = await SecureStorage.GetAsync(UserNameKey).ConfigureAwait(false);
            var password = await SecureStorage.GetAsync(PasswordKey).ConfigureAwait(false);
            if(address == null || username == null || password == null)
            {
                return null;
            }

            var result = new PhotoFrameSettings(address, username, password);
            result.UseHttps = Preferences.Get(HttpsKey, false);
            result.UseSmartAlbum = Preferences.Get(UseSmartAlbumKey, true);
            result.AlbumName = Preferences.Get(AlbumNameKey, null);
            result.AlbumId = Preferences.Get(AlbumIdKey, null);

            return result;
        }
    }
}
