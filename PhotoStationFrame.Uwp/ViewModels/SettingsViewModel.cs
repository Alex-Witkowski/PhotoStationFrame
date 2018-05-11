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
using Xamarin.Essentials;

namespace PhotoStationFrame.Uwp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly PhotoStationClient photoStationClient;
        private readonly ISettingsHelper settingsHelper;
        private string _address;
        private string _userName;
        private string _password;
        private bool _useHttps;
        private ObservableCollection<Smart_Album> _smartAlbums;
        private ObservableCollection<AlbumItem> _albums;
        private AlbumTypes _selectedAlbumType;
        private bool _loginSuccessfull;
        private bool _useSmartAlbum;
        private bool _isLoading;
        private string _message;
        private object _selectedAlbum;
        private object _selectedSmartAlbum;

        public SettingsViewModel(INavigationService navigationService, PhotoStationClient photoStationClient,ISettingsHelper settingsHelper)
        {
            this.navigationService = navigationService;
            this.photoStationClient = photoStationClient;
            this.settingsHelper = settingsHelper;
            CheckCredetialsCommand = new RelayCommand(HandleCheckCredetialsCommand);
            SaveSettingsCommand = new RelayCommand(HandleSaveSettingsCommand, CanExecuteSettingsCommand);
            CancelCommand = new RelayCommand(HandleCancelCommand);
            UseSmartAlbum = true;
        }

        private bool CanExecuteSettingsCommand()
        {
            return true;
        }

        private void HandleCancelCommand()
        {
            navigationService.GoBack();
        }

        private async void HandleSaveSettingsCommand()
        {
            IsLoading = true;
            var settings = new PhotoFrameSettings(Address, UserName, Password);
            settings.UseHttps = UseHttps;
            settings.UseSmartAlbum = UseSmartAlbum;
            if(UseSmartAlbum)
            {
                var album = SelectedSmartAlbum as Smart_Album;
                settings.AlbumName = album?.name;
                settings.AlbumId = album?.id;
            }
            else
            {
                var album = SelectedAlbum as AlbumItem;
                settings.AlbumName = album?.info?.name;
                settings.AlbumId = album?.id;
            }

            await settingsHelper.SaveAsync(settings);
            IsLoading = false;
            navigationService.GoBack();
        }

        public async void Initialize()
        {
            var settings = await settingsHelper.LoadAsync();
            if(settings == null)
            {
                return;
            }

            Address = settings.Address;
            UserName = settings.Username;
            Password = settings.Password;
            UseHttps = settings.UseHttps;
        }

        private async void HandleCheckCredetialsCommand()
        {
            try
            {
                Message = string.Empty;
                IsLoading = true;
                photoStationClient.Initialize(new PhotoFrameSettings(Address, UserName, Password) { UseHttps = UseHttps });
                var result = await photoStationClient.LoginAsync();
                if (result)
                {
                    var albums = await photoStationClient.ListAlbumsAsync();
                    Albums = new ObservableCollection<AlbumItem>(albums?.data?.items);
                    var smartAlbums = await photoStationClient.ListSmartAlbumsAsync();
                    SmartAlbums = new ObservableCollection<Smart_Album>(smartAlbums?.data?.smart_albums);
                    LoginSuccessfull = true;
                }
                else
                {
                    Message = "Login not successfull.";
                }

            }
            catch (System.Exception e)
            {
                Message = $"Something went wrong:\r\n{e.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public string Message { get => _message; set => Set(ref _message, value); }

        public string Address
        {
            get => _address;
            set
            {
                if (Set(ref _address, value))
                {
                    LoginSuccessfull = false;
                }
            }
        }
        public string UserName
        {
            get => _userName; set
            {
                if (Set(ref _userName, value))
                {
                    LoginSuccessfull = false;
                }
            }
        }
        public string Password
        {
            get => _password; set
            {
                if (Set(ref _password, value))
                {
                    LoginSuccessfull = false;
                }
            }
        }
        public bool UseHttps
        {
            get => _useHttps; set
            {
                if (Set(ref _useHttps, value))
                { LoginSuccessfull = false; }
            }
        }

        public bool UseSmartAlbum { get => _useSmartAlbum; set => Set(ref _useSmartAlbum, value); }

        public bool IsLoading { get => _isLoading; set => Set(ref _isLoading, value); }

        public bool LoginSuccessfull { get => _loginSuccessfull; set => Set(ref _loginSuccessfull, value); }

        public object SelectedAlbum { get => _selectedAlbum; set => Set(ref _selectedAlbum, value); }

        public object SelectedSmartAlbum { get => _selectedSmartAlbum; set => Set(ref _selectedSmartAlbum ,value); }

        public ICommand CheckCredetialsCommand { get; set; }

        public ObservableCollection<Smart_Album> SmartAlbums { get => _smartAlbums; set => Set(ref _smartAlbums, value); }
        public ObservableCollection<AlbumItem> Albums { get => _albums; set => Set(ref _albums, value); }
        public AlbumTypes SelectedAlbumType { get => _selectedAlbumType; set => Set(ref _selectedAlbumType, value); }

        public ICommand SaveSettingsCommand { get; set; }

        public ICommand CancelCommand { get; set; }
    }
}
