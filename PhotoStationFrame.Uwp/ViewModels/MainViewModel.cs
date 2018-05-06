using GalaSoft.MvvmLight;
using PhotoStationFrame.Api;
using PhotoStationFrame.Uwp.Settings;
using PhotoStationFrame.Uwp.ViewObjects;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoStationFrame.Uwp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private PhotoStationClient photoClient;
        private ObservableCollection<ImageModel> _thumbnailUrls;

        public MainViewModel()
        {
            this.photoClient = new PhotoStationClient();
        }

        public async Task LoadData()
        {
            this.photoClient.Initialize(new PhotoApiSettings());
            await photoClient.LoginAsync();
            var albums = await photoClient.ListAlbumsAsync();
            var album = albums.data.items.First(x => x.info.name == "SamsungAlex");
            var photos = await photoClient.ListItemsAsync(album.id);
            var images = photos.Select(p => new ImageModel(photoClient.GetBiglUrl(p),photoClient));
            ThumbnailUrls = new ObservableCollection<ImageModel>(images);

        }

        public ObservableCollection<ImageModel> ThumbnailUrls { get => _thumbnailUrls; set => Set(ref _thumbnailUrls, value); }
    }
}
