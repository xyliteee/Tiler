using System.Windows;
using System.IO.Ports;
using System.Diagnostics;
using TilerMain.Pages;
using System.Windows.Automation;


namespace TilerMain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly HomePage homePage;
        public readonly WaterPage waterPage;
        public readonly SystemTimePage systemTimePage;
        public MainWindow()
        {
            InitializeComponent();
            homePage = new();
            waterPage = new();
            systemTimePage = new();
            ActionFrame.Navigate(homePage);
            InstanceGlobal.MainWindow = this;
        }

        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            InstanceGlobal.BluetoothClient?.Close();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        public void RefreshFrame() 
        {
            Animation.FrameMoving(ActionFrame, -50);
            Animation.ChangeOP(ActionFrame, 0, 1, 0.2);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActionFrame.Content != homePage) 
            {
                ActionFrame.Navigate(homePage);
                Animation.PageSilderMoveing(Silder, 9);
                RefreshFrame();
            }
        }

        private void WaterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActionFrame.Content != waterPage && InstanceGlobal.IsConnected)
            {
                ActionFrame.Navigate(waterPage);
                
                Animation.PageSilderMoveing(Silder, 59);
                RefreshFrame();
            }
        }

        public void ShowMessage(string message) 
        {
            Dispatcher.Invoke(async() => 
            {
                Animation.ChangeOP(MessageBox, null, 1, 0.2);
                Animation.RightMoving(MessageBox, -200, 0);
                MessageboxLable.Content = message;
                await Task.Delay(2000);
                Animation.ChangeOP(MessageBox, null, 0, 0.2);
            });
            
        }

        private void SystemTimeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActionFrame.Content != systemTimePage && InstanceGlobal.IsConnected)
            {
                ActionFrame.Navigate(systemTimePage);

                Animation.PageSilderMoveing(Silder, 109);
                RefreshFrame();
            }
        }
    }
}