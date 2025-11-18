using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace Skil.Content
{
    public class skil6 : PatchPlayer
    {
        //技能6, 一直转的射弹
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> ID = new GetSetReset<int>(132, 132, GetSetReset.GetIntFunc(0, ProjectileID.Count - 1));

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil6", Enable)
                .SkilCMDBuild("id", ID),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, ID, int.Parse, "射弹id(132, 79)<int>", "Images/Buff_306", "技能6"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil6(This);
            else a1_skil6_false(This);
        }

        protected static List<Projectile> skil6_ps = null;
        protected static List<Vector2> skil6_ps_p = null;
        public static void a1_skil6(Player player)
        {
            if (player == null) return;

            if (skil6_ps == null)
            {
                skil6_ps = new List<Projectile>();
                skil6_ps_p = new List<Vector2>();
                for (int i = 0; i < 10; ++i)
                {
                    skil6_ps.Add(null);
                    skil6_ps_p.Add(Vector2.Zero);
                }
            }

            for (int i = 0; i < skil6_ps.Count; ++i)
            {
                if (Utils.projExist(skil6_ps[i], ID.val, player)) continue;

                int id = Projectile.NewProjectile(null, player.Center, Vector2.Zero, ID.val, SkilListControl1.damage.val, 1);

                skil6_ps[i] = Main.projectile[id];

                skil6_ps_p[i] = skil6_ps[i].Center;
            }

            for (int i = 0; i < skil6_ps.Count; ++i)
            {
                Projectile p = skil6_ps[i];

                int index = i;
                int indexCount = skil6_ps.Count;
                float speed = 2;
                float x = 4f;
                float y = 1f;
                float spacing = 16f * 2;//间距
                float f = ((float)index / (float)indexCount + player.miscCounterNormalized * (int)speed) * (MathHelper.Pi * 2f);
                float scaleFactor = spacing + (float)indexCount * 6f;
                Vector2 vector = player.position - player.oldPosition;
                skil6_ps_p[i] += vector;
                Vector2 value = f.ToRotationVector2();
                Vector2 value2 = player.Center + value * new Vector2(x, y) * scaleFactor;
                skil6_ps_p[i] = Vector2.Lerp(skil6_ps_p[i], value2, 0.3f);

                p.Center = player.MountedCenter;
                p.Center += (player.MountedCenter.DirectionTo(skil6_ps_p[i]) * player.MountedCenter.Distance(skil6_ps_p[i]))
                    .RotatedBy(Main.GameUpdateCount * 0.01);

                p.velocity = p.Center - player.Center;
                p.velocity.Normalize();

                NetMessage.SendData(27, -1, -1, null, p.whoAmI);
            }
        }
        public static void a1_skil6_false(Player player)
        {
            if (player == null) return;

            for (int i = 0; i < skil6_ps?.Count; ++i)
            {
                skil6_ps[i]?.Kill();
            }
            skil6_ps = null;
        }
    }
}
