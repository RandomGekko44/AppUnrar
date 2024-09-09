using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            pb_decompression.Maximum = AppData.Files_List.Count;
            pb_decompression.Minimum = 0;
            pb_decompression.Value = 0;

            Presenter.ViewHandler.start_decompression(pb_decompression, tb_decompression_log, decompress_only_selected_file_extensions);
        }
    }
}
