using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TilerMain.MessageBox;

namespace TilerMain
{
    public class Feedback
    {
        public bool IsYes = false;
        public bool IsCancle = false;
        public string Input = string.Empty;
    }
    public static class TilerMessageBox
    {
        public static Feedback ShowMessageBoxWithInput(string message,string example)
        {
            Feedback feedback = new();
            MessageBoxWithInput messageBoxWithInput = new(feedback)
            {
                Owner = InstanceGlobal.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Feedback = feedback,
            };
            messageBoxWithInput.TextLable.Text = message;
            messageBoxWithInput.InputBox.Text = example;
            messageBoxWithInput.ShowDialog();
            return feedback;
        }
    }
}
