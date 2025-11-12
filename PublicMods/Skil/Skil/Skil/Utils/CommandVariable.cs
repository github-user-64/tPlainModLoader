using CommandHelp;
using System.Collections.Generic;

namespace Skil.Utils
{
    /// <summary>
    /// 指令匹配或者为空字符串的都会进来
    /// </summary>
    public class CommandVariable : CommandObject
    {
        public bool TextEquals = false;

        public CommandVariable(string text) : base(text) { }

        public override CommandObject Parse(string command)
        {
            if (command == Text)
            {
                TextEquals = true;
                return this;
            }
            else
            if (command == string.Empty) return this;

            return null;
        }

        public override (string cmdParse, string cmd) ParseFormat(string command)
        {
            (string, string) result = base.ParseFormat(command);
            if (result.Item1 == null)
            {
                return (cmdParse: string.Empty, cmd: result.Item2);
            }

            return result;
        }

        public override object Run(ref int index, List<CommandObject> commandList)
        {
            return this;
        }
    }
}
