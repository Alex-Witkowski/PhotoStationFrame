using PhotoStationFrame.Api;
using PhotoStationFrame.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoStationFrame.Consol
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var photoClient = new PhotoStationClient();
            photoClient.Initialize(new DemoApiSettings());
            await photoClient.LoginAsync();
            var albums = await photoClient.ListSmartAlbumsAsync();
            //var album = albums.data.items.First(x => x.info.name == "SamsungAlex");
            var album = albums.data.smart_albums.First(x => x.name == "PhotoFrame");
            //var photos = await photoClient.ListPhotosAsync(album.id);
            PhotoItem[] photos = null;
            await photoClient.ListSmartAlbumItemsAsync(album.id);
            var url = photoClient.GetThumbnailUrl(photos[0]);
            await photoClient.GetThumbnailData(url);
            Console.ReadKey();
        }
    }
}
