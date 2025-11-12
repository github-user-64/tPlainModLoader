using CommandHelp;
using System.Collections.Generic;
using tContentPatch;

namespace OptimizeAndTool
{
    public class Command : Mod
    {
        public override List<CommandObject> GetCommands()
        {
            List<CommandObject> cos = new List<CommandObject>();

            CommandObject root = new CommandObject(nameof(OptimizeAndTool));
            root.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(root.SubCommand));

            root.SubCommand.AddRange(Content.Function.GetCO());

            cos.Add(root);
            return cos;
        }
    }
}
