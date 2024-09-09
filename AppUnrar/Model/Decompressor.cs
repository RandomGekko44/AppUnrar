using AppUnrar.Presenter;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AppUnrar.Model
{
    public static class Decompressor
    {
        private const string unrarExec = @".\unrar\UnRAR.exe";
        private const string sevenZipExec = @".\7zip\7za.exe";

        public static void File_Decompression_Handler(ProgressBar pb, TextBlock tb, bool? decompress_only_selected_file_extensions)
        {
            foreach (File file in AppData.Files_List)
            {
                string output_path = @".\output" + @"\" + file.FileName;

                if (Directory.Exists(output_path))
                {
                    continue;
                }
                else
                {
                    DirectoryInfo dir = Directory.CreateDirectory(output_path);

                    Decompress_File(file, output_path, decompress_only_selected_file_extensions);

                    //await Task.Run(() =>
                    //{
                        
                    //});

                    //New thread to decompress a single file
                    //Thread decompressionThread = new Thread(() => Decompress_File(file, output_path, decompress_only_selected_file_extensions));
                    //decompressionThread.Start();
                }

                pb.Value++;
                ViewHandler.update_decompression_log(tb, file.FileName);
            }
        }

        private static void Decompress_File(File file, string output_path, bool? decompress_only_selected_file_extensions)
        {
            if (file.FileExtension == ".zip" || file.FileExtension == ".7z")
            {
                Unzip(output_path, file.FilePath, file.FileName, decompress_only_selected_file_extensions);
            }
            else
            {
                if (file.FileExtension == ".rar")
                {
                    Unrar(output_path, file.FilePath, file.FileName, decompress_only_selected_file_extensions);
                }
            }
        }

        private static void Unrar(string output_path, string rar_file_path, string file_name, bool? cb_DecompressOnlySelectedFileExtensions)
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = unrarExec;

                string file_extensions_str = string.Empty;

                //Fetching file extensions to an string variable and adding format
                if ((bool)cb_DecompressOnlySelectedFileExtensions)
                {
                    file_extensions_str = Fetch_File_Extensios();
                }

                //Extract RAR with no password
                process.StartInfo.Arguments = string.Format(@"x -ptestpw ""{0}"" {1} ""{2}\""", rar_file_path, file_extensions_str, output_path);
                process.Start();
                process.WaitForExit();

                //Extract RAR with password/encrypted
                if (process.ExitCode == 11) { UnrarPassword(rar_file_path, output_path, file_name, file_extensions_str); }
            }
        }

        private static void UnrarPassword(string _sourceRar, string _outputPath, string file_name, string file_extensions_str)
        {
            string source_rar = _sourceRar;
            string output_path = _outputPath;

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                if (AppData.Passwords.Count != 0)
                {
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = unrarExec;

                    foreach (string password in AppData.Passwords)
                    {
                        process.StartInfo.Arguments = string.Format(@"x -p{0} ""{1}"" {2} ""{3}\""", password, source_rar, file_extensions_str, output_path);
                        process.Start();
                        process.WaitForExit();
                    }
                }
            }
        }

        private static void Unzip(string output_path, string file_path, string file_name, bool? cb_DecompressOnlySelectedFileExtensions)
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = sevenZipExec;

                string file_extensions_str = string.Empty;

                //Fetching file extensions to an string variable and adding format
                if ((bool)cb_DecompressOnlySelectedFileExtensions)
                {
                    file_extensions_str = Fetch_File_Extensios();
                }

                //Extract ZIP/7Z with no password
                process.StartInfo.Arguments = string.Format(@"x -ptestpw ""{0}"" -o""{1}"" -r {2}", file_path, output_path, file_extensions_str);

                //MessageBox.Show(process.StartInfo.Arguments);
                process.Start();
                process.WaitForExit();

                //Extract RAR with password/encrypted
                if (process.ExitCode == 2) { UnzipPassword(file_path, output_path, file_name, file_extensions_str); }
            }
        }

        private static void UnzipPassword(string _sourceRar, string _outputPath, string file_name, string file_extensions_str)
        {
            string source_rar = _sourceRar;
            string output_path = _outputPath;

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                if (AppData.Passwords.Count != 0)
                {
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = sevenZipExec;

                    foreach (string password in AppData.Passwords)
                    {
                        process.StartInfo.Arguments = string.Format(@"x -p{0} ""{1}"" -o""{2}"" -r {3}", password, source_rar, output_path, file_extensions_str);
                        //MessageBox.Show(process.StartInfo.Arguments);
                        process.Start();
                        process.WaitForExit();

                        if (process.ExitCode == 0) { break; }
                    }
                }
            }
        }

        private static string Fetch_File_Extensios()
        {
            string file_extensions_str = string.Empty;

            if (AppData.FileExtensionsToExtract != null && AppData.FileExtensionsToExtract?.Count != 0)
            {
                foreach (string file_extension in AppData.FileExtensionsToExtract)
                {
                    file_extensions_str = file_extensions_str + "*" + file_extension + " ";
                }
            }

            return file_extensions_str;
        }
    }
}
