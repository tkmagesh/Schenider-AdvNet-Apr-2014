using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace TaskParallelismDemo
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

        private void btnDoWork_Click(object sender, RoutedEventArgs e)
        {
            //displayMessage("Work Started");
            var task = new Task<string>(this.DoWork);
            task.ContinueWith((t) =>
            {
                DisplayMessage("Task successfuly completed at " + t.Result);
            },TaskScheduler.FromCurrentSynchronizationContext());
            task.Start();
            //this.DoWork();
            //this.DisplayMessage("Work Completed");
        }
        private string DoWork() {
            Debug.WriteLine("Work started");
            for (int i = 0; i < 100000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        
                    }
                }
            }
            Debug.WriteLine("Work done");
            return DateTime.Now.ToString();
        }

        private void DisplayMessage(string msg) {
            this.lblStatus.Content = msg;
        }
    }
}
