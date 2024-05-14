using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace TilerMain.Pages
{
    /// <summary>
    /// WaterPage.xaml 的交互逻辑
    /// </summary>
    public partial class WaterPage : Page
    {
        private int foodThreshold = 0;
        private int isAuto = 2;//1是自动启用
        public WaterPage()
        {
            InitializeComponent();
        }

        public void UpdataFromFlash() 
        {
            Dispatcher.Invoke(() => 
            {
                WaterSlider.Value = InstanceGlobal.WholeDataPackage.FoodThreshold;
                isAuto = InstanceGlobal.WholeDataPackage.FoodAuto;
                if (isAuto == 2)
                {
                    AutoLable.Content = "自动：关";
                    Animation.RightMoving(BorderCirlce, null, 53);
                }
                else if (isAuto == 1)
                {
                    AutoLable.Content = "自动：开";
                    Animation.RightMoving(BorderCirlce, null, 3);
                }
            });
        }

        private async void DeterminationButton_Click(object sender, RoutedEventArgs e)
        {
            InstanceGlobal.ShowMessage("设置中......");
            int code = await BluetoothContent.SendDataAsync([foodThreshold.ToString(), isAuto.ToString()]);
            
            bool isSuccessful = await BluetoothContent.GetPakcageSucessful();
            if (code == 1 || !isSuccessful)
            {
                InstanceGlobal.ShowMessage("数据传输出错");
                return;
            }
            InstanceGlobal.ShowMessage("设定成功");
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foodThreshold = (int)WaterSlider.Value;
            WaterLable.Content = $"{foodThreshold*10}克";
        }

        private void AutoClick_Click(object sender, RoutedEventArgs e)
        {
            if (isAuto == 1) 
            {
                isAuto = 2;
                AutoLable.Content = "自动：关";
                Animation.RightMoving(BorderCirlce, null,53);
            }
            else if (isAuto == 2)
            {
                isAuto = 1;
                AutoLable.Content = "自动：开";
                Animation.RightMoving(BorderCirlce, null, 3);
            }
        }
    }
}
