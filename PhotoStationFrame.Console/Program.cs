using PhotoStationFrame.Api;
using System;

namespace PhotoStationFrame.Consol
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var photoClient = new PhotoStationClient();
            photoClient.LoginAsync();
            Console.ReadKey();
        }
    }
}
