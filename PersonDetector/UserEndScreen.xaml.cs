using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PersonDetector
{
    /// <summary>
    /// Interaction logic for UserEndScreen.xaml
    /// </summary>
    public partial class UserEndScreen : Window
    {
        public UserEndScreen()
        {
            InitializeComponent();
        }

        private void btnEndTest_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
