using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace Skil.Content
{
    public class skil13 : ModPlayer
    {
        //技能13, 光女
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<string> Sequence = new GetSetReset<string>();//技能顺序

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get1("skil13", Enable, Sequence, new CommandString()),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Sequence, v => v, "顺序0-2<string>", "Images/Gore_1262", "技能13"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil13(This);
            else a1_skil13_false(This);
        }

        private static bool skil13_spawning = false;
        private static bool skil13_spawn_isOne = true;
        private static Vector2 skil13_targetP;
        private static int skil13_sequenceIndex = 0;

        public static void a1_skil13(Player player)
        {
            if (player == null) return;

            if (player.active == false || player.dead == true) a1_skil13_false(player);

            if (Sequence.val == null) Sequence.val = "200011";

            if (skil13_spawning == false)
            {
                if (Main.mouseLeft && player.mouseInterface == false)
                {
                    skil13_targetP = Main.MouseWorld;

                    skil13_spawn_isOne = true;
                    skil13_spawning = true;
                }
            }
            else
            {
                bool isOk = false;
                bool isOne = skil13_spawn_isOne;
                skil13_spawn_isOne = false;

                if (skil13_sequenceIndex >= Sequence.val.Length) skil13_sequenceIndex = 0;

                if (Sequence.val.Length == 0)
                {
                    skil13_spawning = false;
                }
                else
                {
                    switch (Sequence.val[skil13_sequenceIndex])
                    {
                        case '0': isOk = a1_skil13_type0_update(isOne, skil13_targetP); break;
                        case '1': isOk = a1_skil13_type1_update(isOne, skil13_targetP); break;
                        case '2': isOk = a1_skil13_type2_update(isOne, skil13_targetP); break;
                        default: isOk = true; break;
                    }

                    if (isOk)
                    {
                        skil13_spawning = false;
                        skil13_spawn_isOne = true;

                        ++skil13_sequenceIndex;
                    }
                }
            }
        }

        public static void a1_skil13_false(Player player)
        {
            if (player == null) return;

            skil13_spawning = false;
            skil13_sequenceIndex = 0;

            foreach (Projectile i in skil13_types_clearP) i?.Kill();
            skil13_types_clearP.Clear();
        }

        //生成一条线, 每次调用生成一条线段, 要生成新的线one需为true
        private static bool skil13_spawnLen_lock_start = false;
        private static Vector2 skil13_spawnLen_lock_p;
        private static Vector2 skil13_spawnLen_lock_v;
        private static int skil13_spawnLen_lock_count = 0;
        private static Projectile a1_skil13_spawnLen_lock(bool one = false, float x = 0, float y = 0, float vx = 0, float vy = 0)
        {
            if (one)
            {
                skil13_spawnLen_lock_p = new Vector2(x, y);
                skil13_spawnLen_lock_v = new Vector2(vx, vy);
                skil13_spawnLen_lock_v.Normalize();
                skil13_spawnLen_lock_count = 0;

                skil13_spawnLen_lock_start = true;
            }
            else if (skil13_spawnLen_lock_start == false)
            {
                return null;
            }

            int imgHeight = 80 + 33;

            Vector2 p = skil13_spawnLen_lock_p + (skil13_spawnLen_lock_v * skil13_spawnLen_lock_count * imgHeight);
            Vector2 v = skil13_spawnLen_lock_v / 999;

            ++skil13_spawnLen_lock_count;

            int id = Projectile.NewProjectile(null, p, -v, 876, 0, 0);

            return Main.projectile[id];
        }

        private static float skil13_spawnProj_color = 0;
        private static void a1_skil13_spawnProj932(Vector2 p, Vector2 v)
        {
            Projectile.NewProjectile(null, p, v, 932, SkilListControl1.damage.val, 1, ai1: skil13_spawnProj_color);

            if ((skil13_spawnProj_color += 0.1f) > 1) skil13_spawnProj_color = 0;
        }

        private static int skil13_types_state = 0;
        private static int skil13_types_lineLenThis = 0;
        private static int skil13_types_lineRowThis = 0;
        private static List<Projectile> skil13_types_spawnInfo = new List<Projectile>();
        private static List<Projectile> skil13_types_clearP = new List<Projectile>();
        private static bool a1_skil13_types_update(bool isOne, Vector2 spawnP, Vector2 spawnV, int projSpeed, int lineLen, int lineRow, out bool lineNext)
        {
            lineNext = false;

            if (isOne)
            {
                skil13_types_lineLenThis = 0;
                skil13_types_lineRowThis = 0;

                skil13_types_spawnInfo.Clear();

                skil13_types_state = 0;
            }

            if (skil13_types_state == 0)
            {
                Projectile p = null;

                if (skil13_types_lineLenThis == 0)
                {
                    skil13_types_spawnInfo.Add(new Projectile() { Center = spawnP, velocity = spawnV });

                    p = a1_skil13_spawnLen_lock(true, spawnP.X, spawnP.Y, spawnV.X, spawnV.Y);
                }
                else
                {
                    p = a1_skil13_spawnLen_lock();
                }

                if (p != null) skil13_types_clearP.Add(p);

                if (++skil13_types_lineLenThis >= lineLen)
                {
                    lineNext = true;

                    skil13_types_lineLenThis = 0;

                    if (++skil13_types_lineRowThis >= lineRow)
                    {
                        skil13_types_state = 1;
                    }
                }
            }
            else
            if (skil13_types_state == 1)
            {
                if (skil13_types_spawnInfo.Count > 0)
                {
                    Projectile info = skil13_types_spawnInfo.First();
                    skil13_types_spawnInfo.RemoveAt(0);

                    a1_skil13_spawnProj932(info.Center, Vector2.Normalize(info.velocity) * projSpeed);
                }
                else
                {
                    skil13_types_state = 0;

                    foreach (Projectile i in skil13_types_clearP) i?.Kill();
                    skil13_types_clearP.Clear();

                    return true;
                }
            }

            return false;
        }

        private static Vector2 skil13_type0_p;
        private static Vector2 skil13_type0_pOff;
        private static Vector2 skil13_type0_v;
        private static bool a1_skil13_type0_update(bool isOne, Vector2 targetP)
        {
            int lineLen = 10;
            int lineRow = 5;

            if (isOne)
            {
                int lineMargin = 200;

                skil13_type0_v = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 360 * Utils.getRand(0, 360));
                skil13_type0_v.Normalize();

                skil13_type0_pOff = skil13_type0_v.RotatedBy(MathHelper.TwoPi / 4) * lineMargin;

                skil13_type0_p = targetP - (skil13_type0_v * 500);
                skil13_type0_p -= skil13_type0_pOff * (lineRow - 1) / 2;
            }

            Vector2 spawnP = skil13_type0_p;
            Vector2 spawnV = skil13_type0_v;

            bool isOk = a1_skil13_types_update(isOne, spawnP, spawnV, 44, lineLen, lineRow, out bool lineNext);

            if (isOk) return true;

            if (lineNext)
            {
                skil13_type0_p += skil13_type0_pOff;
            }

            return false;
        }

        private static Vector2 skil13_type1_p;
        private static Vector2 skil13_type1_pOff;
        private static Vector2 skil13_type1_v;
        private static int skil13_type1_type = 0;
        private static int skil13_type1_lineRowThis = 0;
        private static bool a1_skil13_type1_update(bool isOne, Vector2 targetP)
        {
            int lineLen = 8;
            int lineRow = 7;

            if (isOne)
            {
                int lineMargin = 100;

                if (++skil13_type1_type > 1) skil13_type1_type = 0;

                skil13_type1_lineRowThis = 0;

                bool reverse = Utils.getRand(0, 2) == 0;

                skil13_type1_v = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 4 * skil13_type1_type);
                skil13_type1_v.Normalize();
                if (reverse) skil13_type1_v = -skil13_type1_v;

                skil13_type1_pOff = skil13_type1_v.RotatedBy(MathHelper.TwoPi / 4) * lineMargin;
                if (reverse) skil13_type1_pOff = -skil13_type1_pOff;

                skil13_type1_p = targetP - (skil13_type1_v * 400);
                if (skil13_type1_type == 0) skil13_type1_p -= skil13_type1_pOff * (lineRow - 1) / 2;
            }

            Vector2 spawnP = skil13_type1_p;
            Vector2 spawnV = targetP - spawnP;
            spawnV = spawnV.RotatedBy((skil13_type1_lineRowThis - (lineRow / 2)) / 30f);

            bool isOk = a1_skil13_types_update(isOne, spawnP, spawnV, 55, lineLen, lineRow, out bool lineNext);

            if (isOk) return true;

            if (lineNext)
            {
                skil13_type1_p += skil13_type1_pOff;

                ++skil13_type1_lineRowThis;
            }

            return false;
        }

        private static int skil13_type2_countThis = 0;
        private static bool a1_skil13_type2_update(bool isOne, Vector2 targetP)
        {
            int count = 30;

            if (isOne)
            {
                skil13_type2_countThis = 0;
            }

            Vector2 off = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / count * skil13_type2_countThis) * 555;

            Vector2 spawnP = targetP + off;
            Vector2 spawnV = off.RotatedBy(2.5);

            bool isOk = a1_skil13_types_update(isOne, spawnP, spawnV, 44, 1, count, out bool lineNext);

            if (isOk) return true;

            ++skil13_type2_countThis;

            return false;
        }
    }
}
