using System.Windows;
using AppUnrar.Model;

namespace AppUnrar
{
    /// <summary>
    /// Interaction logic for PasswordManager.xaml
    /// </summary>
    public partial class PasswordManager : Window
    {
        public PasswordManager()
        {
            InitializeComponent();
        }

        private void btnAddPassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordManager_AddPassword passwordManager_AddPassword = new PasswordManager_AddPassword();
            passwordManager_AddPassword.Owner = this;
            passwordManager_AddPassword.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            passwordManager_AddPassword.ShowDialog();
        }

        private void btnDeletePassword_Click(object sender, RoutedEventArgs e)
        {
            if (lstb_passwords.SelectedItem != null)
            {
                var result = MessageBox.Show("Do you want to delete " + lstb_passwords.SelectedItem.ToString() + " ?", "Delete Password", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    AppData.Passwords.Remove(lstb_passwords.SelectedItem.ToString());
                    Presenter.AppDataView.appdata_save_changes();
                }
            }
            else
            {
                MessageBox.Show("Selected a password from the list");
            }
        }
    }
}
