using PhotoStationFrame.Api;
using System;
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
            await photoClient.ListAlbums();
            Console.ReadKey();
        }
    }
}
