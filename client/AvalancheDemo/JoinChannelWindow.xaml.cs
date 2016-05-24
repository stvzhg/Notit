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

namespace AvalancheDemo
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class JoinChannelWindow : Window
    {
        public string Channel { get; set; }
        public string User { get; set; }

        public Action<string, string> action;

        public JoinChannelWindow(Action<string, string> a)
        {
            InitializeComponent();
            action = a;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            action(Channel, User);
            this.Close();
        }
    }
}
