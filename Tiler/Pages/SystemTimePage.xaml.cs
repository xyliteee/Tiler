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
using TilerMain.MessageBox;

namespace TilerMain.Pages
{
    /// <summary>
    /// SystemTimePage.xaml 的交互逻辑
    /// </summary>
    public partial class SystemTimePage : Page
    {
        private int systemYear;
        private int systemMonth;
        private int systemDay;
        private int systemHour;
        private int systemMinute;
        private bool isEditing = false;
        public SystemTimePage()
        {
            InitializeComponent();
        }

        private void UpdataFromFlash() 
        {
            systemYear = InstanceGlobal.WholeDataPackage.SystemYear;
            systemMonth = InstanceGlobal.WholeDataPackage.SystemMonth;
            systemDay = InstanceGlobal.WholeDataPackage.SystemDay;
            systemHour = InstanceGlobal.WholeDataPackage.SystemHour;
            systemMinute = InstanceGlobal.WholeDataPackage.SystemMinute;
            Dispatcher.Invoke(() =>
            {
                if (systemHour < 12)
                {
                    HourRotateTransform.Angle = systemHour * 30 + (systemMinute/60.0)*30;
                }
                else 
                {
                    HourRotateTransform.Angle = (systemHour-12) * 30 + (systemMinute / 60.0) * 30;
                }
                MineteRotateTransform.Angle = systemMinute * 6;
                string hourString = systemHour.ToString();
                string minuteString = systemMinute.ToString();
                if (hourString.Length == 1) 
                {
                    hourString = "0"+ hourString;
                }
                if (minuteString.Length == 1) 
                {
                    minuteString = "0"+ minuteString;
                }
                TimeLable.Content = $"{hourString}:{minuteString}";
                DateLable.Content = $"{systemYear + 2000}年{systemMonth}月{systemDay}日";
            });
        }
        public void CheckTime() 
        {
            Task.Run(async () =>
            {
                while (InstanceGlobal.IsConnected && !isEditing)
                {
                    await InstanceGlobal.ReFreshPackage();
                    UpdataFromFlash();
                    await Task.Delay(1000);
                    if (!InstanceGlobal.IsConnected) break;
                }
            });
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            static bool IsLeapYear(int systemYear)
            {
                int year = systemYear + 2000;
                return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
            }

            static int DaysInMonth( int systemMonth,int systemYear)
            {
                return systemMonth switch
                {
                    2 => IsLeapYear(systemYear) ? 29 : 28,
                    4 or 6 or 9 or 11 => 30,
                    _ => 31,
                };
            }
            isEditing = true;
            Feedback feedback = TilerMessageBox.ShowMessageBoxWithInput("请按照如下格式修改时间\n年-月-日-时-分",$"{systemYear}-{systemMonth}-{systemDay}-{systemHour}-{systemMinute}");
            if (!feedback.IsYes) 
            {
                isEditing = false;
                CheckTime();
                return;
            }
            string input = feedback.Input;
            string[] parts = input.Split('-');
            string year;
            string month;
            string day;
            string hour;
            string minute;
            try
            {
                year = parts[0];
                month = parts[1];
                day = parts[2];
                hour = parts[3];
                minute = parts[4];
                int yearInt = int.Parse(year);
                int monthInt = int.Parse(month);
                int dayInt = int.Parse(day);
                int hourInt = int.Parse(hour);
                int minuteInt = int.Parse(minute);
                if (yearInt < 0 || yearInt > 99) { throw new Exception(); }
                if (monthInt <= 0 || monthInt > 12) { throw new Exception(); }
                if (dayInt <= 0 || dayInt > DaysInMonth(monthInt, yearInt)) { throw new Exception(); }
                if (hourInt < 0 || hourInt > 23) { throw new Exception(); }
                if(minuteInt < 0 || minuteInt > 59) { throw new Exception(); }
            }
            catch (Exception)
            {
                InstanceGlobal.ShowMessage("格式有误");
                isEditing = false;
                CheckTime();
                return;
            }

            InstanceGlobal.ShowMessage("设置中......");
            int code = await BluetoothContent.SendDataAsync([year, month, day, hour, minute]);
            bool isSuccessful = await BluetoothContent.GetPakcageSucessful();
            if (code == 1 || !isSuccessful)
            {
                InstanceGlobal.ShowMessage("数据传输出错");
                isEditing = false;
                CheckTime();
                return;
            }

            if (await InstanceGlobal.ReFreshPackage()) 
            {
                InstanceGlobal.ShowMessage("设定成功");
                isEditing = false;
                CheckTime();
            };
        }
    }
}
