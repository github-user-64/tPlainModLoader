using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace Skil.Content.skil15
{
    //旋转射出圣骑士锤
    internal class skil15_3 : PatchPlayer
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<bool> NoTp = new GetSetReset<bool>();
        private static int count = 0;
        private static int countMax = 8;
        private static float nowR = 0;

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get2(Enable, "按住右键", "Images/Item_1513", "摧毁停车场"),
                UIBuild.get2(NoTp, null, "Images/Item_1513", "停车场不瞬移"),
            };
        }

        public override void UpdatePostfix(Player This, int playerI)
        {
            if (Enable.val == false) return;
            if (This != Main.LocalPlayer) return;
            if (This.mouseInterface == true) return;
            if (Main.mouseRight == false) return;
            if (skil15_4.Enable.val && This.controlDown == true) return;

            if (Main.GameUpdateCount % 2 != 0) return;

            if (count > countMax)
            {
                count = 0;

                nowR += 1;
                if (nowR > MathHelper.TwoPi) nowR -= MathHelper.TwoPi;
            }

            float r = (MathHelper.TwoPi / countMax) * count;
            r += nowR;

            Vector2 v = Vector2.UnitX.RotatedBy(r) * 22;

            Projectile.NewProjectile(null, This.Center, v,
                ProjectileID.PaladinsHammerFriendly, SkilListControl1.damage.val, 1, This.whoAmI,
                modifier: SetNoTp);

            ++count;
        }

        private static void SetNoTp(Projectile proj)
        {
            if (NoTp.val) proj.localAI[2] = 1;
        }
    }
}
