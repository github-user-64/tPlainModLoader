using CommandHelp;
using System.Collections.Generic;
using tContentPatch;

namespace WandsTool
{
    public class Command : Mod
    {
        public override List<CommandObject> GetCommands()
        {
            List<CommandObject> cos = new List<CommandObject>();

            CommandObject root = new CommandObject(nameof(WandsTool));
            root.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(root.SubCommand));

            CommandMethod enable = new CommandMethod("enable", 1);
            enable.SubCommand.Add(new CommandTrue());
            enable.SubCommand.Add(new CommandFalse());
            enable.Runing += v =>
            {
                if (v == null) return;
                if (v.Length < 1) return;
                if (v[0] is bool e)
                {
                    Content.gameMain.Wand_isEnable = e;
                }
            };

            root.SubCommand.Add(enable);

            bool NoPublic = true;

            if (NoPublic == false)
            {
                CommandMethod updateCount = new CommandMethod("updateCount", 1);
                updateCount.SubCommand.Add(new CommandInt());
                updateCount.Runing += v =>
                {
                    if (v == null) return;
                    if (v.Length < 1) return;
                    if (v[0] is int uc)
                    {
                        Content.gameMain.Wand_UpdateCount = uc;
                    }
                };

                root.SubCommand.Add(updateCount);
            }
            
            cos.Add(root);
            return cos;
        }
    }
}
