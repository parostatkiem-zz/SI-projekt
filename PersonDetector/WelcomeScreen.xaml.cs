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
    /// Interaction logic for WelcomeScreen.xaml
    /// </summary>
    public partial class WelcomeScreen : Window
    {
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateName())
            {
               
                textBoxNickname.BorderBrush = Brushes.Red;
                textBoxNickname.BorderThickness = new Thickness(2);
                return;
            }
            Config.userData.userName = textBoxNickname.Text;
            MainWindow m = new MainWindow();
            m.Show();
            this.Hide();
        }
        private bool ValidateName()
        {
            string name = textBoxNickname.Text;
            if (name == string.Empty) return false;
            if (name.Length > 25) return false;
            if (name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0) return false;
            return true;
        }
    }
}
