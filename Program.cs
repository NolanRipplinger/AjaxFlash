using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace KeyboardFlash
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            const string quote = "\"";

            //Form myForm = new Form();
            using (OpenFileDialog findFlashFile = new OpenFileDialog())
            {


                //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.CurrentDirectory) ;
                findFlashFile.InitialDirectory = Environment.CurrentDirectory;
                findFlashFile.Filter = "Firmware File (*.bin)|*.bin";
                findFlashFile.FilterIndex = 2;
                findFlashFile.RestoreDirectory = true;
                findFlashFile.ShowHelp = true;
                findFlashFile.Multiselect = false;

                if (findFlashFile.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = quote + findFlashFile.FileName.Replace(@"\\",@"\") + quote;
                }

                if (filePath.Length == 0)//If a file was not chosen
                {
                    System.Windows.Forms.MessageBox.Show("No file chosen; Exiting!");

                    System.Environment.Exit(1);
                }

            }
            //MessageBox.Show(filePath, "File Content at path: " + filePath, MessageBoxButtons.OK);
            //Console.WriteLine($"The FileName: {filePath}");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"mdloader.exe");
            //ProcessStartInfo startInfo = new ProcessStartInfo("notepad.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = $@"--first --download {filePath} --restart";
            try
            {
                Process.Start(startInfo);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Please run in the same location as mdloader.exe or select executable!");

                var exeFilePath = string.Empty;

                using (OpenFileDialog findExeFile = new OpenFileDialog())
                {


                    //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.CurrentDirectory) ;
                    findExeFile.InitialDirectory = Environment.CurrentDirectory;
                    findExeFile.Filter = "mdloader.exe (*.exe)|*.exe";
                    findExeFile.FilterIndex = 2;
                    findExeFile.RestoreDirectory = true;
                    findExeFile.ShowHelp = true;
                    findExeFile.Multiselect = false;

                   

                    if (findExeFile.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        exeFilePath = findExeFile.FileName;
                    }

                    if (exeFilePath.Length == 0)//If a file was not chosen
                    {
                        System.Windows.Forms.MessageBox.Show("No executable chosen; Exiting!");

                        System.Environment.Exit(1);
                    }

                }

                ProcessStartInfo startInfoRetry = new ProcessStartInfo(exeFilePath);
                startInfoRetry.WindowStyle = ProcessWindowStyle.Normal;
                startInfoRetry.Arguments = $@"--first --download {filePath} --restart";

                try
                {
                    Process.Start(startInfoRetry);
                }
                catch (Exception ex)
                {
                    System.Environment.Exit(3);
                }

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Unknown error occured, sorry :(");

                System.Environment.Exit(3);
            }

        }
    }
}