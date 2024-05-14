using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
namespace TilerMain
{
    public static class InstanceGlobal//一些可能需要到处都用的实例就放这里进行静态调用
    {
        private static BluetoothClient _bluetoothClient;
        public static BluetoothClient BluetoothClient
        {
            get
            {
                return _bluetoothClient;
            }
            set
            {
                _bluetoothClient = value;
                if (_bluetoothClient != null && _bluetoothClient.Connected)//当客户端被赋值时直接自动获取信息流，就不再手动赋值了
                {
                    BluetoothStream = _bluetoothClient.GetStream();
                }
            }
        }
        public static NetworkStream BluetoothStream { get; private set; }

        public static BluetoothDeviceInfo BluetoothDeviceInfo { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static bool IsConnected { get; set; } = false;

        public static DataPackage WholeDataPackage { get; set; }
        public static void ShowMessage(string message) //重新包装一下消息框的显示方法，不然调用显得麻烦
        {
            MainWindow.ShowMessage(message);
        }

        public async static Task<bool> ReFreshPackage() 
        {
            int code = BluetoothContent.SendData(["0"]);
            if (code == 1)
            {
                ShowMessage("数据刷新出错");
                return false;
            }

            WholeDataPackage = new DataPackage(await BluetoothContent.ReceiveDataAsync());
            if (WholeDataPackage.IsError)
            {
                ShowMessage("数据出错");
                return false;
            }
            return true;
        }
        public static Thread CheckThread { get; set; }
        public static ManualResetEventSlim ResetEvent { get; set; } = new(true);
        public static void RefreshDataLoop()
        {
            CheckThread = new(async () =>
            {
                while (IsConnected)
                {
                    try 
                    {
                        if (!await ReFreshPackage()) 
                        {
                            throw new Exception();
                        };
                        ResetEvent.Wait();
                        MainWindow.systemTimePage.UpdataFromFlash();
                        //MainWindow.waterPage.UpdataFromFlash();
                        await Task.Delay(1000);
                        if (!IsConnected) break;
                    }
                    catch(Exception) 
                    {
                        IsConnected = false;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MainWindow.homePage.ScrollZone.Visibility = Visibility.Hidden;
                            MainWindow.homePage.ConnectedStateLable.Content = "======链接中断======";
                            ShowMessage("链接中断");
                            BitmapImage bitmapImage = new(new Uri("pack://application:,,,/TilerMain;component/Image/Icons/Disconnected.png"));
                            MainWindow.homePage.ConnectedImage.Source = bitmapImage;
                            MainWindow.ActionFrame.Navigate(MainWindow.homePage);
                            Animation.PageSilderMoveing(MainWindow.Silder, 9);
                            MainWindow.RefreshFrame();
                        });
                        break;
                    }
                }
            });
            CheckThread.Start();
        }
    }
}
