using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;
using static Skil.Content.Utils;

namespace Skil.Content
{
    public class skil3 : ModPlayer
    {
        //技能3, 召唤一个眼球激光扫射
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> Mode = new GetSetReset<int>(0, 0);
        public static GetSetReset<int> CD = new GetSetReset<int>(1000, 1000);

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil3", Enable)
                .SkilCMDBuild("mode", Mode)
                .SkilCMDBuild("cd", CD),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Mode, int.Parse, "0: 自动(npc), 1:自动(玩家), 2:手动<int>", "Images/Buff_285", "技能3"),
                UIBuild.get6(CD, int.Parse, "<int>", "Images/Buff_285", "技能3cd"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil3(This);
            else a1_skil3_false(This);
        }

        private static Projectile skil3_pet = null;
        private static int skil3_state = 0;
        private static Vector2 skil3_target = Vector2.Zero;
        public static void a1_skil3(Player player)
        {
            if (player == null || player.active == false || player.dead == true)
            {
                a1_skil3_false(player);
                return;
            }

            if (Mode.val < 0) Mode.val = 0;
            if (Mode.val > 2) Mode.val = 2;
            if (CD.val < 1) CD.val = 1;
            if (CD.val > 5000) CD.val = 5000;
            skil3_state0_cd.setCD(CD.val);

            int petId = 882;

            if (projExist(skil3_pet, petId, player) == false)
            {
                int id = Projectile.NewProjectile(null, player.Center, Vector2.Zero, petId, 0, 0, player.whoAmI);
                skil3_pet = Main.projectile[id];
            }
            if (skil3_pet == null) return;

            switch (skil3_state)
            {
                case 0: a1_skil3_state0(player); break;
                case 1: a1_skil3_state1(player); break;
                default: skil3_state = 0; break;
            }
        }

        public static void a1_skil3_false(Player player)
        {
            if (player == null) return;

            skil3_state = 0;
            skil3_state0_cd.resume();
            skil3_state1_ing = false;
            skil3_state1_oldP = null;

            skil3_pet?.Kill();
            skil3_pet = null;
        }

        private static CDTime skil3_state0_cd = new CDTime(1000);
        private static void a1_skil3_state0(Player player)
        {
            if (skil3_state0_cd.Ok == false)
            {
                skil3_state0_cd.next();
                return;
            }

            Vector2 position = skil3_pet.Center;
            int MaxLen = 900;

            skil3_target = new Vector2(float.NaN);

            if (Mode.val == 0)
            {
                NPC targetNpc = getNpc(position, 3, 3, MaxLen, false);

                if (targetNpc != null && targetNpc.Distance(position) <= MaxLen)
                {
                    skil3_target = targetNpc.Center;
                }
            }
            else if (Mode.val == 1)
            {
                Player targetPlayer = getPlayer_hostile(position, player);

                if (targetPlayer != null && targetPlayer.Distance(position) <= MaxLen)
                {
                    skil3_target = targetPlayer.Center;
                }
            }
            else if (Mode.val == 2)
            {
                if (Main.mouseLeft && player.mouseInterface == false)
                {
                    skil3_target = Main.MouseWorld;
                }
            }

            if (skil3_target.HasNaNs()) return;

            skil3_state = 1;
            skil3_state0_cd.resume();
        }

        private static Projectile skil3_state1_oldP = null;
        private static float skil3_state1_angle = 0;
        private static float skil3_state1_angleOff = 0;
        private static int skil3_state1_angleDirection = 0;
        private static bool skil3_state1_ing = false;
        private static int skil3_state1_damage = 0;
        private static void a1_skil3_state1(Player player)
        {
            float offAngle = MathHelper.Pi / 180 * 45;

            if (skil3_state1_ing == false)
            {
                Vector2 petP = skil3_pet.Center;

                if (petP.HasNaNs() || skil3_target.HasNaNs())
                {
                    skil3_state = 0;
                    return;
                }

                Vector2 v = skil3_target - petP;

                if (v.HasNaNs())
                {
                    skil3_state = 0;
                    return;
                }

                if (v == Vector2.Zero) v = Vector2.UnitX;

                skil3_state1_angle = v.ToRotation();
                skil3_state1_angleDirection = getRand(0, 2) == 0 ? -1 : 1;
                skil3_state1_angleOff = offAngle * skil3_state1_angleDirection;

                skil3_state1_damage = 50;

                skil3_state1_ing = true;
            }

            skil3_state1_oldP?.Kill();

            skil3_state1_angleOff -= MathHelper.TwoPi / 360 * skil3_state1_angleDirection;

            if (skil3_state1_angleOff * skil3_state1_angleDirection < -offAngle)
            {
                skil3_state1_ing = false;

                skil3_state = 0;
                return;
            }

            float ratio = 0.01f;//434射弹的velocity*[该值]=实际长度

            Vector2 velocity = (skil3_state1_angle + skil3_state1_angleOff).ToRotationVector2();
            velocity.Normalize();
            velocity *= 900 * ratio;

            Vector2 position = skil3_pet.Center;
            position.X += 5 * player.direction;
            position.Y -= 2;

            if (skil3_state1_damage < SkilListControl1.damage.val)
            {
                int d = SkilListControl1.damage.val / 30;
                if (d < 1) d = 1;
                if (d > 50) d = 50;
                skil3_state1_damage += d;
                if (skil3_state1_damage > SkilListControl1.damage.val) skil3_state1_damage = SkilListControl1.damage.val;
            }

            int id = Projectile.NewProjectile(null, position, velocity, 434, skil3_state1_damage, 1);
            skil3_state1_oldP = Main.projectile[id];
            skil3_state1_oldP.tileCollide = false;
        }
    }
}
