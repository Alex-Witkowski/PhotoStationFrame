using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PhotoStationFrame.Api;
using PhotoStationFrame.Api.Models;
using PhotoStationFrame.Uwp.Extensions;
using PhotoStationFrame.Uwp.Settings;
using PhotoStationFrame.Uwp.ViewObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotoStationFrame.Uwp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private PhotoStationClient photoClient;
        private readonly INavigationService navigationService;
        private readonly ISettingsHelper settingsHelper;
        private ObservableCollection<ImageModel> _thumbnailUrls;

        private const int pageSize = 100;

        private const bool randomOrder = true;

        public MainViewModel(PhotoStationClient photoStationClient, INavigationService navigationService, ISettingsHelper settingsHelper)
        {
            this.photoClient = photoStationClient;
            this.navigationService = navigationService;
            this.settingsHelper = settingsHelper;
            GoToSettingsCommand = new RelayCommand(HandleGoToSettingsCommand);
        }

        private void HandleGoToSettingsCommand()
        {
            navigationService?.NavigateTo(ViewModelLocator.SettingsPageKey);
        }

        public async Task LoadData()
        {
            try
            {
                var settings = await settingsHelper.LoadAsync();
#if DEBUG
                /*  When debugging or deploying to IoT device use some "predefined" settings. Not included in git 
                    public class PhotoApiSettings : PhotoFrameSettings
                    {
                        public PhotoApiSettings() : base("diskstation", "user", "password")
                        ...
                */
                settings = new PhotoApiSettings();
#endif
                photoClient.Initialize(settings);
                var loginResult = await photoClient.LoginAsync();
                if(!loginResult)
                {
                    // Show message login not successfull.
                    return;
                }
                ListItemResponse listResponse = null;

                // ToDo: if smells like duplicate code
                string albumId = null;
                if (settings.UseSmartAlbum)
                {
                    // Known bug when album contains videos
                    var albums = await photoClient.ListSmartAlbumsAsync();
                    var album = albums.data.smart_albums.FirstOrDefault(x => x.name == settings.AlbumName);
                    if (album == null)
                    {
                        return;
                    }

                    albumId = album.id;
                    listResponse = await photoClient.ListSmartAlbumItemsAsync(albumId, 0, pageSize);
                }
                else
                {
                    var albums = await photoClient.ListAlbumsAsync();
                    var album = albums.data.items.FirstOrDefault(x => x.info.name == settings.AlbumName);
                    if (album == null)
                    {
                        return;
                    }

                    albumId = album.id;
                    listResponse = await photoClient.ListPhotosAsync(albumId, 0, pageSize);
                }

                var images = listResponse.data?.items?.Select(p => new ImageModel(photoClient.GetBiglUrl(p), p, photoClient)).ToList();
                if (randomOrder)
                {
                    images.Shuffle();
                }
                var tempimages = images.ToList();
                Images = new ObservableCollection<ImageModel>(images);

                for (int i = images.Count; i < listResponse.data.total; i += pageSize)
                    {
                    var pagingListResponse = settings.UseSmartAlbum ? (await photoClient.ListSmartAlbumItemsAsync(albumId, i, pageSize)) : (await photoClient.ListPhotosAsync(albumId, i, pageSize));
                    images = pagingListResponse.data?.items?.Select(p => new ImageModel(photoClient.GetBiglUrl(p), p, photoClient)).ToList();
                    tempimages.AddRange(images);
                }

                if (randomOrder)
                {
                    tempimages.Shuffle();
                }

                Images = new ObservableCollection<ImageModel>(tempimages);

            }
            catch (Exception e)
            {
                // ToDo: Show user some hint on error
                Debug.WriteLine(e.Message);
            }
        }

        public ICommand GoToSettingsCommand { get; set; }

        public ObservableCollection<ImageModel> Images { get => _thumbnailUrls; set => Set(ref _thumbnailUrls, value); }
    }
}
