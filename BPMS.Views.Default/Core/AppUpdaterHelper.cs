using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BPMS.Views.Default
{
    public class AppUpdaterHelper
    {
        public static int CheckNewFiles(string appUpdaterPath, string endPointAddress)
        {
            return RunUpdater(appUpdaterPath, String.Format("1 \"{0}\"", endPointAddress), true);
        }

        public static void RunUpdater(string appUpdaterPath, string endPointAddress, int threadNum, long bufferSize, int delayMillisecond, string executablePath)
        {
            RunUpdater(appUpdaterPath, String.Format("0 \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"",
                    endPointAddress, executablePath, threadNum, bufferSize, delayMillisecond), false);
        }

        private static int RunUpdater(string appUpdaterPath, string arguments, bool waitForExit)
        {
            int exitCode = 0;
            FileInfo info = new FileInfo(appUpdaterPath);
            if (!info.Exists)
                return 0;
            ProcessStartInfo info2 = new ProcessStartInfo();
            info2.FileName = info.FullName;
            info2.WorkingDirectory = info.Directory.FullName;
            info2.Arguments = arguments;
            Process process = new Process();
            process.StartInfo = info2;
            process.Start();
            if (waitForExit)
            {
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            else
            {
                //process.WaitForExit(1000);
                exitCode = 0;
            }
            process.Close();
            return exitCode;
        }
    }
}
