using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PhotoStationFrame.Api;
using PhotoStationFrame.Api.Models;
using PhotoStationFrame.Uwp.Models;
using PhotoStationFrame.Uwp.Settings;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PhotoStationFrame.Uwp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly PhotoStationClient photoStationClient;
        private string _address;
        private string _userName;
        private string _password;
        private bool _useHttps;
        private ObservableCollection<Smart_Albums> _smartAlbums;
        private ObservableCollection<AlbumItem> _albums;
        private AlbumTypes _selectedAlbumType;

        public SettingsViewModel(INavigationService navigationService, PhotoStationClient photoStationClient)
        {
            this.navigationService = navigationService;
            this.photoStationClient = photoStationClient;
            CheckCredetialsCommand = new RelayCommand(HandleCheckCredetialsCommand);
            SaveSettingsCommand = new RelayCommand(HandleSaveSettingsCommand);
            CancelCommand = new RelayCommand(HandleCancelCommand);
        }

        private void HandleCancelCommand()
        {
            navigationService.GoBack();
        }

        private void HandleSaveSettingsCommand()
        {
            navigationService.GoBack();
        }

        private async void HandleCheckCredetialsCommand()
        {
            photoStationClient.Initialize(new ApiSettings("http://"+Address, UserName, Password));
            var result = await photoStationClient.LoginAsync();
            if (result)
            {
                var albums = await photoStationClient.ListAlbumsAsync();
                Albums = new ObservableCollection<AlbumItem>(albums?.data?.items);
                var smartAlbums = await photoStationClient.ListSmartAlbumsAsync();
                SmartAlbums = new ObservableCollection<Smart_Albums>(smartAlbums?.data?.smart_albums);
            }
        }

        public string Address { get => _address; set => Set(ref _address , value); }
        public string UserName { get => _userName; set => Set(ref _userName , value); }
        public string Password { get => _password; set => Set( ref _password ,value); }
        public bool UseHttps { get => _useHttps; set => Set( ref _useHttps, value); }

        public ICommand CheckCredetialsCommand { get; set; }

        public ObservableCollection<Smart_Albums> SmartAlbums { get => _smartAlbums; set => Set( ref _smartAlbums, value); }
        public ObservableCollection<AlbumItem> Albums { get => _albums; set => Set( ref _albums, value); }
        public AlbumTypes SelectedAlbumType { get => _selectedAlbumType; set => Set( ref _selectedAlbumType, value); }

        public ICommand SaveSettingsCommand { get; set; }

        public ICommand CancelCommand { get; set; }
    }
}
