using PhotoStationFrame.Uwp.ViewModels;
using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoStationFrame.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer flipTimer;
        private DispatcherTimer reloadTimer;
        private bool rotate = false;
        private bool imagesShown;

        public MainPage()
        {
            this.InitializeComponent();

            // Initialize timer thats changes image every x seconds
            flipTimer = new DispatcherTimer();
            flipTimer.Interval = TimeSpan.FromSeconds(10);
            flipTimer.Tick += HandleFlipTimerTick;

            // Initialize timer that relaods the images after y minutes
            reloadTimer = new DispatcherTimer();
            reloadTimer.Interval = TimeSpan.FromMinutes(10);
            reloadTimer.Tick += HandleReloadTimerTick;

            Loaded += HanldeLoaded;
        }

        private void HandleReloadTimerTick(object sender, object e)
        {
            // ToDo: Handle Relaod better with loading images and check if something changed
            if(!imagesShown)
            {
                return;
            }

            var viewModel = this.DataContext as MainViewModel;
            viewModel?.LoadData();
            imagesShown = false;
        }

        private void HanldeLoaded(object sender, RoutedEventArgs e)
        {
            CheckRotation();
        }

        private void HandleFlipTimerTick(object sender, object e)
        {
            if (!(MyFlipView.Items?.Count > 0))
            {
                return;
            }

            if (MyFlipView.SelectedIndex < MyFlipView.Items.Count - 1)
            {
                MyFlipView.SelectedIndex++;
            }
            else
            {
                MyFlipView.SelectedIndex = 0;
                imagesShown = true;
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Add handler for settinge keyboard shortcut
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            var vm = this.DataContext as MainViewModel;
            await vm?.LoadData();
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (sender.GetKeyState(VirtualKey.Control) == CoreVirtualKeyStates.Down &&
                args.VirtualKey == VirtualKey.S)
            {
                var vm = this.DataContext as MainViewModel;
                vm?.GoToSettingsCommand?.Execute(null);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Remove handler for settinge keyboard shortcut
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        private void CheckRotation()
        {
            if (!rotate)
            {
                return;
            }

            MyGrid.Width = this.ActualHeight;
            MyGrid.Height = this.ActualWidth;
            MyFlipView.Width = this.ActualHeight;
            MyFlipView.Height = this.ActualWidth;
            MyGrid.RenderTransformOrigin = new Point(0.5, 0.5);
            var transformGroup = new TransformGroup();

            var translateTransform = new TranslateTransform();
            translateTransform.X = (this.ActualWidth - this.ActualHeight) / 2 * -1;
            Debug.WriteLine(translateTransform.Y);
            translateTransform.Y = 0;
            transformGroup.Children.Add(translateTransform);
            var rotateTransfor = new RotateTransform();
            rotateTransfor.Angle = 90;
            transformGroup.Children.Add(rotateTransfor);
            MyGrid.RenderTransform = transformGroup;
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine(e?.ErrorMessage);
        }

        private void FlipView_Loaded(object sender, RoutedEventArgs e)
        {
            flipTimer.Start();
            reloadTimer.Start();
        }

        private void MyFlipView_Unloaded(object sender, RoutedEventArgs e)
        {
            flipTimer.Stop();
            reloadTimer.Stop();
        }
    }
}
