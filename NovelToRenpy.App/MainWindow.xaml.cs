using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovelToRenpy.App
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

        private void BtnOpenOutput_Click(object sender, RoutedEventArgs e)
        {
            var d1 = new FolderBrowserDialog();
            if (d1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.TbPathSave.Text = d1.SelectedPath;
            }

        }

        private void BtnOpenText_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d1 = new OpenFileDialog();
            if (d1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.TbFile.Text = d1.FileName;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            var file = this.TbFile.Text;
            var path = this.TbPathSave.Text;
            LogText("任务开始");
            Task task = Task.Run(()=> {
                var p = new BookProvider();
                var book = p.Get(file);
                var b = new RpyBuilder();
                b.WriteToFile(book.List);
            });

            task.ContinueWith((t)=> { LogText("任务完成"); });
        }

        private void LogText(string msg)
        {
            Dispatcher.Invoke(() => {
                this.TbLog.Text = msg;
            });
        }
    }
}
