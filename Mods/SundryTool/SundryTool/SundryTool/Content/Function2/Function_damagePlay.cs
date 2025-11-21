using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_damagePlay : PatchPlayer
    {
        public static GetSetReset<bool> damagePlay = new GetSetReset<bool>();
        public static GetSetReset<int> damagePlay_set = new GetSetReset<int>(-1, -1);

        public override void Initialize()
        {
            damagePlay_set.OnValUpdate += v => Utils.OutputPlayerName(damagePlay_set.val);
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Main.GameUpdateCount % 15 != 0) return;

            if (damagePlay.val == false) return;

            if (damagePlay_set.val < 0) return;
            if (damagePlay_set.val >= Main.player.Length) return;

            Player target = Main.player[damagePlay_set.val];
            Vector2 targetP = Function.aimAdvance.val ?
                target.Center + target.velocity * Function.aimAdvance_val.val :
                target.Center;

            int id = Projectile.NewProjectile(null, targetP, Vector2.Zero,
                102, 0, 0);
            Main.projectile[id].Damage();
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get1("damagePlay", damagePlay, damagePlay_set, new CommandInt()),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(damagePlay, damagePlay_set, int.Parse, "生成机械骷髅王炸弹<int>", "Images/Buff_30", "伤害玩家"),
            };

            return uis;
        }
    }
}
