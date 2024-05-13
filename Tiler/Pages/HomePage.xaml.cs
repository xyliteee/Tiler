using HandyControl.Controls;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.PortableExecutable;
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

namespace TilerMain.Pages
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {

        public HomePage()
        {
            InitializeComponent();
            ScrollZone.Visibility = Visibility.Hidden;
        }

        private async void OpenPort(object sender, RoutedEventArgs e)
        {
            if (BluetoothRadio.PrimaryRadio == null)
            {
                InstanceGlobal.ShowMessage("蓝牙功能未启用");
                return;
            }

            Button button = (Button)sender;
            button.IsEnabled = false;
            ConnectedStateLable.Content = "======正在链接======";
            BluetoothDeviceInfo bluetoothDeviceInfo = (BluetoothDeviceInfo)button.Tag;
            BluetoothClient client = new();
            
            await Task.Run(() =>
            {
                try
                {
                    client.Connect(bluetoothDeviceInfo.DeviceAddress, BluetoothService.SerialPort);
                }
                catch (Exception es)
                {
                    Dispatcher.Invoke(() =>
                    {
                        string errorMessage = string.Empty;
                        if (es.Message.Contains("主机没有反应")) errorMessage = $"设备[{bluetoothDeviceInfo.DeviceName}]未响应：该设备被占用或未开启";
                        else if (es.Message.Contains("该请求的地址无效")) errorMessage = $"设备[{bluetoothDeviceInfo.DeviceName}]请求地址无效：该设备可能不需要被链接";
                        else errorMessage = es.Message;
                        Debug.WriteLine(es.Message);
                        ConnectedStateLable.Content = errorMessage;
                        button.IsEnabled = true;
                    });
                }
            });
            if (client.Connected)
            {
                ScrollZone.Visibility = Visibility.Hidden;
                ConnectedStateLable.Content = "======链接成功======";
                InstanceGlobal.BluetoothClient = client;
                InstanceGlobal.BluetoothDeviceInfo = bluetoothDeviceInfo;
                InstanceGlobal.IsConnected = true;
                BitmapImage bitmapImage = new(new Uri("pack://application:,,,/TilerMain;component/Image/Icons/Connected.png"));
                ConnectedButton.Content = "断开链接";
                ConnectedImage.Source = bitmapImage;
                CheckConnection();
                InstanceGlobal.ShowMessage("正在获取数据");

                if (await InstanceGlobal.ReFreshPackage()) 
                {
                    InstanceGlobal.MainWindow.waterPage.UpdataFromFlash();
                    InstanceGlobal.MainWindow.systemTimePage.CheckTime();
                    InstanceGlobal.ShowMessage("数据获取完成");
                }
            }
        }

        private void ConnectedButton_Click(object sender, RoutedEventArgs e)
        {
            ScanBlueToothDevices();
        }
        private async void ScanBlueToothDevices()
        {
            if (BluetoothRadio.PrimaryRadio == null) 
            {
                InstanceGlobal.ShowMessage("蓝牙功能未启用");
                return;
            }

            if (InstanceGlobal.IsConnected)
            {
                InstanceGlobal.BluetoothClient.Close();
                ConnectedButton.Content = "点击此处开始扫描";
            }
            else 
            {
                BluetoothClient client = new();
                ScrollCanvas.Children.Clear();
                ScrollZone.Visibility = Visibility.Visible;
                ConnectedButton.IsEnabled = false;
                ConnectedButton.Opacity = 0.5;
                ConnectedStateLable.Content = $"======正在执行扫描======";
                LoadingCircle.Visibility = Visibility.Visible;

                await Task.Delay(1000);
                BluetoothDeviceInfo[] bluetoothDeviceInfos = await Task.Run(() => client.DiscoverDevices(255, true, true, false));

                for (int index = 0; index < bluetoothDeviceInfos.Length; index++)
                {
                    CreatSingleCard(index, bluetoothDeviceInfos[index]);
                }

                ConnectedStateLable.Content = $"======扫描完成======";
                LoadingCircle.Visibility = Visibility.Hidden;
                ConnectedButton.IsEnabled = true;
                ConnectedButton.Opacity = 1;
            }
        }


        private void CheckConnection()
        {
            new Thread(() =>
            {
                try 
                {
                    while (true)
                    {
                        try
                        {
                            InstanceGlobal.BluetoothStream.Write([0x01]);
                        }
                        catch
                        {
                            InstanceGlobal.IsConnected = false;
                            Dispatcher.Invoke(() =>
                            {
                                ScrollZone.Visibility = Visibility.Hidden;
                                ConnectedStateLable.Content = "======链接中断======";
                                InstanceGlobal.ShowMessage("链接中断");
                                BitmapImage bitmapImage = new(new Uri("pack://application:,,,/TilerMain;component/Image/Icons/Disconnected.png"));
                                ConnectedImage.Source = bitmapImage;
                                InstanceGlobal.MainWindow.ActionFrame.Navigate(this);
                                Animation.PageSilderMoveing(InstanceGlobal.MainWindow.Silder, 9);
                                InstanceGlobal.MainWindow.RefreshFrame();
                            });
                            break;
                        }
                        Thread.Sleep(200);
                    }
                } 
                catch(TaskCanceledException) { }
            }).Start();
        }


        private void CreatSingleCard(int index, BluetoothDeviceInfo bluetoothDeviceInfo) 
        {
            Dispatcher.Invoke(() => 
            {
                Canvas canvas = new()
                {
                    Width = 690,
                    Height = 80,
                    Margin = new Thickness(10)
                };

                Border border1 = new()
                {
                    Height = 80,
                    Width = 690,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666")),
                    CornerRadius = new CornerRadius(5)
                };
                canvas.Children.Add(border1);

                Border border2 = new()
                {
                    Width = 60,
                    Height = 60,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#222222")),
                    CornerRadius = new CornerRadius(5)
                };
                Canvas.SetLeft(border2, 10);
                Canvas.SetTop(border2, 10);
                canvas.Children.Add(border2);

                Image image = new()
                {
                    Height = 48,
                    Width = 48,
                    Source = new BitmapImage(new Uri("/Image/Icons/Bluetooth.png", UriKind.Relative))
                };
                border2.Child = image;

                Label label = new()
                {
                    BorderThickness = new Thickness(0),
                    Background = Brushes.Transparent,
                    Content = $"Name:{bluetoothDeviceInfo.DeviceName}",
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#222222")),
                    FontSize = 22,
                    Height = 80
                };
                Canvas.SetLeft(label, 80);
                canvas.Children.Add(label);

                Button button = new()
                {
                    Width = 100,
                    Height = 40,
                    Content = "链接",
                    FontSize = 22,
                    Style = (Style)FindResource("NormalButton2"),
                    Tag = bluetoothDeviceInfo
                };
                button.Click += OpenPort;
                Canvas.SetRight(button, 10);
                Canvas.SetTop(button, 20);
                canvas.Children.Add(button);
                Canvas.SetTop(canvas, 10 + 90 * index);
                ScrollCanvas.Children.Add(canvas);

            });
            
        }
    }
}
