using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PersonDetector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // const int SPEECH_AMOUNT = 4;
        private int currentSpeech = 1;
        private SingleInput currentInput;
        private DispatcherTimer aTimer= new DispatcherTimer();
       
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
            currentInput = new SingleInput();

            aTimer.Tick  += new EventHandler(RefreshDebug);
            aTimer.Interval = TimeSpan.FromMilliseconds( Config.DEBUG_REFRESH_INTERVAL);
            aTimer.Start();

        }
        private  void RefreshDebug(object source, EventArgs e)
        {
            LnewLinesPerText.Content = currentInput.newLinesPerText;
            LspacesAfterPunctuation.Content = currentInput.spacesAfterPunctuation;
            LspacesBeforePunctuation.Content = currentInput.spacesBeforePunctuation;
            LpolishChars.Content = currentInput.polishChars;
            LavgLetterTime.Content = currentInput.avgLetterTime;
            LavgCapitalLetterTime.Content = currentInput.avgCapitalLetterTime;

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

        private void expanderDebug_Expanded(object sender, RoutedEventArgs e)
        {
            MainWindowForm.Height = 520;
        }

        private void expanderDebug_Collapsed(object sender, RoutedEventArgs e)
        {
            MainWindowForm.Height = 365;
        }
    }
}
