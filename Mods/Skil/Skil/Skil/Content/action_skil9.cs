using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;

namespace Skil.Content
{
    public class skil9 : PatchPlayer
    {
        //技能9, 一圈激光围绕旋转
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> Mode = new GetSetReset<int>();
        public static GetSetReset<int> Count = new GetSetReset<int>(3, 3);
        public static GetSetReset<float> Speed = new GetSetReset<float>(0.01f, 0.01f);
        public static GetSetReset<int> CD = new GetSetReset<int>(1, 1);
        public static GetSetReset<bool> TileCollide = new GetSetReset<bool>(true, true);
        public static GetSetReset<bool> fire = new GetSetReset<bool>(true, true);
        public static GetSetReset<float> VelocityOff = new GetSetReset<float>();
        public static GetSetReset<float> R = new GetSetReset<float>(100, 100);//半径
        public static GetSetReset<int> Len = new GetSetReset<int>(100, 100);

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil9", Enable)
                .SkilCMDBuild("mode", Mode)
                .SkilCMDBuild("count", Count)
                .SkilCMDBuild("speed", Speed)
                .SkilCMDBuild("cd", CD)
                .SkilCMDBuild("tileCollide", TileCollide)
                .SkilCMDBuild("fire", fire)
                .SkilCMDBuild("velocityOff", VelocityOff)
                .SkilCMDBuild("r", R)
                .SkilCMDBuild("len", Len),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Mode, UITextBox.StringToInt, "0:自动(npc), 1:自动(玩家), 2:手动<int>", "Images/Buff_213", "技能9"),
                UIBuild.get6(Count, UITextBox.StringToInt, "<int>", "Images/Buff_213", "技能9数量"),
                UIBuild.get6(Speed, UITextBox.StringToFloat, "<int>", "Images/Buff_213", "技能9速度"),
                UIBuild.get6(CD, UITextBox.StringToInt, "<int>", "Images/Buff_213", "技能9cd"),
                UIBuild.get2(TileCollide, ico:"Images/Buff_213", text:"技能9方块碰撞"),
                UIBuild.get2(fire, ico:"Images/Buff_213", text:"技能9碰撞火花"),
                UIBuild.get6(VelocityOff, UITextBox.StringToFloat, "<int>", "Images/Buff_213", "技能9角度"),
                UIBuild.get6(R, UITextBox.StringToFloat, "<int>", "Images/Buff_213", "技能9半径"),
                UIBuild.get6(Len, UITextBox.StringToInt, "<int>", "Images/Buff_213", "技能9长度"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil9(This);
        }

        private static int skil9_i = 0;
        public static void a1_skil9(Player player)
        {
            if (player == null) return;

            if (Count.val < 1) Count.val = 1;
            if (Mode.val < 0) Mode.val = 0;
            if (Mode.val > 2) Mode.val = 2;
            if (CD.val < 1) CD.val = 1;

            if (skil9_i < 0) skil9_i = 0;
            if (skil9_i >= Count.val) skil9_i = 0;

            if (Main.GameUpdateCount % CD.val != 0) return;

            Vector2 position = player.Center;
            Vector2 velocity = Vector2.Zero;
            int len = Len.val;//正常长度
            int MaxLen = 850;//最长长度
            float ratio = 0.01f;//434射弹的velocity*[该值]=实际长度

            float radians = MathHelper.TwoPi / Count.val;//弧度
            float r = R.val;//半径
            float angle = radians * skil9_i + (Main.GameUpdateCount * Speed.val);//射弹角度

            velocity = Vector2.Normalize(angle.ToRotationVector2());
            position += velocity * r;

            velocity *= len;
            velocity = velocity.RotatedBy((MathHelper.TwoPi / 360) * VelocityOff.val);

            if (Mode.val == 0)
            {
                NPC targetNpc = Utils.getNpc(position, 3, 3, MaxLen, TileCollide.val);

                if (targetNpc != null && targetNpc.Distance(position) <= MaxLen)
                {
                    Vector2 targetP = SkilListControl1.aimAdvance.val ?
                    Utils.aimAdvance(position, SkilListControl1.aimAdvance_val.val, targetNpc.Center, targetNpc.velocity) :
                    targetNpc.Center;

                    Vector2 v = targetP - position;

                    if (v.HasNaNs() == false && v != Vector2.Zero) velocity = v;
                }
            }
            else if (Mode.val == 1)
            {
                Player targetPlayer = Utils.getPlayer_hostile(position, player);

                if (targetPlayer != null && targetPlayer.Distance(position) <= MaxLen)
                {
                    Vector2 targetP = SkilListControl1.aimAdvance.val ?
                        Utils.aimAdvance(position, SkilListControl1.aimAdvance_val.val, targetPlayer.Center, targetPlayer.velocity) :
                        targetPlayer.Center;

                    Vector2 v = targetP - position;

                    if (v.HasNaNs() == false && v != Vector2.Zero) velocity = v;
                }
            }
            else if (Mode.val == 2)
            {
                if (Main.mouseLeft && player.mouseInterface == false)
                {
                    Vector2 v = Main.MouseWorld - position;

                    if (v.Length() > MaxLen) v = Vector2.Normalize(v) * MaxLen;

                    if (v.HasNaNs() == false && v != Vector2.Zero) velocity = v;
                }
            }

            int id = Projectile.NewProjectile(null, position, velocity * ratio, 434, SkilListControl1.damage.val, 1, player.whoAmI);
            Main.projectile[id].tileCollide = TileCollide.val;

            ++skil9_i;

            if (fire.val) UpdateFire(Main.projectile[id], position + velocity, player);
            else proj = null;
        }

        private static Projectile proj = null;
        private static Vector2 projTargetP = Vector2.Zero;
        private static void UpdateFire(Projectile p, Vector2 targetP, Player player)
        {
            Projectile old = proj;
            if (Utils.projExist(old, 434, player))
            {
                proj = null;

                if (Vector2.Distance(old.position, projTargetP) > 5)
                {
                    Vector2 v = Vector2.Normalize(old.position - new Vector2(old.ai[0], old.ai[1])) * -2;

                    ParticleOrchestraType type = ParticleOrchestraType.BestReforge;
                    ParticleOrchestrator.BroadcastOrRequestParticleSpawn(type, new ParticleOrchestraSettings
                    {
                        PositionInWorld = old.Center,
                        MovementVector = v,
                    });
                }
            }

            projTargetP = targetP;
            proj = p;
        }
    }
}
