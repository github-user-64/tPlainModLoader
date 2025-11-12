using CommandHelp;
using System;

namespace SundryTool.Utils
{
    /// <summary>
    /// Help, Reset, Add
    /// </summary>
    internal class CommandHRA<T> : CommandMethod
    {
        public CommandHRA(string texe, GetSetReset<T> gsr, params CommandObject[] add) : base(texe, 1)
        {
            SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(SubCommand));
            SubCommand.Add(new CommandVariable("reset"));
            foreach (CommandObject co in add) SubCommand.Add(co);

            Runing += vs =>
            {
                if (vs[0] == null) return;

                if (vs[0] is CommandVariable cv)
                {
                    if (cv.TextEquals) gsr.Reset();
                    else Console.WriteLine(gsr.val);
                }
                else
                {
                    gsr.val = (T)vs[0];
                }
            };
        }
    }
}
