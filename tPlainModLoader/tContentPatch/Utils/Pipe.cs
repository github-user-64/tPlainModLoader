using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace tContentPatch.Utils
{
    public static class Pipe
    {
        public static void Pipe_receive(string path, Action<string> action)
        {
            while (true)
            {
                try
                {
                    using (NamedPipeServerStream pipeServer1 = new NamedPipeServerStream(path, PipeDirection.InOut))
                    {
                        try
                        {
                            pipeServer1.WaitForConnection();//等待连接，程序会阻塞在此处，直到有一个连接到达

                            using (StreamReader sr = new StreamReader(pipeServer1))
                            {
                                string s = sr.ReadToEnd();
                                if (path == Command.Pipe.pipe_toTContentPatch) s = s.Trim();
                                action?.Invoke(s);
                            }
                        }
                        catch
                        {
                            pipeServer1.Disconnect();
                            pipeServer1.Close();
                        }
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        }

        public static void Pipe_send(string path, string s)
        {
            if (s == null) return;
            if (s.Length == 0) return;

            _ = Task.Run(() =>
            {
                try
                {
                    using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", path, PipeDirection.InOut))
                    {
                        try
                        {
                            pipeClient.Connect(1000);

                            using (StreamWriter sw = new StreamWriter(pipeClient))
                            {
                                sw.AutoFlush = true;
                                sw.WriteLine(s);
                            }
                        }
                        catch
                        {
                            pipeClient.Close();
                        }
                    }
                }
                catch { }
            });
        }
    }
}
