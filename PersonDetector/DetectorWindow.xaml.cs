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
        public DetectorWindow()
        {
            InitializeComponent();
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
            }
            else
                return;


        }

    }
}
