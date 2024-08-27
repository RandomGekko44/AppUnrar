using AppUnrar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading;

namespace AppUnrar.Presenter
{
    public static class ViewHandler
    {
        public static void start_decompression(ProgressBar pb, TextBlock tb, bool? decompress_only_selected_file_extensions)
        {
            if (AppData.Files_List?.Count != 0)
            {
                Model.Decompressor.File_Decompression_Handler(pb, tb, decompress_only_selected_file_extensions);
                AppData.Files_List?.Clear();
            }
            else
            {
                MessageBox.Show("Selected the files to extract", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void clear_file_list() { AppData.Files_List?.Clear(); }

        public static void fetch_files_info(String[] fileNames, DataGrid dg_file_list)
        {
            foreach (string filepath in fileNames)
            {
                var file = new FileInfo(filepath);
                var filesize = file.Length;
                var filename = file.Name;

                filesize /= 1048576;

                Model.File selected_file = new Model.File() { FileName = filename, FileSize = filesize.ToString() + " MB", FileExtension = file.Extension, FilePath = filepath };
                AppData.Files_List?.Add(selected_file);
            }
        }

        public static void update_decompression_log(TextBlock tb, string file_name)
        {
            tb.Text += "Decompressing " + file_name + "\n";
        }
    }
}
