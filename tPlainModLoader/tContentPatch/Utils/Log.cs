using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tContentPatch.Utils
{
    public static class Log
    {
        public static string path { get; private set; } = null;
        private static List<string> logs = new List<string>();
        private static DateTime lastSaveTime = DateTime.Now;
        private const int MAX_LOG_COUNT = 1000;
        private const int SAVE_INTERVAL_MINUTES = 5;

        public static void Add(string s)
        {
            DateTime time = DateTime.Now;

            string ss = $"[{time.Hour}:{time.Minute}:{time.Millisecond}]:{s}\n";
            logs.Add(ss);

            bool shouldSave = false;

            if (logs.Count >= MAX_LOG_COUNT)
            {
                shouldSave = true;
            }
            else if ((DateTime.Now - lastSaveTime).TotalMinutes >= SAVE_INTERVAL_MINUTES)
            {
                shouldSave = true;
            }

            if (shouldSave)
            {
                SaveTry();
                lastSaveTime = DateTime.Now;
            }
        }

        public static void SaveTry()
        {
            try
            {
                string file = path;

                if (Directory.Exists(Path.GetDirectoryName(file)) == false) file = Path.GetFileName(file);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < logs.Count; ++i) sb.Append(logs[i]);

                File.AppendAllText(file, sb.ToString(), Encoding.UTF8);
                logs.Clear();
            }
            catch { }
        }

        public static void SetPath(string filePath)
        {
            if (filePath == null) return;
            if (Directory.Exists(Path.GetDirectoryName(filePath)) == false) return;

            Log.path = filePath;
        }
    }
}
