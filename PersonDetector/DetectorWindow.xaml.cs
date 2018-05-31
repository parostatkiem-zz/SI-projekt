using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for DetectorWindow.xaml
    /// </summary>
    public partial class DetectorWindow : Window
    {
        private bool weightsFileLoaded = false;
        public DetectorWindow()
        {
            InitializeComponent();
            weightsFileLoaded = IOoperations.ReadWeightsFrom(Config.weightsFilePath);
            RefreshDebug();
            lFirstPerson.Content = "";
            lSecondPerson.Content = "";
            lDesc5.Visibility = Visibility.Collapsed;
            listBoxNames.Visibility = Visibility.Collapsed;
            IOoperations.AIdataFromJSON(Config.aiDataFilePath);
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.SelectedPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string path;
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
               path = dialog.SelectedPath;
               IOoperations.ReadFilesFrom(path);
                RefreshDebug();

            }
            else
                return;


        }

        private void btnDoFinalTest_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow(true);
            m.Show();
            m.Closed += M_Closed;
        }
        private void RefreshDebug()
        {
            lFinalTestDone.Content = (Config.allUsersData.Where(p => p.userName == "FINAL").Count() > 0).ToString();
            lLoadedSets.Content = Config.parsedFiles;
            lNotLoadedSets.Content = Config.unParsedFiles;
            lWeightsLoaded.Content = weightsFileLoaded;
        }
        private void M_Closed(object sender, EventArgs e)
        {
            RefreshDebug();
        }

        private void btnDoMagic_Click(object sender, RoutedEventArgs e)
        {
            if (Config.allUsersData.Where(p => p.userName == "FINAL").Count() <= 0) return;
            DataAnalytics.NormalizeData();
            SudczakClassifier.Classify(Config.allUsersNormalized, Config.allUsersClassified, "FINAL");

            Config.allUsersClassified= Config.allUsersClassified.OrderBy(u => u.probability).Reverse().ToList();

            lFirstPerson.Content = Config.allUsersClassified[0].userName + " na " + Math.Round(Config.allUsersClassified[0].probability * 100,2) + "%";
            lSecondPerson.Content = Config.allUsersClassified[1].userName + " na " + Math.Round(Config.allUsersClassified[1].probability * 100, 2) + "%";

            listBoxNames.Items.Clear();

            foreach(UserData d in Config.allUsersClassified)
            {
                listBoxNames.Items.Add(d);
            }

            lDesc5.Visibility = Visibility.Visible;
            listBoxNames.Visibility = Visibility.Visible;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                string actualRightPerson = listBoxNames.SelectedItem.ToString();
                if (actualRightPerson == null) return;

                IOoperations.AIdataToJSON(actualRightPerson);

            }
            catch { }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
