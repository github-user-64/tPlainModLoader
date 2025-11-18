using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.DataStructures;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_damagePlay2 : PatchMain
    {
        public static GetSetReset<int> damagePlay_play = new GetSetReset<int>(-1, -1);
        public static GetSetReset<int> damagePlay_damage = new GetSetReset<int>(777, 777);

        public override void Initialize()
        {
            damagePlay_play.OnValUpdate += v => Utils.OutputPlayerName(damagePlay_play.val);
        }

        public static void damage()
        {
            if (damagePlay_play.val >= Main.player.Length) return;

            PlayerDeathReason reason = PlayerDeathReason.ByPlayer(255);

            if (damagePlay_play.val == -1)
            {
                for (int i = 0; i < 255; ++i)
                {
                    NetMessage.SendPlayerHurt(i, reason, damagePlay_damage.val, -1, true, true, -1, -1, -1);
                }
            }
            else
            {
                if (damagePlay_play.val < 0) return;

                NetMessage.SendPlayerHurt(damagePlay_play.val, reason, damagePlay_damage.val, -1, true, true, -1, -1, -1);
            }
        }

        public static List<CommandObject> GetCO()
        {
            CommandObject co = new CommandObject("damagePlay2");
            co.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(co.SubCommand));

            CommandMethod damage = new CommandMethod("invoke");
            damage.Runing += v => Function_damagePlay2.damage();
            co.SubCommand.Add(damage);

            co.SubCommand.Add(new CommandHRA<int>("play", damagePlay_play, new CommandInt()));
            co.SubCommand.Add(new CommandHRA<int>("damage", damagePlay_damage, new CommandInt()));

            List<CommandObject> cos = new List<CommandObject>
            {
                co,
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get5(damagePlay_play, int.Parse, "伤害", damage, "对玩家造成伤害, -1时对全部玩家<int>", null, "伤害玩家2"),
            };

            return uis;
        }
    }
}
