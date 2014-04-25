using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncAwaitDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new WebClient();
            
            //var responseTask = client.DownloadStringTaskAsync("http://www.reddit.com");
            //this.lblResult.Content = "Retrieving data...";
            //var content = await responseTask;
            //this.lblResult.Content = content.Length.ToString();

            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync("http://www.reddit.com");
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            this.lblResult.Content = e.Result.Length;
        }
    }
}
