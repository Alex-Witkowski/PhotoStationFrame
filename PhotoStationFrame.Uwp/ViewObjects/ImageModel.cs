using PhotoStationFrame.Api;

namespace PhotoStationFrame.Uwp.ViewObjects
{
    public class ImageModel
    {
        public ImageModel(string url,PhotoStationClient client)
        {
            Url = url;
            Client = client;
        }

        public string Url;
        public PhotoStationClient Client;
    }
}
