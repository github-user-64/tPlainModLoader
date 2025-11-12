using CommandHelp;
using System.Collections.Generic;
using tContentPatch;

namespace Skil
{
    public class Command : Mod
    {
        public override List<CommandObject> GetCommands()
        {
            List<CommandObject> cos = new List<CommandObject>();

            CommandObject root = new CommandObject(nameof(Skil));
            root.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(root.SubCommand));

            root.SubCommand.AddRange(Content.SkilListControl1.GetCO());
            root.SubCommand.AddRange(Content.SkilListControl2.GetCO());
            root.SubCommand.AddRange(Content.SkilListControl3.GetCO());
            root.SubCommand.AddRange(Content.SkilListControl4.GetCO());

            cos.Add(root);
            return cos;
        }
    }
}
