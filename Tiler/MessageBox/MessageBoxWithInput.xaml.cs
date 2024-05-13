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
using System.Windows.Shapes;

namespace TilerMain.MessageBox
{
    /// <summary>
    /// MessageBoxWithInput.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxWithInput : Window
    {
        public Feedback Feedback;
        public MessageBoxWithInput(Feedback feedback)
        {
            Feedback = feedback;
            InitializeComponent();
        }

        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            Feedback.IsCancle = true;
            Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Feedback.IsYes = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Feedback.IsYes = false;
            Close();
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Feedback.Input = textBox.Text;
        }
    }
    
}
