using System.Threading.Tasks;

namespace tContentPatch.Command
{
    internal static class Pipe
    {
        internal static bool EnablePipe { get; private set; } = false;
        internal const string pipe_toTContentPatch = "tContentPatch_Pipe_command_ToTContentPatch";
        internal const string pipe_toOutput = "tContentPatch_Pipe_command_ToOutput";

        public static void Initialize(bool enable)
        {
            EnablePipe = enable;

            if (EnablePipe == false) return;

            _ = Task.Run(() =>
            {
                tContentPatch.Utils.Pipe.Pipe_receive(pipe_toTContentPatch, s => ContentPatch.RunCommand(s));
            });
        }
    }
}
