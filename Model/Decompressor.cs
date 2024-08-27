using AppUnrar.Presenter;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static string? fileExtensionsToDecompress;
        private static List<FileDecompressionData>? FilesData;

        public static void File_Decompression_Handler(ProgressBar pb, TextBlock tb, bool? DecompressOnlySelectedFileExtensions)
        {
            fileExtensionsToDecompress = String.Empty;
            FilesData = new List<FileDecompressionData>();

            //Fetching file extensions to an string variable and adding format
            if ((bool)DecompressOnlySelectedFileExtensions)
            {
                fileExtensionsToDecompress = Fetch_File_Extensios();
            }

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

                    //Creating the file decompression data
                    FileDecompressionData decompression_data = new FileDecompressionData();
                    decompression_data.fileData = file;
                    decompression_data.output_path = output_path;
                    decompression_data.decompressionProcess = Create_Decompression_Process(file, output_path);

                    FilesData.Add(decompression_data);

                    //New thread to decompress a single file
                    //Thread decompressionThread = new Thread(() => Decompress_File(file, output_path, decompress_only_selected_file_extensions));
                    //decompressionThread.Start();
                }

                //ViewHandler.update_decompression_log(tb, file.FileName);
            }

            foreach (FileDecompressionData File in FilesData)
            {
                File.decompressionProcess?.Start();
                File.decompressionProcess?.WaitForExit();

                if (File.decompressionProcess?.ExitCode == 11) { UnrarPassword(File.fileData?.FilePath, File.output_path, File.fileData?.FileName); }
            }
        }

        private static System.Diagnostics.Process Create_Decompression_Process(File file, string output_path)
        {
            System.Diagnostics.Process process;

            if (file.FileExtension == ".zip" || file.FileExtension == ".7z")
            {
                process = Unzip(output_path, file.FilePath, file.FileName);

                return process;
            }
            else
            {
                if (file.FileExtension == ".rar")
                {
                    process = Unrar(output_path, file.FilePath, file.FileName);

                    return process;
                }
            }

            process = null;
            return process;
        }

        private static System.Diagnostics.Process Unrar(string output_path, string file_path, string file_name)
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = unrarExec;

                //Extract RAR with no password
                process.StartInfo.Arguments = string.Format(@"x -ptestpw ""{0}"" {1} ""{2}\""", file_path, fileExtensionsToDecompress, output_path);

                return process;
            }
        }

        private static void UnrarPassword(string _filePath, string _outputPath, string file_name)
        {
            string filePath = _filePath;
            string output_path = _outputPath;

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                if (AppData.Passwords.Count != 0)
                {
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = unrarExec;

                    foreach (string password in AppData.Passwords)
                    {
                        process.StartInfo.Arguments = string.Format(@"x -p{0} ""{1}"" {2} ""{3}\""", password, filePath, fileExtensionsToDecompress, output_path);
                        process.Start();
                        process.WaitForExit();
                    }
                }
            }
        }

        private static System.Diagnostics.Process Unzip(string output_path, string file_path, string file_name)
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = sevenZipExec;

                //Extract ZIP/7Z with no password
                process.StartInfo.Arguments = string.Format(@"x -ptestpw ""{0}"" -o""{1}"" -r {2}", file_path, output_path, fileExtensionsToDecompress);

                //Extract RAR with password/encrypted
                //if (process.ExitCode == 2) { UnzipPassword(file_path, output_path, file_name, fileExtensionsToDecompress); }

                return process;
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
