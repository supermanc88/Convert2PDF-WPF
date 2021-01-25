using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
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

                // DirectoryInfo info = new DirectoryInfo("C:\\Users\\CHM\\Google Drive\\1800大卡食谱.doc");
                // MessageBox.Show(info.ToString());
            }
            else
            {
                return;
            }

            if (inputDir.Text != "")
            {
                fileTreeView.ItemsSource = DirectoryTree.InitRootPath(inputDir.Text);
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
