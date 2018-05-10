using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PhotoStationFrame.Api;
using PhotoStationFrame.Uwp.Settings;
using PhotoStationFrame.Uwp.ViewObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotoStationFrame.Uwp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private PhotoStationClient photoClient;
        private readonly INavigationService navigationService;
        private ObservableCollection<ImageModel> _thumbnailUrls;

        public MainViewModel(PhotoStationClient photoStationClient, INavigationService navigationService)
        {
            this.photoClient = photoStationClient;
            this.navigationService = navigationService;
            GoToSettingsCommand = new RelayCommand(HandleGoToSettingsCommand);
        }

        private void HandleGoToSettingsCommand()
        {
            navigationService?.NavigateTo(ViewModelLocator.SettingsPageKey);
        }

        public async Task LoadData()
        {
            this.photoClient.Initialize(new PhotoApiSettings());
            await photoClient.LoginAsync();
            var albums = await photoClient.ListSmartAlbumsAsync();
            var album = albums.data.smart_albums.FirstOrDefault(x => x.name == "PhotoFrame");
            //var album = albums.data.items.FirstOrDefault(x => x.info.name == "Axel-Nokia8");
            if (album == null)
            {
                return;
            }

            var photos = await photoClient.ListSmartAlbumItemsAsync(album.id);
            var images = photos?.Select(p => new ImageModel(photoClient.GetBiglUrl(p), p, photoClient)).ToArray();
            if (images?.Length > 0)
            {
                ThumbnailUrls = new ObservableCollection<ImageModel>(images);
            }
        }

        public ICommand GoToSettingsCommand { get; set; }

        public ObservableCollection<ImageModel> ThumbnailUrls { get => _thumbnailUrls; set => Set(ref _thumbnailUrls, value); }
    }
}
