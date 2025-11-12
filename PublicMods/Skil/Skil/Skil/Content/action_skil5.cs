using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using static Skil.Content.Utils;

namespace Skil.Content
{
    public class skil5 : ModPlayer
    {
        //技能5, 从两侧生成一堆射弹
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> Size = new GetSetReset<int>(16, 16);//射弹生成范围, 单位(格)
        public static GetSetReset<float> ShootSpeed = new GetSetReset<float>(22, 22);
        public static GetSetReset<int> ShootId = new GetSetReset<int>();
        public static GetSetReset<float> Ai0 = new GetSetReset<float>();
        public static GetSetReset<float> Ai1 = new GetSetReset<float>();
        public static GetSetReset<float> Ai2 = new GetSetReset<float>();
        public static GetSetReset<int> Mode = new GetSetReset<int>();

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil5", Enable)
                .SkilCMDBuild("size", Size)
                .SkilCMDBuild("shootSpeed", ShootSpeed)
                .SkilCMDBuild("shootId", ShootId)
                .SkilCMDBuild("ai0", Ai0)
                .SkilCMDBuild("ai1", Ai1)
                .SkilCMDBuild("ai2", Ai2)
                .SkilCMDBuild("mode", Mode),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Size, int.Parse, "范围<int>", "Images/Buff_322", "技能5"),
                UIBuild.get6(Mode, int.Parse, "0: 宝石法杖, 1:光束, 2:日暮, 3:自定义<int>", "Images/Buff_322", "技能5模式"),
                UIBuild.get6(ShootSpeed, float.Parse, "<float>", "Images/Buff_322", "技能5射弹速度"),
                UIBuild.get6(ShootId, int.Parse, "<int>", "Images/Buff_322", "技能5射弹id"),
                UIBuild.get6(Ai0, float.Parse, "<float>", "Images/Buff_322", "技能5射弹ai0"),
                UIBuild.get6(Ai1, float.Parse, "<float>", "Images/Buff_322", "技能5射弹ai1"),
                UIBuild.get6(Ai2, float.Parse, "<float>", "Images/Buff_322", "技能5射弹ai2"),
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil5(This);
        }

        protected static int[] skil5_projIds1 = new int[] { 121, 122, 123, 124, 125, 126 };//宝石法杖
        protected static int[] skil5_projIds2 = new int[] { 116, 132, 156, 157 };//光束
        protected static int[] skil5_projIds3 = new int[] { 932 };//暮光长枪
        protected static int[] skil5_projIds4 = new int[] { 1 };//自定义
        protected static CDTime skil5_time = new CDTime(0);
        public static void a1_skil5(Player player)
        {
            if (player == null) return;

            if (Main.mouseLeft == false || player.mouseInterface == true) return;

            if (Size.val < 1) Size.val = 1;
            if (Size.val > 50) Size.val = 50;
            if (Mode.val < 0) Mode.val = 0;
            if (Mode.val > 3) Mode.val = 3;
            if (ShootId.val < 0) ShootId.val = 0;
            if (ShootId.val >= ProjectileID.Count) ShootId.val = ProjectileID.Count - 1;

            //
            skil5_time.setCD(52 - Size.val);

            skil5_time.next();
            if (skil5_time.Ok == false) return;

            skil5_time.resume();
            //

            Vector2 position = player.Center;
            Vector2 velocity = Main.MouseWorld - player.Center;//从玩家到鼠标的方向
            velocity.Normalize();

            Vector2 off = velocity.RotatedBy((MathHelper.TwoPi / 360) * 90);//从玩家到鼠标的方向的90度方向

            position += off * ((Size.val / 2) * 16f);//偏移长度为射弹生成大小的一半

            position -= off * (getRand(0, Size.val) * 16);

            if (Mode.val == 3) skil5_projIds4[0] = ShootId.val;

            int[] projs =
                Mode.val == 0 ? skil5_projIds1 :
                Mode.val == 1 ? skil5_projIds2 :
                Mode.val == 2 ? skil5_projIds3 :
                skil5_projIds4;

            float ai0 = Ai0.val;
            float ai1 = Ai1.val;
            float ai2 = Ai2.val;
            if (Mode.val == 2) ai1 = getRandFloat();

            Projectile.NewProjectile(null, position, velocity * ShootSpeed.val,
                projs[getRand(0, projs.Length)], SkilListControl1.damage.val, 1,
                ai0: ai0, ai1: ai1, ai2: ai2);
        }
    }
}
