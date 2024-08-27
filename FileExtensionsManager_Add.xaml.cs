using System.Windows;
using AppUnrar.Model;

namespace AppUnrar
{
    /// <summary>
    /// Interaction logic for FileExtensionsManager_Add.xaml
    /// </summary>
    public partial class FileExtensionsManager_Add : Window
    {
        public FileExtensionsManager_Add()
        {
            InitializeComponent();
        }

        private void btnAddFileExtension_Click(object sender, RoutedEventArgs e)
        {
            if (!AppData.FileExtensionsToExtract.Contains(txtFileExtension.Text))
            {
                if (!txtFileExtension.Text.Contains("."))
                {
                    AppData.FileExtensionsToExtract.Add("." + txtFileExtension.Text);
                }
                else
                {
                    AppData.FileExtensionsToExtract.Add(txtFileExtension.Text);
                }

                Presenter.AppDataView.appdata_save_changes();
            }
            else
            {
                MessageBox.Show("This file extension its already added.");
            }
        }
    }
}
