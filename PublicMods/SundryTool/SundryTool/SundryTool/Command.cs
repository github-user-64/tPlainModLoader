using CommandHelp;
using System.Collections.Generic;
using tContentPatch;

namespace SundryTool
{
    public class Command : Mod
    {
        public override List<CommandObject> GetCommands()
        {
            List<CommandObject> cos = new List<CommandObject>();

            CommandObject root = new CommandObject(nameof(SundryTool));
            root.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(root.SubCommand));

            CommandObject player = new CommandObject("player");
            player.SubCommand.AddRange(Content.PlayerModify.ValSet.GetCO());
            player.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(player.SubCommand));
            root.SubCommand.Add(player);

            CommandObject item = new CommandObject("heldItem");
            item.SubCommand.AddRange(Content.HeldItemModify.ValSet.GetCO());
            item.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(item.SubCommand));
            root.SubCommand.Add(item);

            root.SubCommand.AddRange(Content.Function1.Function.GetCO());
            root.SubCommand.AddRange(Content.Function2.Function.GetCO());

            cos.Add(root);
            return cos;
        }
    }
}
