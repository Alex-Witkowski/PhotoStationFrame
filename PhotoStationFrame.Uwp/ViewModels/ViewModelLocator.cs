using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using PhotoStationFrame.Api;
using PhotoStationFrame.Uwp.Views;
using System;

namespace PhotoStationFrame.Uwp.ViewModels
{
    public class ViewModelLocator
    {
        public const string MainPageKey = "MainPage";
        public const string SettingsPageKey = "SettingsPage";

        public ViewModelLocator()
        {
            ConfigureIocContainer();
            SetupDependencies();
            SetupNavigation();

        }

        private void SetupNavigation()
        {
            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>() as NavigationService;
            navigationService.Configure(MainPageKey, typeof(MainPage));
            navigationService.Configure(SettingsPageKey, typeof(SettingsPage));
        }

        private void SetupDependencies()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<PhotoStationClient>();
            SimpleIoc.Default.Register<INavigationService,NavigationService>();
        }

        private void ConfigureIocContainer()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
