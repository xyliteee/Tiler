using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilerMain
{
    public class DataPackage
    {
        public int SystemYear;
        public int SystemMonth;
        public int SystemDay;
        public int SystemHour;
        public int SystemMinute;
        public int FoodThreshold;
        public int FoodAuto;
        public int Hour1;
        public int Minite1;
        public int Second1;
        public int Hour2;
        public int Minite2;
        public int Second2;
        public int Hour3;
        public int Minite3;
        public int Second3;
        public bool IsError = false;

        public DataPackage(byte[] bytes)
        {
            DataInit(bytes);
        }
        private void DataInit(byte[] bytes) 
        {
            SystemYear = bytes[0];
            if(SystemYear>=99) { IsError = true; InstanceGlobal.ShowMessage("年份出错");return; }

            SystemMonth = bytes[1];
            if (SystemMonth > 12 || SystemMonth == 0) { IsError = true; InstanceGlobal.ShowMessage("月份出错"); return; }

            SystemDay = bytes[2];
            if (SystemDay > DaysInMonth() || SystemDay == 0) { IsError = true; InstanceGlobal.ShowMessage("日期出错"); return; }

            SystemHour = bytes[3];
            if (SystemHour > 23) { IsError = true; InstanceGlobal.ShowMessage("系统时出错"); return; }

            SystemMinute = bytes[4];
            if (SystemMinute > 59) { IsError = true; InstanceGlobal.ShowMessage("系统分出错"); return; }

            FoodThreshold = bytes[5];
            if (FoodThreshold > 30) { IsError = true; InstanceGlobal.ShowMessage("食物阈值出错"); return; }

            FoodAuto = bytes[6];
            if (FoodAuto > 2) { IsError = true; InstanceGlobal.ShowMessage("自动标识出错"); return; }

            Hour1 = bytes[7];
            if (Hour1 > 23) { IsError = true; InstanceGlobal.ShowMessage("喂食1 时出错"); return; }

            Minite1 = bytes[8];
            if (Minite1 > 59) { IsError = true; InstanceGlobal.ShowMessage("喂食1 分出错"); return; }

            Second1 = bytes[9];
            if (Second1 > 59) { IsError = true; InstanceGlobal.ShowMessage("喂食1 秒出错"); return; }

            Hour2 = bytes[10];
            if (Hour2 > 23) { IsError = true; InstanceGlobal.ShowMessage("喂食2 时出错"); return; }

            Minite2 = bytes[11];
            if (Minite2 > 59) { IsError = true; InstanceGlobal.ShowMessage("喂食2 分出错"); return; }

            Second2 = bytes[12];
            if (Second2 > 59) { IsError = true; InstanceGlobal.ShowMessage("喂食2 秒出错"); return; }

            Hour3 = bytes[13];
            if (Hour3 > 23) { IsError = true; InstanceGlobal.ShowMessage("喂食3 时出错"); return; }

            Minite3 = bytes[14];
            if (Minite3 > 59) { IsError = true; InstanceGlobal.ShowMessage("喂食3 分出错"); return; }

            Second3 = bytes[15];
            if (Second3 > 59) { IsError = true; InstanceGlobal.ShowMessage("喂食3 ，秒出错"); return; }

        }
        private bool IsLeapYear()
        {
            int year = SystemYear + 2000;
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        private int DaysInMonth()
        {
            return SystemMonth switch
            {
                2 => IsLeapYear() ? 29 : 28,
                4 or 6 or 9 or 11 => 30,
                _ => 31,
            };
        }
    }

}
