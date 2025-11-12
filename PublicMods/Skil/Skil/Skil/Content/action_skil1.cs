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
    public class skil1 : ModPlayer
    {
        //技能1, 生成一圈旋转的射弹
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> Count = new GetSetReset<int>(5, 5, GetSetReset.GetIntFunc(0, 64));//数量
        public static GetSetReset<int> Range = new GetSetReset<int>(100, 100);//半径
        public static GetSetReset<float> Speed = new GetSetReset<float>(-0.04f, -0.04f);//速度

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil1", Enable)
                .SkilCMDBuild("count", Count)
                .SkilCMDBuild("range", Range)
                .SkilCMDBuild("speed", Speed),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Count, int.Parse, "数量<int>", "Images/Projectile_274", "技能1"),
                UIBuild.get6(Range, int.Parse, "<int>", "Images/Projectile_274", "技能1半径"),
                UIBuild.get6(Speed, float.Parse, "<float>", "Images/Projectile_274", "技能1速度"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil1(This);
            else a1_skil1_false(This);
        }

        protected static List<Projectile> skil1_ps = new List<Projectile>();
        public static void a1_skil1(Player player)
        {
            if (player == null || player.active == false || player.dead == true)
            {
                a1_skil1_false(player);
                return;
            }

            //

            int projId = 274;//死神镰刀

            //还在
            for (int i = 0; i < skil1_ps.Count; ++i)
            {
                if (Utils.projExist(skil1_ps[i], projId, player)) continue;

                skil1_ps.RemoveAt(i);
                --i;
            }
            //速度没慢下来
            for (int i = 0; i < skil1_ps.Count; ++i)
            {
                if (skil1_ps[i].timeLeft < 85 == false) continue;

                skil1_ps[i].Kill();
                skil1_ps.RemoveAt(i);
                --i;
            }

            if (skil1_ps.Count < Count.val)
            {
                int id = Projectile.NewProjectile(null, player.Center, Vector2.Zero, projId, SkilListControl1.damage.val, 1);
                skil1_ps.Add(Main.projectile[id]);
            }

            for (int i = 0; i < skil1_ps.Count; ++i)
            {
                float radians = MathHelper.TwoPi / Count.val;//弧度
                float angle = radians * i;
                angle += Main.GameUpdateCount * Speed.val;

                Vector2 p = Vector2.UnitX.RotatedBy(angle) * Range.val;
                p += player.Center;

                skil1_ps[i].Center = p;

                NetMessage.SendData(27, -1, -1, null, skil1_ps[i].whoAmI);
            }
        }
        public static void a1_skil1_false(Player player)
        {
            if (player == null) return;

            for (int i = 0; i < skil1_ps.Count; ++i) skil1_ps[i]?.Kill();
            skil1_ps.Clear();
        }
    }
}
