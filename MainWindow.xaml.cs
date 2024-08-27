using AppUnrar.Model;
using Microsoft.Win32;
using SharpCompress.Archives;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace AppUnrar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Presenter.AppDataView.appdata_fetch_json();
            AppData.Files_List = new ObservableCollection<Model.File>();

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Multiselect = true;
            ofd.Title = "Select the Compressed Files";
            ofd.Filter = "Compressed Files(*.RAR;*.ZIP;*.7Z)|*.RAR;*.ZIP;*.7Z|All files (*.*)|*.*";
            ofd.ShowDialog();

            Presenter.ViewHandler.fetch_files_info(ofd.FileNames, dg_file_list);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            DecompressionWindow decompressionWindow = new DecompressionWindow(cb_DecompressOnlySelectedFileExtensions.IsChecked);
            decompressionWindow.Show();
            decompressionWindow.test();
        }

        private void btnClearSelection_Click(object sender, RoutedEventArgs e)
        {
            Presenter.ViewHandler.clear_file_list();
        }

        private void btnOutputLocation_Click(object sender, RoutedEventArgs e)
        {
            var file_explorer_process = new ProcessStartInfo();
            file_explorer_process.FileName = @"c:\windows\explorer.exe";
            file_explorer_process.Arguments = @".\output";
            Process.Start(file_explorer_process);
        }

        private void btnManagePws_Click(object sender, RoutedEventArgs e)
        {
            PasswordManager passwordManager = new PasswordManager();
            passwordManager.Owner = this;
            passwordManager.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            passwordManager.ShowDialog();
        }

        private void btnManageFileExtensions_Click(object sender, RoutedEventArgs e)
        {
            FileExtensionsManager fileExtensionsManager = new FileExtensionsManager();
            fileExtensionsManager.Owner = this;
            fileExtensionsManager.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fileExtensionsManager.ShowDialog();
        }
    }
}