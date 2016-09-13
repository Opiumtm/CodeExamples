using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BufferMemoryLeak
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void UpdateAppMemoryUsage()
        {
            var rep = MemoryManager.GetAppMemoryReport();
            var privateMb = rep.PrivateCommitUsage/(1024.0 * 1024.0);
            var totalMb = rep.TotalCommitUsage / (1024.0 * 1024.0);
            PrivateCommit = $"{privateMb:F2} Mb";
            TotalCommit = $"{totalMb:F2} Mb";
        }

        public static readonly DependencyProperty PrivateCommitProperty = DependencyProperty.Register(
            "PrivateCommit", typeof (string), typeof (MainPage), new PropertyMetadata(default(string)));

        public string PrivateCommit
        {
            get { return (string) GetValue(PrivateCommitProperty); }
            set { SetValue(PrivateCommitProperty, value); }
        }

        public static readonly DependencyProperty TotalCommitProperty = DependencyProperty.Register(
            "TotalCommit", typeof (string), typeof (MainPage), new PropertyMetadata(default(string)));

        public string TotalCommit
        {
            get { return (string) GetValue(TotalCommitProperty); }
            set { SetValue(TotalCommitProperty, value); }
        }

        private async void RunButton_OnClick(object sender, RoutedEventArgs e)
        {
            RunButton.IsEnabled = false;
            for (int i = 0; i < 10; i++)
            {
                await LeakyExample.Run();
                UpdateAppMemoryUsage();
            }
            RunButton.IsEnabled = true;
        }

        private void GetStatsButton_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateAppMemoryUsage();
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateAppMemoryUsage();
        }

        private void GcButton_OnClick(object sender, RoutedEventArgs e)
        {
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced, true);
            UpdateAppMemoryUsage();
        }
    }
}
