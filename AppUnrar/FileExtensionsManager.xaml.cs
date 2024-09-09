using System.Windows;
using AppUnrar.Model;

namespace AppUnrar
{
    /// <summary>
    /// Interaction logic for FileExtensionsManager.xaml
    /// </summary>
    public partial class FileExtensionsManager : Window
    {
        public FileExtensionsManager()
        {
            InitializeComponent();
        }

        private void btnAddFileExtension_Click(object sender, RoutedEventArgs e)
        {
            FileExtensionsManager_Add fileExtensionsManager = new FileExtensionsManager_Add();
            fileExtensionsManager.Owner = this;
            fileExtensionsManager.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fileExtensionsManager.ShowDialog();
        }

        private void btnDeleteFileExtension_Click(object sender, RoutedEventArgs e)
        {
            if (lstb_file_extensions.SelectedItem != null)
            {
                AppData.FileExtensionsToExtract?.Remove(lstb_file_extensions.SelectedItem.ToString());
                Presenter.AppDataView.appdata_save_changes();
            }
            else
            {
                MessageBox.Show("Selected a password from the list");
            }
        }
    }
}
