using PhotoStationFrame.Api;

namespace PhotoStationFrame.Uwp.Settings
{
    public class PhotoFrameSettings : IApiSettings
    {
        public PhotoFrameSettings(string address, string username, string password)
        {
            Address = address;
            Username = username;
            Password = password;
        }

        public string Url
        {
            get
            {
                var protocol = UseHttps ? "https://" : "http://";
                return protocol + Address;
            }
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Address { get; private set; }
        public string AlbumName { get; set; }
        public string AlbumId { get; set; }
        public bool UseHttps { get; set; }
        public bool UseSmartAlbum { get; set; }
        public bool RamdomSort { get; set; }
    }
}
