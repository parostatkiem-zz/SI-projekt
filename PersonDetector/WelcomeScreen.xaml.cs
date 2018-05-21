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
            Config.currentUserData.userName = textBoxNickname.Text;
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

        private void textBoxNickname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                button_Click(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxNickname.Focus();
        }

    

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.L)
            {
                DetectorWindow m = new DetectorWindow();
                m.Show();
                this.Hide();
            }
        }
    }
}
