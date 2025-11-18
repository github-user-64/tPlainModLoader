using CommandHelp;
using System.Collections.Generic;
using tContentPatch;
using Terraria;

namespace test1
{
    internal class Command : Mod
    {
        public override List<CommandObject> GetCommands()
        {
            List<CommandObject> list = new List<CommandObject>();

            CommandMethod clear = new CommandMethod("clear", 1);
            CommandKeyVal kp = new CommandKeyVal("p", 0);
            clear.SubCommand.Add(kp);
            clear.Runing += v =>
            {
                if (v[0] as int? == 0)
                {
                    foreach(var i in Main.projectile) i.Kill();
                }
            };

            CommandMethod vp = new CommandMethod("vp");
            vp.Runing += _ =>
            {
                VirtualPlayer.VP.spaw();
            };

            list.Add(clear);
            list.Add(vp);
            return list;
        }
    }
}
