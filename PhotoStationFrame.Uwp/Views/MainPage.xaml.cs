﻿using PhotoStationFrame.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
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
        private DispatcherTimer timer;
        private bool rotate = true;

        public MainPage()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += HandleTimerTick;
            Loaded += HanldeLoaded;
        }

        private void HanldeLoaded(object sender, RoutedEventArgs e)
        {
            CheckRotation();
        }

        private void HandleTimerTick(object sender, object e)
        {
            if(MyFlipView.SelectedIndex <= MyFlipView.Items?.Count)
            {
                MyFlipView.SelectedIndex++;
            }
            else
            {
                MyFlipView.SelectedIndex = 0;
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
            var vm = new MainViewModel();
            this.DataContext = vm;
            await vm.LoadData();
        }

        private void CheckRotation()
        {
            if(!rotate)
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
            timer.Start();
        }

        private void MyFlipView_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
