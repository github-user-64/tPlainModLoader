using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;
using static Skil.Content.Utils;

namespace Skil.Content
{
    public class skil8 : ModPlayer
    {
        //技能8, 生成法阵, 生成后发射射弹
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> Mode = new GetSetReset<int>();
        public static GetSetReset<int> Types = new GetSetReset<int>();

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil8", Enable)
                .SkilCMDBuild("mode", Mode)
                .SkilCMDBuild ("type", Types),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Mode, int.Parse, "0:朝固定方向, 1:自动(npc), 2:自动(玩家)<int>", "Images/Buff_312", "技能8"),
                UIBuild.get6(Types, int.Parse, "0-2<int>", "Images/Buff_312", "技能8法阵类型"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil8(This);
            else a1_skil8_false(This);
        }

        private static CDTime skil8_cd = new CDTime(10);//其实生成法阵的时间就够久了
        //每个List<Projectile>用于存放1个法阵, 其中第1个Projectile用于存放法阵信息, 不是实际的射弹
        private static List<List<Projectile>> skil8_ps = new List<List<Projectile>>();
        //存放正在生成的法阵的List<Projectile>, 其中第1个Projectile用于存放法阵信息.
        //处第1个用于存放法阵信息用外, 其它射弹用于存储生成信息, 射弹生成后将替换该射弹
        private static List<Projectile> skil8_ps_spawning = new List<Projectile>();
        private static bool skil8_spawning = false;
        private static bool skil8_isSpawnInfoOk = false;
        private static int skil8_i = 0;
        public static void a1_skil8(Player player)
        {
            if (player == null) return;

            if (Mode.val < 0) Mode.val = 0;
            if (Mode.val > 2) Mode.val = 2;
            if (Types.val < 0) Types.val = 0;
            if (Types.val > 2) Types.val = 2;

            //已生成法阵的行为
            a1_skil8_type2_attack_update();
            List<List<Projectile>> skil8_ps_del = new List<List<Projectile>>();
            for (int i = 0; i < skil8_ps.Count; ++i)
            {
                List<Projectile> ps = skil8_ps[i];

                if (ps == null || ps.Count < 1)
                {
                    skil8_ps_del.Add(ps);
                    continue;
                }

                Projectile info = ps[0];
                int type = info.type;
                int attackCount = info.timeLeft;
                Vector2 position = info.position;
                Vector2 velocity = info.velocity;

                //能否攻击
                if (a1_skil8_types_canAttack(type, attackCount, position, velocity) == false)
                {
                    foreach (Projectile p in ps) p?.Kill();

                    skil8_ps_del.Add(ps);
                    continue;
                }

                a1_skil8_updateInfo(info, player);

                //攻击
                switch (type)
                {
                    case 0: a1_skil8_type0_attack(ref attackCount, position, velocity); break;
                    case 1: a1_skil8_type1_attack(ref attackCount, position, velocity); break;
                    case 2: a1_skil8_type2_attack(ref attackCount, position, velocity); break;
                    default: break;
                }

                info.timeLeft = attackCount;
            }
            for (int i = 0; i < skil8_ps_del.Count; ++i)
            {
                skil8_ps.Remove(skil8_ps_del[i]);
            }

            //下面都是生成法阵用的

            if (skil8_spawning == false)
            {
                if (Main.mouseLeft && player.mouseInterface == false && skil8_cd.Ok)
                {
                    skil8_spawning = true;
                    skil8_isSpawnInfoOk = false;

                    skil8_ps_spawning = null;

                    skil8_i = 0;

                    skil8_cd.resume();
                }
                else
                {
                    if (skil8_cd.Ok == false) skil8_cd.next();
                    return;
                }
            }

            //生成要生成射弹的信息
            if (skil8_isSpawnInfoOk == false)
            {
                Vector2 spawnP = player.Center;
                float spawnAngle = player.Center.AngleTo(Main.MouseWorld);

                //spawnAngle += (MathHelper.TwoPi / 360) * -18;//偏移

                int type = Types.val;

                skil8_ps_spawning =
                    type == 0 ? a1_skil8_type0_getSpawnInfo(spawnP, spawnAngle) :
                    type == 1 ? a1_skil8_type1_getSpawnInfo(spawnP, spawnAngle) :
                    type == 2 ? a1_skil8_type2_getSpawnInfo(spawnP, spawnAngle) :
                    a1_skil8_type0_getSpawnInfo(spawnP, spawnAngle);

                //第一个用于存放法阵信息
                skil8_ps_spawning.Insert(0, new Projectile()
                {
                    type = type,
                    position = spawnP,
                    velocity = spawnAngle.ToRotationVector2(),
                    timeLeft = 0,
                });

                skil8_isSpawnInfoOk = true;
            }

            if (skil8_i >= skil8_ps_spawning.Count - 1)
            {
                //生成法阵完成

                skil8_ps.Add(skil8_ps_spawning);
                skil8_ps_spawning = null;
                skil8_spawning = false;//生成结束
            }
            else
            {
                //生成法阵

                Projectile projInfo = skil8_ps_spawning[skil8_i + 1];

                int cd =
                    skil8_ps_spawning[0].type == 0 ? 1 :
                    skil8_ps_spawning[0].type == 1 ? 3 :
                    skil8_ps_spawning[0].type == 2 ? 5 :
                    1;

                if (Main.GameUpdateCount % cd != 0) return;

                int id = Projectile.NewProjectile(null, projInfo.position, projInfo.velocity, projInfo.type, 0, 0,
                    ai1: projInfo.ai[1]);

                NetMessage.SendData(27, -1, -1, null, id);

                skil8_ps_spawning[skil8_i + 1] = Main.projectile[id];

                ++skil8_i;
            }
        }
        public static void a1_skil8_false(Player player)
        {
            if (player == null) return;

            skil8_spawning = false;
            skil8_ps.Clear();

            a1_skil8_type2_attack_update_ps.Clear();
        }
        private static void a1_skil8_updateInfo(Projectile info, Player player)
        {
            int type = info.type;
            int attackCount = info.timeLeft;
            Vector2 position = info.position;
            Vector2 velocity = info.velocity;

            if (type == 0 || type == 1)
            {
                if (Mode.val == 1)
                {
                    NPC targetNpc = getNpc(position, 0, 0, 20 * 64, false);

                    //修改攻击方向
                    if (targetNpc != null)
                    {
                        Vector2 targetP = SkilListControl1.aimAdvance.val ?
                            aimAdvance(position, SkilListControl1.aimAdvance_val.val, targetNpc.Center, targetNpc.velocity) :
                            targetNpc.Center;

                        velocity = targetP - position;

                        if (velocity.HasNaNs() || velocity == Vector2.Zero) velocity = Vector2.UnitX;

                        info.velocity = velocity;
                    }
                }
                else if (Mode.val == 2)
                {
                    Player targetPlayer = getPlayer_hostile(position, player);

                    //修改攻击方向
                    if (targetPlayer != null)
                    {
                        Vector2 targetP = SkilListControl1.aimAdvance.val ?
                            aimAdvance(position, SkilListControl1.aimAdvance_val.val, targetPlayer.Center, targetPlayer.velocity) :
                            targetPlayer.Center;

                        velocity = targetP - position;

                        if (velocity.HasNaNs() || velocity == Vector2.Zero) velocity = Vector2.UnitX;

                        info.velocity = velocity;
                    }
                }
            }
            else if (type == 2)
            {
                NPC targetNpc = getNpc(position, 0, 0, 800, false);
                Player targetPlayer = getPlayer_hostile(position, player);

                Vector2 v = Vector2.Zero;
                float minLen = 800;

                if (targetNpc != null && targetNpc.Center.HasNaNs() == false)
                {
                    Vector2 v2 = targetNpc.Center - position;
                    if (v2.Length() < minLen)
                    {
                        v = v2;
                        minLen = v2.Length();
                    }
                }
                if (targetPlayer != null && targetPlayer.Center.HasNaNs() == false)
                {
                    Vector2 v2 = targetPlayer.Center - position;
                    if (v2.Length() < minLen)
                    {
                        v = v2;
                    }
                }

                info.velocity = v;
            }
        }
        private static bool a1_skil8_types_canAttack(int type, int attackCount, Vector2 position, Vector2 velocity)
        {
            return type == 0 ? attackCount < 10 :
                type == 1 ? attackCount < 60 :
                type == 2 ? attackCount < 70 :
                false;
        }
        private static List<Projectile> a1_skil8_type0_getSpawnInfo(Vector2 spawnP, float spawnAngle)
        {
            List<Projectile> spawnInfo = new List<Projectile>();

            //圈
            for (int i = 0, shape_count = 5; i < shape_count * 2; ++i)
            {
                Vector2 p = Vector2.Zero;
                Vector2 v = Vector2.Zero;

                float angleOff = i < shape_count ? 0 : MathHelper.TwoPi / shape_count / 2;//生成的第2个要换角度
                float radians = (float)((Math.PI / 180) * Math.Round(360f / shape_count));//弧度
                float r = 95;//半径
                float x = p.X + r * (float)Math.Sin(radians * i);
                float y = p.Y + r * (float)Math.Cos(radians * i);
                float x2 = p.X + r * (float)Math.Sin(radians * (i + 1));
                float y2 = p.Y + r * (float)Math.Cos(radians * (i + 1));

                p = new Vector2(x, y);
                v = new Vector2(x2, y2).DirectionTo(new Vector2(x, y)) / 999;

                p = p.RotatedBy(spawnAngle + angleOff);
                v = v.RotatedBy(spawnAngle + angleOff);

                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p + spawnP,
                    velocity = v,
                    ai = new float[] { 0, 1, 0 },
                });
            }

            //五角星
            for (int i = 0, shape_count = 5; i < shape_count; ++i)
            {
                Vector2 p = Vector2.Zero;
                Vector2 p2 = Vector2.Zero;
                Vector2 v = Vector2.Zero;

                float angleOff = 0;
                float radians = MathHelper.TwoPi / shape_count;//弧度
                float angle = (MathHelper.TwoPi / 360) * 18;//用于和圈对其的偏转角度
                float r = 93;//五角星的角距离中心的距离

                p += new Vector2(r, 0).RotatedBy(radians * i + angle);
                p2 += new Vector2(r, 0).RotatedBy(radians * (i + 2) + angle);
                v = p2.DirectionTo(p) / 999;

                p = p.RotatedBy(spawnAngle + angleOff);
                p2 = p2.RotatedBy(spawnAngle + angleOff);
                v = v.RotatedBy(spawnAngle + angleOff);

                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p + spawnP,
                    velocity = v,
                    ai = new float[] { 0, 1, 0 },
                });

                //长度不够, 再生成一个
                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p2 + spawnP,
                    velocity = Vector2.Negate(v),
                    ai = new float[] { 0, 1, 0 },
                });
            }

            //电圈发射器
            spawnInfo.Add(new Projectile()
            {
                type = 443,
                position = spawnP,
                velocity = Vector2.Zero,
                ai = new float[] { 0, 0, 0 },
            });

            return spawnInfo;
        }
        private static void a1_skil8_type0_attack(ref int attackCount, Vector2 position, Vector2 velocity)
        {
            if (Main.GameUpdateCount % 8 == 0)
            {
                velocity = Vector2.Normalize(velocity) * 80;
                float scale = 1 + getRandFloat();
                //612日耀ai1设置太大在服务器会出不来
                //953破晓ai1小于1大于2会在服务器出不来
                //978火山
                int id = Projectile.NewProjectile(null, position, velocity, 953, SkilListControl1.damage.val, 1, ai1: scale);
                NetMessage.SendData(27, -1, -1, null, id);

                ++attackCount;
            }
        }
        private static List<Projectile> a1_skil8_type1_getSpawnInfo(Vector2 spawnP, float spawnAngle)
        {
            List<Projectile> spawnInfo = new List<Projectile>();

            //圈
            for (int i = 0, shape_count = 6; i < shape_count * 2; ++i)
            {
                Vector2 p = Vector2.Zero;
                Vector2 v = Vector2.Zero;

                float angleOff = i < shape_count ? 0 : MathHelper.TwoPi / shape_count / 2;//生成的第2个要换角度
                float radians = (float)((Math.PI / 180) * Math.Round(360f / shape_count));//弧度
                float r = 110;//半径
                float x = p.X + r * (float)Math.Sin(radians * i);
                float y = p.Y + r * (float)Math.Cos(radians * i);
                float x2 = p.X + r * (float)Math.Sin(radians * (i + 1));
                float y2 = p.Y + r * (float)Math.Cos(radians * (i + 1));

                p = new Vector2(x, y);
                v = new Vector2(x2, y2).DirectionTo(new Vector2(x, y)) / 999;

                p = p.RotatedBy(spawnAngle + angleOff);
                v = v.RotatedBy(spawnAngle + angleOff);

                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p + spawnP,
                    velocity = v,
                    ai = new float[] { 0, 1, 0 },
                });
            }

            //三角形
            for (int i = 0, shape_count = 3; i < shape_count * 2; ++i)
            {
                Vector2 p = Vector2.Zero;
                Vector2 p2 = Vector2.Zero;
                Vector2 v = Vector2.Zero;

                float angleOff = i < shape_count ? 0 : MathHelper.TwoPi / shape_count / 2;//生成的第2个要换角度
                float radians = MathHelper.TwoPi / shape_count;//弧度
                float angle = (MathHelper.TwoPi / 360) * 30;//用于对齐的偏转角度
                float r = 108;//三角形的角距离中心的距离

                p += new Vector2(r, 0).RotatedBy(radians * i + angle);
                p2 += new Vector2(r, 0).RotatedBy(radians * (i + 1) + angle);
                v = p2.DirectionTo(p) / 999;

                p = p.RotatedBy(spawnAngle + angleOff);
                p2 = p2.RotatedBy(spawnAngle + angleOff);
                v = v.RotatedBy(spawnAngle + angleOff);

                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p + spawnP,
                    velocity = v,
                    ai = new float[] { 0, 1, 0 },
                });

                //长度不够, 再生成一个
                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p2 + spawnP,
                    velocity = Vector2.Negate(v),
                    ai = new float[] { 0, 1, 0 },
                });

                //恶魔镰刀, 貌似只要不移动就不会判定碰到方块
                spawnInfo.Add(new Projectile()
                {
                    type = 45,
                    position = p + spawnP,
                    velocity = Vector2.Zero,
                    ai = new float[] { 0, 0, 0 },
                });
            }

            return spawnInfo;
        }
        private static void a1_skil8_type1_attack(ref int attackCount, Vector2 position, Vector2 velocity)
        {
            if (Main.GameUpdateCount % 5 == 0)
            {
                ParticleOrchestrator.BroadcastOrRequestParticleSpawn((ParticleOrchestraType)10, new ParticleOrchestraSettings
                {
                    PositionInWorld = position,
                });
            }
            if (Main.GameUpdateCount % 2 == 0)
            {
                velocity = Vector2.Normalize(velocity) * 80;

                float scale = 5;
                int offSize = 10;
                Vector2 off = velocity.RotatedBy(MathHelper.TwoPi / 4);

                position -= off * ((offSize / 2) / scale);
                position += off * (getRand(0, offSize) / scale);

                position += Vector2.Normalize(velocity) * 80;//向前一点

                //974魔光剑, ai0用于修改大小
                int id = Projectile.NewProjectile(null, position, velocity, 974, SkilListControl1.damage.val, 1, ai0: scale);
                //NetMessage.SendData(27, -1, -1, null, id);

                ++attackCount;
            }
        }
        private static List<Projectile> a1_skil8_type2_getSpawnInfo(Vector2 spawnP, float spawnAngle)
        {
            List<Projectile> spawnInfo = new List<Projectile>();

            //圈
            for (int i = 0, shape_count = 6; i < shape_count * 1; ++i)
            {
                Vector2 p = Vector2.Zero;
                Vector2 p2 = Vector2.Zero;
                Vector2 v = Vector2.Zero;

                float angleOff = i < shape_count ? 0 : MathHelper.TwoPi / shape_count / 1;//生成的第2个要换角度
                float radians = MathHelper.TwoPi / shape_count;//弧度
                float r = 110;//半径
                p = Vector2.UnitX.RotatedBy(radians * i) * r;
                p2 = Vector2.UnitX.RotatedBy(radians * (i + 1)) * r;

                v = p2.DirectionTo(p) / 999;

                p = p.RotatedBy(spawnAngle + angleOff);
                v = v.RotatedBy(spawnAngle + angleOff);

                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p + spawnP,
                    velocity = v,
                    ai = new float[] { 0, 1, 0 },
                });
            }

            //圈外围
            for (int i = 0, shape_count = 6; i < shape_count; ++i)
            {
                Vector2 p = Vector2.UnitX;

                float angle = MathHelper.TwoPi / 4;//用于对齐的偏转角度
                float radians = MathHelper.TwoPi / shape_count;//弧度
                float r = 108;//半径

                p = p.RotatedBy(i * radians + angle + spawnAngle) * r;

                spawnInfo.Add(new Projectile()
                {
                    type = 967,
                    position = p + spawnP,
                    velocity = Vector2.Normalize(p) / 999,
                    ai = new float[] { 0, 0, 0 },
                });
            }

            //三角形
            for (int i = 0, shape_count = 3; i < shape_count * 2; ++i)
            {
                Vector2 p = Vector2.Zero;
                Vector2 p2 = Vector2.Zero;
                Vector2 v = Vector2.Zero;

                float angleOff = i < shape_count ? 0 : MathHelper.TwoPi / shape_count / 2;//生成的第2个要换角度
                float radians = MathHelper.TwoPi / shape_count;//弧度
                float angle = (MathHelper.TwoPi / 360) * 30;//用于对齐的偏转角度
                float r = 108;//三角形的角距离中心的距离

                p += new Vector2(r, 0).RotatedBy(radians * i + angle);
                p2 += new Vector2(r, 0).RotatedBy(radians * (i + 1) + angle);
                v = p2.DirectionTo(p) / 999;

                p = p.RotatedBy(spawnAngle + angleOff);
                p2 = p2.RotatedBy(spawnAngle + angleOff);
                v = v.RotatedBy(spawnAngle + angleOff);

                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p + spawnP,
                    velocity = v,
                    ai = new float[] { 0, 1, 0 },
                });

                //长度不够, 再生成一个
                spawnInfo.Add(new Projectile()
                {
                    type = 876,
                    position = p2 + spawnP,
                    velocity = Vector2.Negate(v),
                    ai = new float[] { 0, 1, 0 },
                });
            }

            //圈
            for (int i = 0, shape_count = 12; i < shape_count; ++i)
            {
                Vector2 p = Vector2.UnitX;

                float radians = MathHelper.TwoPi / shape_count;//弧度
                float r = 80;//半径

                p = p.RotatedBy(i * radians + spawnAngle) * r;

                spawnInfo.Add(new Projectile()
                {
                    type = 712,
                    position = p + spawnP,
                    velocity = Vector2.Zero,
                    ai = new float[] { 0, 0, 0 },
                });
            }

            //spawnInfo.Add(new Projectile()
            //{
            //    type = 254,
            //    position = spawnP,
            //    velocity = Vector2.Zero,
            //    ai = new float[] { 0, 0, 0 },
            //});

            return spawnInfo;
        }
        private static List<Projectile> a1_skil8_type2_attack_update_ps = new List<Projectile>();
        private static void a1_skil8_type2_attack_update()
        {
            List<Projectile> ps = a1_skil8_type2_attack_update_ps;

            for (int i = 0; i < ps.Count; ++i)
            {
                Projectile p = ps[i];

                if (p.active)
                {
                    if (p.type == 434)
                    {
                        if (p?.localAI[2] > 0) continue;
                    }
                    else
                    {

                    }
                }
                else
                {

                }

                Projectile.NewProjectile(null, p.Center, Vector2.Normalize(p.Center - new Vector2(p.ai[0], p.ai[1])) * 4, 978, p.damage, 1);

                ps.RemoveAt(i);
                i--;
            }

            //已生成的雷
            for (int i = 0; i < ps.Count; ++i)
            {
                Projectile p = ps[i];

                Vector2 v = (p.Center - new Vector2(p.ai[0], p.ai[1])).RotatedBy(MathHelper.TwoPi / 360 * getRand(-45, 45));
                v.Normalize();
                v *= getRand(50, 300) * 0.01f;

                int id = Projectile.NewProjectile(null, p.Center, v,
                    p.type, p.damage - 100, p.knockBack);
                ps[i] = Main.projectile[id];
                ps[i].localAI[2] = p.localAI[2] - 1;
                ps[i].tileCollide = false;
            }
        }
        private static void a1_skil8_type2_attack(ref int attackCount, Vector2 position, Vector2 velocity)
        {
            if (Main.GameUpdateCount % 5 == 0)
            {
                ParticleOrchestrator.BroadcastOrRequestParticleSpawn((ParticleOrchestraType)14, new ParticleOrchestraSettings
                {
                    PositionInWorld = position,
                });
            }
            if (Main.GameUpdateCount % 4 == 0)
            {
                if (velocity == Vector2.Zero) velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 360 * getRand(0, 360));
                else velocity = velocity.RotatedBy(MathHelper.TwoPi / 360 * getRand(-45, 45));

                velocity = Vector2.Normalize(velocity) * getRand(50, 300) * 0.01f;

                int id = Projectile.NewProjectile(null, position, velocity, 434, 700, 1);
                Projectile p = Main.projectile[id];
                p.localAI[2] = getRand(2, 4);
                p.tileCollide = false;

                a1_skil8_type2_attack_update_ps.Add(p);

                ++attackCount;
            }
        }
    }
}
