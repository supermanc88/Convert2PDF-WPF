using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
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
            this.Title = "Convert2PDF V3.0";
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

        private int ForeachTreeViewItems(ObservableCollection<DirectoryTree> dtsArg, bool isConvert)
        {
            // ObservableCollection<DirectoryTree> dts = (ObservableCollection<DirectoryTree>)fileTreeView.ItemsSource;
            int docNum = 0;
            ObservableCollection<DirectoryTree> dts = dtsArg;
            if (dts == null)
                return 0;

            foreach (DirectoryTree dt in dts)
            {
                // MessageBox.Show(dt.Info.FullName);
                // MessageBox.Show(dt.Info.Name);
                // MessageBox.Show(dt.Info.Extension);
                // if ((bool) dt.IsChecked)
                // {
                //     MessageBox.Show("checked");
                // }

                if ((dt.Info.Extension == ".docx" || dt.Info.Extension == ".doc") && (bool)dt.IsChecked)
                {
                    docNum++;

                    if (isConvert)
                    {
                        // 这里写转换流程
                        string pdfName = Path.GetFileNameWithoutExtension(dt.Info.FullName) + ".pdf";
                        var application  = new Microsoft.Office.Interop.Word.Application();
                        string pdfPath = Path.GetDirectoryName(dt.Info.FullName) + "\\" + pdfName;
                        // MessageBox.Show(dt.Info.FullName);
                        Microsoft.Office.Interop.Word.Document office_document = null;

                        try
                        {
                            application.Visible = false;
                            office_document = application.Documents.Open(dt.Info.FullName);
                            office_document.ExportAsFixedFormat(pdfPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF, CreateBookmarks: Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateHeadingBookmarks);
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err.Message);
                        }
                        finally
                        {
                            office_document.Close();
                            // convertProcessBar.Dispatcher.Invoke(() => { convertProcessBar.Value = 1; });
                            // convertProcessBar.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Background);
                        }
                    }

                }

                docNum += ForeachTreeViewItems(dt.dirs, isConvert);
            }

            return docNum;
        }


        public void ConvertThread()
        {
            int docNum = ForeachTreeViewItems((ObservableCollection<DirectoryTree>)fileTreeView.ItemsSource, false);
            // MessageBox.Show("111111");
            // convertProcessBar.Dispatcher.Invoke(() => { convertProcessBar.Value = 1; });
            // convertProcessBar.Dispatcher.Invoke(() => { convertProcessBar.Minimum = 1; });
            // convertProcessBar.Dispatcher.Invoke(() => { convertProcessBar.Maximum = docNum + 1; });


            docNum = ForeachTreeViewItems((ObservableCollection<DirectoryTree>)fileTreeView.ItemsSource, true);

            MessageBox.Show("转换完成");
            
        }

        private void ConvertButtonClicked(object sender, RoutedEventArgs e)
        {

            string docDirPath = outputDir.Text;
            string pdfDirPath = inputDir.Text;

            // 检查路径有效
            if (!Directory.Exists(docDirPath))
            {
                MessageBox.Show("文档目录不存在");
                return;
            }

            if (!Directory.Exists(pdfDirPath))
            {
                MessageBox.Show("保存目录不存在");
                return;
            }

            Thread th = new Thread(ConvertThread);
            th.Start();
        }
    }
}
