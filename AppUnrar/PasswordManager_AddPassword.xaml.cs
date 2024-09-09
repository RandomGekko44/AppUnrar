using System.Windows;
using AppUnrar.Model;

namespace AppUnrar
{
    /// <summary>
    /// Interaction logic for PasswordManager_AddPassword.xaml
    /// </summary>
    public partial class PasswordManager_AddPassword : Window
    {
        public PasswordManager_AddPassword()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!AppData.Passwords.Contains(txtPassword.Text))
            {
                AppData.Passwords.Add(txtPassword.Text);
                Presenter.AppDataView.appdata_save_changes();
            }
            else
            {
                MessageBox.Show("This password already exist.");
            }
        }
    }
}
