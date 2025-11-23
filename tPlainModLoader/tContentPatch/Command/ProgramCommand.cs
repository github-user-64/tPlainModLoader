using CommandHelp;
using System;
using System.Collections.Generic;

namespace tContentPatch.Command
{
    internal class ProgramCommand
    {
        /// <summary>
        /// 用已有的指令列表运行指令
        /// </summary>
        /// <param name="command"></param>
        public static void Run(string command)
        {
            List<CommandObject> cos = GetCO();

            List<CommandObject> cosMod = GetModCO();
            if (cosMod != null) cos.AddRange(cosMod);

            cos.Add(Utils.GetCO_OutputCOList(cos));

            string msg = Utils.CommandRun(command, cos);

            if (msg == null) return;
            
            ContentPatch.PrintTry(msg);
        }

        private static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>();

            CommandObject console = new CommandObject("console");
            CommandMethod console_clear = new CommandMethod("clear");
            console_clear.Runing += args =>
            {
                tContentPatch.Utils.ConsoleUtils.Clear();
            };
            console.SubCommand.Add(console_clear);
            console.SubCommand.Add(Utils.GetCO_OutputCOList(console.SubCommand));

            cos.Add(console);

            return cos;
        }

        private static List<CommandObject> GetModCO()
        {
            try
            {
                List<ModLoad.ModObject> mos = ContentPatch.GetModObjects();
                if (mos == null) return null;

                List<CommandObject> cos_mod = new List<CommandObject>();

                foreach (ModLoad.ModObject mo in mos)
                {
                    for (int i = 0; i < mo?.inheritance_mod?.Count; ++i)
                    {
                        List<CommandObject> cosMod = mo.inheritance_mod[i].GetCommands();
                        if (cosMod == null) continue;
                        cos_mod.AddRange(cosMod);
                    }
                }

                return cos_mod;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取模组指令列表失败:{ex.Message}");
                return null;
            }
        }
    }
}
