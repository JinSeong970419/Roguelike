using System;
using System.Diagnostics;

namespace Roguelike.Core
{
    public class BatchFileExecutor
    {
        public static void Execute(string batchFilePath)
        {
            Process.Start(batchFilePath);
        }

        public static void ExecuteAsAdministrator(string batchFilePath)
        {
            if (string.IsNullOrEmpty(batchFilePath)) return;

            ProcessStartInfo info = new ProcessStartInfo();
            info.CreateNoWindow = false;
            info.FileName = @"cmd.exe";
            info.Verb = "runas";
            info.Arguments = "/C " + batchFilePath;

            var process = new Process();
            process.StartInfo = info;
            process.Start();
        }
    }
}
