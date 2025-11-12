using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace Skil.Content
{
    public class skil12 : ModPlayer
    {
        //技能12, 多个圈
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<float> ArcLength = new GetSetReset<float>(126, 126);//弧长, 在圆周长上的任意一段弧的长度叫做弧长
        public static GetSetReset<int> R = new GetSetReset<int>(100, 100);//半径
        public static GetSetReset<int> R_base = new GetSetReset<int>();//基础半径
        public static GetSetReset<int> CircleCount = new GetSetReset<int>(2, 2);//圈数
        public static GetSetReset<float> Speed = new GetSetReset<float>(0.04f, 0.04f);
        public static GetSetReset<float> Size = new GetSetReset<float>(1, 1);

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil12", Enable)
                .SkilCMDBuild("arcLength", ArcLength)
                .SkilCMDBuild("r", R)
                .SkilCMDBuild("r_base", R_base)
                .SkilCMDBuild("circleCount", CircleCount)
                .SkilCMDBuild("speed", Speed)
                .SkilCMDBuild("size", Size),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get2(Enable, ico:"Images/Extra_19", text:"技能12"),
                UIBuild.get6(ArcLength, float.Parse, "<float>", "Images/Extra_19", "技能12弧长"),
                UIBuild.get6(R, int.Parse, "<int>", "Images/Extra_19", "技能12半径"),
                UIBuild.get6(R_base, int.Parse, "<int>", "Images/Extra_19", "技能12基础半径"),
                UIBuild.get6(CircleCount, int.Parse, "<int>", "Images/Extra_19", "技能12圈数"),
                UIBuild.get6(Speed, float.Parse, "<float>", "Images/Extra_19", "技能12速度"),
                UIBuild.get6(Size, float.Parse, "<float>", "Images/Extra_19", "技能12大小"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil12(This);
            else a1_skil12_false(This);
        }

        private static List<Projectile> skil12_ps = null;
        private static int skil12_r_this = 0;//当前半径
        private static int skil12_r_base_this = 0;//当前基础半径
        private static float skil12_size_this = 0;

        public static void a1_skil12(Player player)
        {
            if (player == null) return;
            if (player.active == false || player.dead == true) a1_skil12_false(player);

            if (skil12_ps == null) skil12_ps = new List<Projectile>();

            if (ArcLength.val < 1) ArcLength.val = 1;
            if (R.val < 0) R.val = 0;

            //

            if (skil12_r_this < R.val) ++skil12_r_this;
            else
            if (skil12_r_this > R.val) skil12_r_this = R.val;
            else
            if (skil12_r_base_this < R_base.val) ++skil12_r_base_this;
            else
            if (skil12_r_base_this > R_base.val) skil12_r_base_this = R_base.val;

            if (skil12_size_this < Size.val) skil12_size_this += 0.01f;
            else
            if (skil12_size_this > Size.val) skil12_size_this = Size.val;

            //

            a1_skil12_update_proj(player);
        }

        public static void a1_skil12_false(Player player)
        {
            if (player == null) return;

            skil12_r_this = 0;
            skil12_r_base_this = 0;
            skil12_size_this = 0;

            if (skil12_ps != null)
            {
                for (int i = 0; i < skil12_ps.Count; ++i) skil12_ps[i]?.Kill();
                skil12_ps.Clear();
                skil12_ps = null;
            }
        }

        private static Vector2[] a1_skil12_getCirclePos(int r, float arcLength)//根据弧长将圆分成n份
        {
            float circumference = MathHelper.TwoPi * r;//圆周长
            int pointCount = (int)Math.Floor(circumference / ArcLength.val);//该圆周长最多能分成几份

            Vector2[] pos = new Vector2[pointCount];

            for (int i = 0; i < pointCount; ++i)
            {
                float radian = MathHelper.TwoPi / pointCount * i;

                Vector2 p = Vector2.UnitX * r;
                p = p.RotatedBy(radian);

                pos[i] = p;
            }

            return pos;
        }

        private static void a1_skil12_update_proj(Player player)
        {
            bool isOneSpawn = true;
            for (int i = 0; i < skil12_ps.Count; ++i)
            {
                if (Utils.projExist(skil12_ps[i], 459, player) == false)
                {
                    if (isOneSpawn)
                    {
                        int id = Projectile.NewProjectile(null, player.Center, Vector2.Zero, 459, SkilListControl1.damage.val, 1);
                        skil12_ps[i] = Main.projectile[id];
                        isOneSpawn = false;
                    }
                    else
                    {
                        skil12_ps[i] = null;
                    }
                }
            }

            //

            List<Vector2[]> pos = new List<Vector2[]>();

            for (int i = 1; i <= CircleCount.val; ++i)
            {
                pos.Add(a1_skil12_getCirclePos(i * R.val, ArcLength.val));
            }

            //

            int psI = 0;

            for (int i = 0; i < pos.Count; ++i)
            {
                for (int j = 0; j < pos[i].Length; ++j)
                {
                    if (psI < skil12_ps.Count == false) skil12_ps.Add(null);

                    Projectile proj = skil12_ps[psI];
                    ++psI;
                    if (proj == null) continue;

                    int direction = i % 2 == 0 ? 1 : -1;

                    Vector2 p = pos[i][j];
                    p = Vector2.Normalize(p) * ((i + 1) * skil12_r_this + skil12_r_base_this);

                    a1_skil12_update_pos(player, proj, p, direction);
                }
            }

            //

            while (psI < skil12_ps.Count)
            {
                skil12_ps[psI]?.Kill();
                skil12_ps.RemoveAt(psI);
            }
        }

        private static void a1_skil12_update_pos(Player player, Projectile proj, Vector2 pos, int direction)
        {
            Vector2 position = pos;
            position = position.RotatedBy(Main.GameUpdateCount * Speed.val * direction);

            Vector2 velocity = Vector2.Normalize(position.RotatedBy(MathHelper.TwoPi / 4));
            velocity *= 0.001f;

            position += player.Center;

            //

            proj.Center = position;
            proj.velocity = velocity * direction;

            proj.ai[1] = skil12_size_this;

            NetMessage.SendData(27, -1, -1, null, proj.whoAmI);
        }
    }
}
