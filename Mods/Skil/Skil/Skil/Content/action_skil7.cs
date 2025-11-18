using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace Skil.Content
{
    public class skil7 : PatchPlayer
    {
        //技能7, 超级星星炮的额外攻击射弹在周围生成
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil7", Enable),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get2(Enable, ico: "Images/Buff_159", text:"技能7"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil7(This);
        }

        public static void a1_skil7(Player player)
        {
            if (player == null) return;

            if (Main.mouseLeft == false || player.mouseInterface == true) return;

            int projId = 729;

            Vector2 d = Vector2.UnitX.RotatedBy((MathHelper.TwoPi / 360) * Utils.getRand(0, 365));

            Vector2 p = player.Center + d * 200;

            Vector2 v = Vector2.Negate(d);
            v = v.RotatedBy((MathHelper.TwoPi / 360) * Utils.getRand(-45, 45));
            v *= Utils.getRand(5, 14);

            Projectile.NewProjectile(null, p, v, projId, SkilListControl1.damage.val, 1);
        }
    }
}
