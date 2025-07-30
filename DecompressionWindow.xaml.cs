using System.Diagnostics;
using System.Windows;

namespace AppUnrar
{
    /// <summary>
    /// Interaction logic for DecompressionWindow.xaml
    /// </summary>
    public partial class DecompressionWindow : Window
    {
        private bool? decompress_only_selected_file_extensions;

        public DecompressionWindow(bool? decompress_only_selected_file_extensions)
        {
            InitializeComponent();

            this.decompress_only_selected_file_extensions = decompress_only_selected_file_extensions;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pb_decompression.Maximum = AppData.Files_List.Count;
            pb_decompression.Minimum = 0;
            pb_decompression.Value = 0;

            Debug.Print(AppData.Files_List.Count.ToString());

            Presenter.ViewHandler.start_decompression(pb_decompression, tb_decompression_log, decompress_only_selected_file_extensions);
        }
    }
}
