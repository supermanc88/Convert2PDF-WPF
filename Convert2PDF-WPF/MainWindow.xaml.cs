using System;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Convert2PDF_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputOpenButtonClick(object sender, RoutedEventArgs e)
        {
            FolderSelectDialog dialog = new FolderSelectDialog();

            if (dialog.ShowDialog())
            {
                string foldPath = dialog.FileName;
                
                outputDir.Text = foldPath;
                inputDir.Text = foldPath;
            }
            else
            {
                return;
            }
        }

        private void OutputOpenButtonClick(object sender, RoutedEventArgs e)
        {
            FolderSelectDialog dialog = new FolderSelectDialog();

            if (dialog.ShowDialog())
            {
                string foldPath = dialog.FileName;
                
                outputDir.Text = foldPath;
            }
            else
            {
                return;
            }
        }
    }
}
