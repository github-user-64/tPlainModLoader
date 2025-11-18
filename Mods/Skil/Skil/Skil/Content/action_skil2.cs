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
    public class skil2 : PatchPlayer
    {
        //技能2, 召唤一个眼球发射射弹
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> CD = new GetSetReset<int>(100, 100);
        public static int action_skil2_cd = 0;
        public static GetSetReset<int> Mode = new GetSetReset<int>(0, 0);

        protected static Projectile skil2_pet = null;
        protected static Projectile[] skil2_extraAttack_ps = null;
        protected static bool skil2_extraAttack = false;//额外攻击

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil2", Enable)
                .SkilCMDBuild("cd", CD)
                .SkilCMDBuild("mode", Mode),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Mode, int.Parse, "0: 自动(npc), 1:自动(玩家), 2:手动<int>", "Images/Buff_190", "技能2"),
                UIBuild.get6(CD, int.Parse, "<int>", "Images/Buff_190", "技能2cd"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil2(This);
            else a1_skil2_false(This);
        }

        public static void a1_skil2(Player player)
        {
            if (player == null || player.active == false || player.dead == true)
            {
                a1_skil2_false(player);
                return;
            }

            int petId = 650;

            if (Utils.projExist(skil2_pet, petId, player) == false)
            {
                int id = Projectile.NewProjectile(null, player.Center, Vector2.Zero, petId, 0, 0, player.whoAmI);
                skil2_pet = Main.projectile[id];
            }

            if (Main.GameUpdateCount % 60 == 0) NetMessage.SendData(27, -1, -1, null, skil2_pet.whoAmI);

            Vector2 targetP = Vector2.Zero;
            bool hasTarger = false;

            if (Mode.val == 0)
            {
                NPC targetNpc = Utils.getNpc(skil2_pet.Center, 5, 5, 16 * 64);
                if (targetNpc != null)
                {
                    targetP = SkilListControl1.aimAdvance.val ?
                        Utils.aimAdvance(skil2_pet.Center, SkilListControl1.aimAdvance_val.val, targetNpc.Center, targetNpc.velocity) :
                        targetNpc.Center;

                    hasTarger = true;
                }
            }
            else if (Mode.val == 1)
            {
                Player targetPlayer = Utils.getPlayer_hostile(skil2_pet.Center, player);

                if (targetPlayer != null)
                {
                    targetP = SkilListControl1.aimAdvance.val ?
                        Utils.aimAdvance(skil2_pet.Center, SkilListControl1.aimAdvance_val.val, targetPlayer.Center, targetPlayer.velocity) :
                        targetPlayer.Center;

                    hasTarger = true;
                }
            }
            else if (Mode.val == 2)
            {
                if (Main.mouseLeft == true && player.mouseInterface == false)
                {
                    targetP = Main.MouseWorld;
                    hasTarger = true;
                }
            }
            else
            {
                Mode.val = 0;
                return;
            }

            if (hasTarger)
            {
                targetP -= skil2_pet.Center;
                targetP.Normalize();

                if (targetP.HasNaNs() || targetP == Vector2.Zero) targetP = Vector2.UnitX;
            }

            //额外攻击
            if (skil2_extraAttack)
            {
                if (skil2_extraAttack_ps == null) skil2_extraAttack_ps = new Projectile[6];

                int extraAttackType = 459;
                bool hasNull = false;

                //有null的就生成
                for (int i = 0; i < skil2_extraAttack_ps.Length; ++i)
                {
                    if (skil2_extraAttack_ps[i] == null)
                    {
                        hasNull = true;

                        if (Main.GameUpdateCount % 10 != 0) break;//生成cd

                        //ai1: 射弹大小缩放
                        int id = Projectile.NewProjectile(null, skil2_pet.Center, Vector2.Zero, extraAttackType, SkilListControl1.damage.val, 1, ai1: 1);
                        skil2_extraAttack_ps[i] = Main.projectile[id];

                        break;
                    }
                }

                //围绕在宠物周围
                for (int i = 0; i < skil2_extraAttack_ps.Length; ++i)
                {
                    Projectile proj = skil2_extraAttack_ps[i];
                    if (proj == null) continue;

                    if (Utils.projExist(proj, extraAttackType, player) == false) continue;

                    float range = (skil2_pet.width / 2) + (proj.width / 2) + 10;
                    float rad = MathHelper.TwoPi / skil2_extraAttack_ps.Length;
                    Vector2 p = (rad * i).ToRotationVector2() * range;

                    proj.Center = skil2_pet.Center + p;
                    proj.timeLeft = 500;

                    NetMessage.SendData(27, -1, -1, null, proj.whoAmI);
                }

                if (hasNull == false && hasTarger == true)
                {
                    for (int i = 0; i < skil2_extraAttack_ps.Length; ++i)//发射
                    {
                        Projectile proj = skil2_extraAttack_ps[i];
                        if (Utils.projExist(proj, extraAttackType, player) == false) continue;

                        proj.velocity = targetP * 20;

                        NetMessage.SendData(27, -1, -1, null, proj.whoAmI);
                    }

                    skil2_extraAttack_ps = null;
                    skil2_extraAttack = false;
                }
            }


            if (Main.GameUpdateCount % 180 == 0 && hasTarger) skil2_extraAttack = true;

            if (Utils.cdTime(ref action_skil2_cd, CD.val) == false) return;

            if (hasTarger == false) return;

            int projId = skil3.Enable.val ? 440 : 606;

            Projectile.NewProjectile(null, skil2_pet.Center, targetP * 20, projId, SkilListControl1.damage.val, 1);
        }
        public static void a1_skil2_false(Player player)
        {
            if (player == null) return;

            skil2_pet?.Kill();
            skil2_pet = null;
            skil2_extraAttack = false;
            for (int i = 0; i < skil2_extraAttack_ps?.Length; ++i) skil2_extraAttack_ps[i]?.Kill();
            skil2_extraAttack_ps = null;
        }
    }
}
