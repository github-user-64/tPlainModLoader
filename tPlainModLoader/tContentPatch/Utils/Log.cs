using System;
using System.IO;
using System.Text;

namespace tContentPatch.Utils
{
    /// <summary/>
    public static class Log
    {
        /// <summary/>
        public static string File { get; private set; } = null;
        private readonly static object _lock = new object();

        /// <summary/>
        public static void Add(string s, bool addTime = true)
        {
            if (s == null) return;
            if (File == null) return;

            lock (_lock)
            {
                if (addTime)
                {
                    DateTime time = DateTime.Now;

                    s = $"[{time.Hour}:{time.Minute}:{time.Millisecond}]:{s}\n";
                }

                try
                {
                    System.IO.File.AppendAllText(File, s, Encoding.UTF8);
                }
                catch { }
            }
        }

        /// <summary/>
        public static void SetPath(string filePath)
        {
            if (filePath == null) return;

            try
            {
                if (Directory.Exists(Path.GetDirectoryName(filePath)) != true) filePath = Path.GetFileName(filePath);
            }
            catch { }

            if (filePath == null) return;
            filePath = filePath.TrimEnd();
            if (filePath.Length < 1) return;

            File = filePath;
        }

        /// <summary/>
        public static void Clear()
        {
            if (File == null) return;

            if (System.IO.File.Exists(File) != true) return;

            System.IO.File.Delete(File);
        }
    }
}
