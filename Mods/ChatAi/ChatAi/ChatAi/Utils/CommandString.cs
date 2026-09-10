using CommandHelp;
using System.Collections.Generic;

namespace ChatAi.Utils
{
    internal class CommandString : CommandValue<string>
    {
        public override string Text => "<string>";
        protected override string ArgConvertThrow(string arg) => arg;
        protected override string GetDefault() => null;
        private string rVal = null;

        public override CommandObject Parse(string command)
        {
            if (command.Length < 2) return null;
            if (command.Length < 3)
            {
                rVal = string.Empty;
                return this;
            }

            rVal = command.Substring(1, command.Length - 2);

            return this;
        }

        public override (string cmdParse, string cmd) ParseFormat(string command)
        {
            if (command == null) return (null, command);

            string cmd = command.TrimStart();

            if (cmd.Length < 2) return (null, command);
            if (cmd[0] != '\"') return (null, command);

            int index1 = 0;
            int index2 = cmd.IndexOf('\"', index1 + 1);

            if (index2 == -1) return (null, cmd);

            string text = cmd.Substring(index1, index2 + 1);
            cmd = cmd.Remove(index1, index2 + 1);

            return (text, cmd);
        }

        public override object Run(ref int index, List<CommandObject> commandList)
        {
            return rVal;
        }
    }
}
