using PhotoStationFrame.Api;
using PhotoStationFrame.Api.Models;

namespace PhotoStationFrame.Uwp.ViewObjects
{
    public class ImageModel
    {
        public ImageModel(string url,PhotoItem photoItem, PhotoStationClient client)
        {
            Url = url;
            Client = client;
            var info = photoItem.info;
            Width = !info.rotated ? info.resolutionx : info.resolutiony;
            Height = !info.rotated ? photoItem.info.resolutiony : info.resolutionx;
        }

        public string Url;
        public PhotoStationClient Client;

        public int Width { get; private set; }
        public int Height { get; }
    }
}
