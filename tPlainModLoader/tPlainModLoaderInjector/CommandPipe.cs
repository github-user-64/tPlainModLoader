using System;
using System.Threading;
using System.Threading.Tasks;

namespace tPlainModLoaderInjector
{
    internal class CommandPipe
    {
        internal const string pipe_toTContentPatch = "tContentPatch_Pipe_command_ToTContentPatch";
        internal const string pipe_toOutput = "tContentPatch_Pipe_command_ToOutput";

        public static void Initialize()
        {
            _ = Task.Run(() =>
            {
                tContentPatch.Utils.Pipe.Pipe_receive(pipe_toOutput, s => Console.WriteLine(s));
            });
        }

        public static void Run()
        {
            while (true)
            {
                try
                {
                    string s = Console.ReadLine();
                    tContentPatch.Utils.Pipe.Pipe_send(pipe_toTContentPatch, s);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"发送消息时发生未知异常:{ex.Message}");
                    Thread.Sleep(1);
                }
            }
        }
    }
}
