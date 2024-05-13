using System;
using System.Collections.Generic;
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
            if(SystemYear>=99) { IsError = true; return; }

            SystemMonth = bytes[1];
            if (SystemMonth > 12 || SystemMonth == 0) { IsError = true;return; }

            SystemDay = bytes[2];
            if (SystemDay > DaysInMonth() || SystemDay == 0) { IsError = true; return; }

            SystemHour = bytes[3];
            if (SystemHour > 23) { IsError = true; return; }

            SystemMinute = bytes[4];
            if (SystemMinute > 59) { IsError = true; return; }

            FoodThreshold = bytes[5];
            if (FoodThreshold > 30) { IsError = true; return; }

            FoodAuto = bytes[6];
            if (FoodAuto > 2) { IsError = true; return; }

            Hour1 = bytes[7];
            if (Hour1 > 23) { IsError = true; return; }

            Minite1 = bytes[8];
            if (Minite1 > 59) { IsError = true; return; }

            Second1 = bytes[9];
            if (Second1 > 59) { IsError = true; return; }

            Hour2 = bytes[10];
            if (Hour2 > 23) { IsError = true; return; }

            Minite2 = bytes[11];
            if (Minite2 > 59) { IsError = true; return; }

            Second2 = bytes[12];
            if (Second2 > 59) { IsError = true; return; }

            Hour3 = bytes[13];
            if (Hour3 > 23) { IsError = true; return; }

            Minite3 = bytes[14];
            if (Minite3 > 59) { IsError = true; return; }

            Second3 = bytes[15];
            if (Second3 > 59) { IsError = true; return; }

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
