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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonDetector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // const int SPEECH_AMOUNT = 4;
        private int currentSpeech = 1;
        private int CurrentSpeech
        {
            get { return currentSpeech; }
            set
            {
                if(value==Config.SPEECH_AMOUNT)
                {
                    //koniec testu
                    buttonNextSpeech.Content = "Zakończ test";
                    buttonNextSpeech.Click -= buttonNextSpeech_Click;
                    buttonNextSpeech.Click += finishTest;
                }
                currentSpeech = value;
                labelSpeechNumber.Content = value;
               richTextBox.Document.Blocks.Clear();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            CurrentSpeech = 1;
        }

        private void buttonNextSpeech_Click(object sender, RoutedEventArgs e)
        {
            CurrentSpeech++;
        }
        private void finishTest(object sender, RoutedEventArgs e)
        {
            //TODO: okienko zamykania
            UserEndScreen s = new UserEndScreen();
            s.Show();
            this.Hide();
        }
    }
}
