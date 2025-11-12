using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;
using static Skil.Content.Utils;

namespace Skil.Content
{
    public class skil4 : ModPlayer
    {
        //技能4, 生成一圈旋转的射弹, 范围不断变大, 时间到后开始爆炸
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<float> Speed = new GetSetReset<float>(0.01f, 0.01f);//旋转速度
        public static GetSetReset<int> Time_max = new GetSetReset<int>(5000, 5000);//ai0的变大时间

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil4", Enable)
                .SkilCMDBuild("speed", Speed)
                .SkilCMDBuild("time", Time_max),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Time_max, int.Parse, "变大时间<int>", "Images/Item_1275", "技能4"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil4(This);
            else a1_skil4_false(This);
        }

        private class enterWorld : ModMain
        {
            public override void OnEnterWorld()
            {
                skil4_ps = null;
            }
        }

        protected static CDTime skil4_cd = null;
        protected static bool skil4_actioning = false;
        protected static CDTime skil4_ai0_time = null;//ai0的变大计时器
        protected static int skil4_ai1_time_max = 2000;//ai1的爆炸时间
        protected static CDTime skil4_ai1_time = null;
        protected static int skil4_ai = 0;//阶段
        protected static List<Projectile> skil4_ps = null;
        protected static int skil4_ps_countMax = 0;//记录最多的时候有几个射弹
        protected static float skil4_range = 10;//半径
        protected static float skil4_radian = 126;//射弹之间的弧度
        public static void a1_skil4(Player player)
        {
            if (player == null) return;

            if (skil4_cd == null) skil4_cd = new CDTime(2000);
            if (skil4_ai0_time == null) skil4_ai0_time = new CDTime(Time_max.val);
            if (skil4_ai1_time == null) skil4_ai1_time = new CDTime(skil4_ai1_time_max);

            if (skil4_actioning == false && skil4_cd.Ok == false)
            {
                skil4_cd.next();
                return;
            }

            if (skil4_actioning == false)
            {
                if (Terraria.Main.mouseLeft == false || player.mouseInterface == true) return;

                skil4_actioning = true;
                skil4_cd.resume();

                skil4_ps = null;//让下面给它重新初始化
            }
            //if (t.Player.isFirstActive) skil4_ps = null;

            if (Time_max.val < 1000) Time_max.val = 1000;
            if (Time_max.val > 6000) Time_max.val = 6000;
            skil4_ai0_time.setCD(Time_max.val);

            if (skil4_ps == null || (skil4_ai == 0 || skil4_ai == 1) == false)
            {
                skil4_ps = new List<Projectile>();
                skil4_ps_countMax = 0;
                skil4_ai = 0;
                skil4_cd.resume();
                skil4_ai0_time.resume();
                skil4_ai1_time.resume();
                skil4_range = 10;
            }

            //

            if (skil4_ai == 0)//计时和切换ai
            {
                if (skil4_ai0_time.Ok)//时间到后换ai
                {
                    skil4_ai = 1;
                    skil4_ai0_time.resume();
                }
                else//继续计时
                {
                    skil4_ai0_time.setCD(Time_max.val);
                    skil4_ai0_time.next();
                }
            }
            else if (skil4_ai == 1)
            {
                if (skil4_ai1_time.Ok)
                {
                    skil4_ai = 0;
                    skil4_ai1_time.resume();

                    skil4_actioning = false;
                    return;
                }
                else
                {
                    skil4_ai1_time.setCD(skil4_ai1_time_max);
                    skil4_ai1_time.next();
                }
            }

            int proj1_type = 274;//死神镰刀
            int proj1_size = 42;

            if (skil4_ai == 0 && Terraria.Main.GameUpdateCount % 3 == 0)
            {
                float startP = 50;//开始位置

                if (startP + 5 < skil4_range)
                {
                    Vector2 p = Vector2.Zero;
                    Vector2 v = new Vector2(1, 0);
                    int len = 0;

                    float angle = (MathHelper.TwoPi / 360) * getRand(0, 360);
                    v = v.RotatedBy(angle);

                    p = v * startP;
                    len = Math.Max(5, (int)(skil4_range - p.Length() - proj1_size) / 10);
                    if (len > 15) len = 15;
                    v *= getRand(5, len);

                    angle = (MathHelper.TwoPi / 360) * getRand(-30, 30);
                    v = v.RotatedBy(angle);

                    int life = Terraria.Main.GameUpdateCount % 9 == 0 ? 1 : 0;
                    Projectile.NewProjectile(null, player.Center + p, v, 298, 0, 0, ai0: player.whoAmI, ai1: life);
                }
            }

            if (skil4_ai == 0)//扩大圈并添加射弹数量
            {
                ++skil4_range;

                float perimeter = MathHelper.TwoPi * skil4_range;//周长
                int count = (int)Math.Truncate(perimeter / skil4_radian);//当前周长可以放下几个射弹

                if (count > skil4_ps_countMax)//可以放下的射弹比当前多
                {
                    //如果是当前ai那么会自动添加
                    skil4_ps.Add(null);

                    ++skil4_ps_countMax;
                }
            }

            //维持射弹
            for (int i = 0; i < skil4_ps.Count; ++i)
            {
                Projectile proj = skil4_ps[i];
                bool newP = projExist(proj, proj1_type, player) == false;
                newP |= proj?.timeLeft < 1;

                if (newP == true && skil4_ai != 0)//已经消失, 且不在当前ai就不维持了
                {
                    skil4_ps.RemoveAt(i);
                    continue;
                }

                if (proj != null && proj.timeLeft < 85)
                {
                    proj.Kill();
                    newP = true;
                }

                if (newP == false) continue;

                int id = Projectile.NewProjectile(null, player.position, Vector2.Zero,
                        proj1_type, SkilListControl1.damage.val, 0, player.whoAmI);

                skil4_ps[i] = Terraria.Main.projectile[id];
            }

            //行为
            for (int i = 0; i < skil4_ps.Count; ++i)
            {
                Projectile proj = skil4_ps[i];
                if (projExist(proj, proj1_type, player) == false) continue;

                float radians = Terraria.Main.GameUpdateCount * Speed.val;
                radians += (skil4_radian / skil4_range) * i;
                radians += ((MathHelper.TwoPi * skil4_range) - (skil4_ps_countMax * skil4_radian)) / skil4_range;

                Vector2 v = new Vector2(1, 0);
                v = v.RotatedBy(radians);
                v *= skil4_range;

                proj.Center = player.Center + v;

                NetMessage.SendData(27, -1, -1, null, proj.whoAmI);
            }

            if (skil4_ai == 1)
            {
                int cd = (int)(MathHelper.Pi * (skil4_range * skil4_range));
                if (cd <= 0) cd = 1;
                cd /= 15000;
                cd = 15 - cd;
                if (cd * 16 + 3 > skil4_ai1_time_max) cd = skil4_ai1_time_max - 3;
                if (cd <= 0) cd = 1;
                if (Terraria.Main.GameUpdateCount % cd != 0) return;

                Vector2 p = new Vector2(1, 0);

                float angle = (MathHelper.TwoPi / 360) * getRand(0, 360);
                p = p.RotatedBy(angle);

                p *= getRand(50, (int)skil4_range - proj1_size);
                p += player.Center;

                if (getRand(0, 3) == 0)
                {
                    NPC npc = getNpc(player.Center, 0, 0, (int)skil4_range, false);
                    if (npc != null) p = npc.Center;
                }

                Projectile.NewProjectile(null, p, Vector2.Zero,
                        645, SkilListControl1.damage.val, 1, player.whoAmI, 0, -1);
            }
        }
        public static void a1_skil4_false(Player player)
        {
            if (player == null) return;

            skil4_ps = null;
            skil4_actioning = false;
        }
    }
}
