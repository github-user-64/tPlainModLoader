using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace tPlainModLoaderInjector
{
    public static class ProcessUtils
    {
        public static List<Process> GetProcessPID(string[] name)
        {
            if (name == null || name.Length < 1) return null;

            Process[] processes = Process.GetProcesses();

            List<Process> ids = processes.Where(i => name.Contains(i.ProcessName) && i.Id != -1).ToList();

            return ids.Count > 0 ? ids : null;
        }
    }
}
