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
            var albums = await photoClient.ListAlbumsAsync();
            var album = albums.data.items.First(x => x.info.name == "SamsungAlex");
            await photoClient.ListItemsAsync(album.id);
            Console.ReadKey();
        }
    }
}
