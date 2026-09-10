using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace Skil.Content.skil15
{
    //圣骑士锤相关
    internal class skil15 : PatchMain
    {
        private static readonly bool[] IsGoHome = new bool[Main.projectile.Length - 1];

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>();
            uis.AddRange(skil15_1.GetUI());
            uis.AddRange(skil15_2.GetUI());
            uis.AddRange(skil15_3.GetUI());
            uis.AddRange(skil15_4.GetUI());

            return uis;
        }

        public override void OnEnterWorld()
        {
            for (int i = 0; i < IsGoHome.Length; i++) IsGoHome[i] = false;
        }

        public override void DoUpdateInWorldPostfix()
        {
            Player player = Main.LocalPlayer;
            if (player?.active != true) return;

            for (int i = 0; i < IsGoHome.Length; ++i)
            {
                Projectile proj = Main.projectile[i];
                if (proj?.active != true || proj.timeLeft < 1 || proj.type != ProjectileID.PaladinsHammerFriendly)
                {
                    IsGoHome[i] = false;
                    continue;
                }

                //

                if (proj.ai[0] == 0)//飞出去的状态
                {
                    IsGoHome[i] = true;
                    continue;
                }

                //

                if (IsGoHome[i] == false) continue;
                if (proj.ai[0] != 1) continue;
                //飞回来的状态

                IsGoHome[i] = false;

                skil15_1.Update(proj, player);
                skil15_2.Update(proj, player);
            }
        }
    }
}
