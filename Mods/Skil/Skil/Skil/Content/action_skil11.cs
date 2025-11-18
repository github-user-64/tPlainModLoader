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
    public class skil11 : PatchPlayer
    {
        //技能11, 雷云
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil11", Enable),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get2(Enable, ico:"Images/Buff_38", text:"技能11")
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil11(This);
            else a1_skil11_false(This);
        }

        private static Projectile skil11_p = null;//云
        private static Projectile skil11_attack_p = null;//雷
        private static Vector2 skil11_attack_p_targetP;//雷的目标位置
        private static int skil11_attack_count = 0;//生成几段雷
        private static int skil11_attack_cd = 0;

        public static void a1_skil11(Player player)
        {
            if (player == null || player.active == false || player.dead == true)
            {
                return;
            }

            #region
            if (skil11_p == null)
            {
                if (Main.mouseRight && Main.mouseRightRelease && player.mouseInterface == false)
                {
                    int id = Projectile.NewProjectile(null, Main.MouseWorld, Vector2.Zero, 244, 0, 0);
                    skil11_p = Main.projectile[id];
                }
            }
            else
            if (Utils.projExist(skil11_p, 244, player) == false)
            {
                int id = Projectile.NewProjectile(null, skil11_p.Center, Vector2.Zero, 244, 0, 0);
                skil11_p = Main.projectile[id];
            }
            #endregion

            if (skil11_p == null) return;

            #region
            skil11_p.ai[0] = 0;

            if (Main.mouseRight && skil11_p != null)
            {
                Vector2 distance = Main.MouseWorld - skil11_p.Center;

                if (distance != Vector2.Zero)
                {
                    Vector2 velocity = distance / 20;
                    velocity += Vector2.Normalize(distance) * 3;

                    if (velocity.Length() > distance.Length()) velocity = distance / 20;

                    skil11_p.velocity = velocity;

                    if (Main.GameUpdateCount % 2 == 0) NetMessage.SendData(27, -1, -1, null, skil11_p.whoAmI);
                }
            }
            else
            if (skil11_p.velocity != Vector2.Zero)
            {
                skil11_p.velocity /= 1.1f;

                if (Main.GameUpdateCount % 2 == 0) NetMessage.SendData(27, -1, -1, null, skil11_p.whoAmI);
            }
            #endregion

            if (skil11_attack_p == null)
            {
                --skil11_attack_cd;

                if (skil11_attack_cd < 1 && (Utils.getRand(0, 60) == 0 || (Main.mouseLeft && player.mouseInterface == false)))
                {
                    skil11_attack_cd = 0;
                    skil11_attack_count = Utils.getRand(2, 5);

                    Vector2 position = skil11_p.Center + new Vector2(0, 14);//加上云的一半高度
                    position.X += Utils.getRand(-10, 10);

                    Vector2 velocity = Vector2.UnitY * Utils.getRand(100, 400);
                    velocity = velocity.RotatedBy(MathHelper.TwoPi / 360 * Utils.getRand(-30, 30));

                    skil11_attack_p_targetP = position + velocity;

                    float ratio = 0.01f;//434射弹的velocity*[该值]=实际长度

                    int damage = skil11_attack_count * 100 + 100;
                    if (damage > 900) damage = 900;
                    else if (damage < 50) damage = 50;

                    int id = Projectile.NewProjectile(null, position, velocity * ratio, 434, damage, 1);
                    skil11_attack_p = Main.projectile[id];
                }
            }
            else
            {
                if (Vector2.Distance(skil11_attack_p.Center, skil11_attack_p_targetP) > 5)//雷没到目标位置, 说明击中了物体
                {
                    _ = Projectile.NewProjectile(null, skil11_attack_p.Center - new Vector2(0, 72), Vector2.Zero, 695, skil11_attack_p.damage, 1);//爆炸

                    _ = Projectile.NewProjectile(null, skil11_attack_p.Center, Vector2.Zero, 612, 0, 0);

                    skil11_attack_p = null;
                }
                else
                {
                    if (skil11_attack_count-- > 0)
                    {
                        Vector2 position = skil11_attack_p.Center;

                        Vector2 velocity = Vector2.UnitY * Utils.getRand(100, 400);
                        velocity = velocity.RotatedBy(MathHelper.TwoPi / 360 * Utils.getRand(-30, 30));

                        skil11_attack_p_targetP = position + velocity;

                        float ratio = 0.01f;//434射弹的velocity*[该值]=实际长度

                        int damage = skil11_attack_p.damage - 100;
                        if (damage < 50) damage = 50;

                        int id = Projectile.NewProjectile(null, position, velocity * ratio, 434, damage, 1);
                        skil11_attack_p = Main.projectile[id];
                    }
                    else
                    {
                        skil11_attack_p = null;
                    }
                }
            }
        }

        public static void a1_skil11_false(Player player)
        {
            if (player == null) return;

            skil11_p?.Kill();
            skil11_p = null;
            skil11_attack_p = null;
        }
    }
}
