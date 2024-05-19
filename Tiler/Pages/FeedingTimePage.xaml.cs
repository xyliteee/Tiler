using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;

namespace TilerMain.Pages
{
    /// <summary>
    /// FeedingTimePage.xaml 的交互逻辑
    /// </summary>
    public partial class FeedingTimePage : Page
    {
        private int hour1;
        private int hour2;
        private int hour3;
        private int minute1;
        private int minute2;
        private int minute3;
        private int second1;
        private int second2;
        private int second3;
        public FeedingTimePage()
        {
            InitializeComponent();
        }

        public void UpdataFromFlash()
        {
            hour1 = InstanceGlobal.WholeDataPackage.Hour1;
            hour2 = InstanceGlobal.WholeDataPackage.Hour2;
            hour3 = InstanceGlobal.WholeDataPackage.Hour3;
            minute1 = InstanceGlobal.WholeDataPackage.Minite1;
            minute2 = InstanceGlobal.WholeDataPackage.Minite2;
            minute3 = InstanceGlobal.WholeDataPackage.Minite3;
            second1 = InstanceGlobal.WholeDataPackage.Second1;
            second2 = InstanceGlobal.WholeDataPackage.Second2;
            second3 = InstanceGlobal.WholeDataPackage.Second3;
            UpdataUI();
        }

        private void UpdataUI() 
        {
            string[] timeString = [hour1.ToString(),hour2.ToString(),hour3.ToString(),minute1.ToString(), minute2.ToString(), minute3.ToString(),second1.ToString(), second2.ToString(), second3.ToString()];
            for (int index = 0;index <timeString.Length;index++) 
            {
                if (timeString[index].Length == 1) 
                {
                    timeString[index] = "0" + timeString[index];
                }
            }
            Dispatcher.Invoke(() => 
            {
                Time1.Content = $"喂食时间#1  {timeString[0]}:{timeString[3]}:{timeString[6]}";
                Time2.Content = $"喂食时间#2  {timeString[1]}:{timeString[4]}:{timeString[7]}";
                Time3.Content = $"喂食时间#3  {timeString[2]}:{timeString[5]}:{timeString[8]}";
            });
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            var feedback = TilerMessageBox.ShowMessageBoxWithInput("请输如喂食时间#1将要修改的时间", Time1.Content.ToString()!.Replace("喂食时间#1  ",""));
            if (!feedback.IsYes) return;
            string input = feedback.Input;
            string[] parts = input.Split(':');
            string hour;
            string minute;
            string second;
            try 
            {
                hour = parts[0];
                minute = parts[1];
                second = parts[2];
                int hourInt = int.Parse(hour);
                int minuteInt = int.Parse(minute);
                int secondInt = int.Parse(second);
                if (hourInt < 0 || hourInt > 23) { throw new Exception(); }
                if (minuteInt < 0 || minuteInt > 59) { throw new Exception(); }
                if (secondInt < 0 || secondInt > 59) { throw new Exception(); }
            } catch 
            {
                InstanceGlobal.ShowMessage("格式有误");
                return;
            };

            InstanceGlobal.ShowMessage("设置中......");

            int code = await BluetoothContent.SendDataAsync([hour, minute, second, hour2.ToString(), minute2.ToString(),second2.ToString(),hour3.ToString(),minute3.ToString(),second3.ToString()]);
            bool isSuccessful = await BluetoothContent.GetPakcageSucessful();
            if (code == 1 || !isSuccessful)
            {
                InstanceGlobal.ShowMessage("数据传输出错");
                return;
            }

            InstanceGlobal.ShowMessage("设定成功");
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            var feedback = TilerMessageBox.ShowMessageBoxWithInput("请输如喂食时间#1将要修改的时间", Time1.Content.ToString()!.Replace("喂食时间#1  ", ""));
            if (!feedback.IsYes) return;
            string input = feedback.Input;
            string[] parts = input.Split(':');
            string hour;
            string minute;
            string second;
            try
            {
                hour = parts[0];
                minute = parts[1];
                second = parts[2];
                int hourInt = int.Parse(hour);
                int minuteInt = int.Parse(minute);
                int secondInt = int.Parse(second);
                if (hourInt < 0 || hourInt > 23) { throw new Exception(); }
                if (minuteInt < 0 || minuteInt > 59) { throw new Exception(); }
                if (secondInt < 0 || secondInt > 59) { throw new Exception(); }
            }
            catch
            {
                InstanceGlobal.ShowMessage("格式有误");
                return;
            };

            InstanceGlobal.ShowMessage("设置中......");

            int code = await BluetoothContent.SendDataAsync([hour1.ToString(), minute1.ToString(), second1.ToString(), hour,minute,second, hour3.ToString(), minute3.ToString(), second3.ToString()]);
            bool isSuccessful = await BluetoothContent.GetPakcageSucessful();
            if (code == 1 || !isSuccessful)
            {
                InstanceGlobal.ShowMessage("数据传输出错");
                return;
            }

            InstanceGlobal.ShowMessage("设定成功");
        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            var feedback = TilerMessageBox.ShowMessageBoxWithInput("请输如喂食时间#1将要修改的时间", Time1.Content.ToString()!.Replace("喂食时间#1  ", ""));
            if (!feedback.IsYes) return;
            string input = feedback.Input;
            string[] parts = input.Split(':');
            string hour;
            string minute;
            string second;
            try
            {
                hour = parts[0];
                minute = parts[1];
                second = parts[2];
                int hourInt = int.Parse(hour);
                int minuteInt = int.Parse(minute);
                int secondInt = int.Parse(second);
                if (hourInt < 0 || hourInt > 23) { throw new Exception(); }
                if (minuteInt < 0 || minuteInt > 59) { throw new Exception(); }
                if (secondInt < 0 || secondInt > 59) { throw new Exception(); }
            }
            catch
            {
                InstanceGlobal.ShowMessage("格式有误");
                return;
            };

            InstanceGlobal.ShowMessage("设置中......");

            int code = await BluetoothContent.SendDataAsync([hour1.ToString(), minute1.ToString(), second1.ToString(), hour2.ToString(), minute2.ToString(), second2.ToString(), hour, minute, second,]);
            bool isSuccessful = await BluetoothContent.GetPakcageSucessful();
            if (code == 1 || !isSuccessful)
            {
                InstanceGlobal.ShowMessage("数据传输出错");
                return;
            }

            InstanceGlobal.ShowMessage("设定成功");
        }
    }
}
