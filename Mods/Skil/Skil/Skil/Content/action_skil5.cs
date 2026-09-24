using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Content.UI;
using Skil.ModLinkage;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;
using static Skil.Content.Utils;

namespace Skil.Content
{
    public class skil5 : PatchPlayer
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
        public static GetSetReset<bool> OriginalDamage = new GetSetReset<bool>();
        public static GetSetReset<bool> StaffFeatures = new GetSetReset<bool>();//法杖样式
        public static GetSetReset<bool> StaffFeatures_Homing = new GetSetReset<bool>();//紫晶追踪
        public static GetSetReset<bool> StaffFeatures_FastThenSlow = new GetSetReset<bool>();//翡翠快变慢|贴地
        public static GetSetReset<bool> StaffFeatures_CanBounce = new GetSetReset<bool>();//琥珀弹跳
        public static GetSetReset<bool> StaffFeatures_AoeExplosion = new GetSetReset<bool>();//黄玉范围伤害
        public static GetSetReset<bool> StaffFeatures_SwirlTwins = new GetSetReset<bool>();//蓝玉蛇皮
        public static GetSetReset<bool> StaffFeatures_ArPenSpread = new GetSetReset<bool>();//红玉处理伤害
        public static GetSetReset<bool> StaffFeatures_BiggerHitbox = new GetSetReset<bool>();//钻石变大
        public static GetSetReset<bool> StaffFeatures_AllGems = new GetSetReset<bool>();//
        public static GetSetReset<bool> StaffFeatures_RepeatsGem = new GetSetReset<bool>();//红玉重复
        public static GetSetReset<bool> StaffFeatures_AllGems2 = new GetSetReset<bool>();//
        private static string shootName = null;

        static skil5()
        {
            ShootId.OnValUpdate += v =>
            {
                Projectile p = new Projectile();
                p.SetDefaults(ShootId.val);

                shootName = p.Name;
            };
        }

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
                .SkilCMDBuild("mode", Mode)
                .SkilCMDBuild("originalDamage", OriginalDamage)
                .SkilCMDBuild("StaffFeatures", StaffFeatures),
            };
        }

        public static List<UIElement> GetUI()
        {
            UIItemTextBoxBind<int> type = UIBuild.get6(ShootId, int.Parse, "<int>", "Images/Buff_322", "技能5射弹id");
            type.OnUpdate += _ => type.MouseText = $"{shootName}<int>";

            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Size, int.Parse, "范围<int>", "Images/Buff_322", "技能5"),
                UIBuild.get6(Mode, int.Parse, "0: 宝石法杖, 1:光束, 2:日暮, 3:自定义<int>", "Images/Buff_322", "技能5模式"),
                UIBuild.get6(ShootSpeed, float.Parse, "<float>", "Images/Buff_322", "技能5射弹速度"),
                type,
                UIBuild.get6(Ai0, float.Parse, "<float>", "Images/Buff_322", "技能5射弹ai0"),
                UIBuild.get6(Ai1, float.Parse, "<float>", "Images/Buff_322", "技能5射弹ai1"),
                UIBuild.get6(Ai2, float.Parse, "<float>", "Images/Buff_322", "技能5射弹ai2"),
                UIBuild.get2(OriginalDamage, "开启后召唤物射弹才有伤害", "Images/Buff_322", "技能5设置原伤害"),
                UIBuild.get2(StaffFeatures, null, "Images/Item_739", "法杖样式"),
                UIBuild.get2(StaffFeatures_Homing, null, "Images/Item_1282", "追踪"),
                UIBuild.get2(StaffFeatures_FastThenSlow, null, "Images/Item_1285", "快变慢|贴地"),
                UIBuild.get2(StaffFeatures_CanBounce, null, "Images/Item_4256", "弹跳"),
                UIBuild.get2(StaffFeatures_AoeExplosion, null, "Images/Item_1283", "范围伤害"),
                UIBuild.get2(StaffFeatures_SwirlTwins, null, "Images/Item_1284", "蛇皮"),
                UIBuild.get2(StaffFeatures_ArPenSpread , null, "Images/Item_1286", "伤害//无效"),
                UIBuild.get2(StaffFeatures_BiggerHitbox, null, "Images/Item_1287", "变大"),
                UIBuild.get2(StaffFeatures_AllGems, null, "Images/Item_1282", "AllGems"),
                UIBuild.get2(StaffFeatures_RepeatsGem, null, "Images/Item_1286", "重复//无效"),
                UIBuild.get2(StaffFeatures_AllGems2, null, "Images/Item_1282", "AllGems2"),
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
            if (ShootId.val >= ModExtenContent.ProjectileCount) ShootId.val = ModExtenContent.ProjectileCount - 1;

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
            if (Mode.val == 2) ai1 = getRandFloat();//彩色暮光长枪
            //法杖样式
            if (StaffFeatures.val) ai0 = GetGemStaffFeatures(
                StaffFeatures_Homing.val,
                StaffFeatures_FastThenSlow.val,
                StaffFeatures_CanBounce.val,
                StaffFeatures_AoeExplosion.val,
                StaffFeatures_SwirlTwins.val,
                StaffFeatures_ArPenSpread.val,
                StaffFeatures_BiggerHitbox.val,
                StaffFeatures_AllGems.val,
                StaffFeatures_RepeatsGem.val,
                StaffFeatures_AllGems2.val
                );

            Projectile.NewProjectile(null, position, velocity * ShootSpeed.val,
                projs[getRand(0, projs.Length)], SkilListControl1.damage.val, 1, player.whoAmI,
                ai0, ai1, ai2,
                p =>
                {
                    if (OriginalDamage.val) p.originalDamage = p.damage;
                });
        }

        private static float GetGemStaffFeatures(
            bool Homing = false,
            bool FastThenSlow = false,
            bool CanBounce = false,
            bool AoeExplosion = false,
            bool SwirlTwins = false,
            bool ArPenSpread = false,
            bool BiggerHitbox = false,
            bool AllGems = false,
            bool RepeatsGem = false,
            bool AllGems2 = false
            )
        {
            Projectile.GemStaffFeatures gsf = default;
            gsf.Homing = Homing;//归航
            gsf.FastThenSlow = FastThenSlow;//快然后慢
            gsf.CanBounce = CanBounce;//可以弹跳
            gsf.AoeExplosion = AoeExplosion;//Aoe爆炸
            gsf.SwirlTwins = SwirlTwins;//旋涡双胞胎
            gsf.ArPenSpread = ArPenSpread;//Ar笔扩散
            gsf.BiggerHitbox = BiggerHitbox;//更大的Hitbox
            gsf.AllGems = AllGems;//
            gsf.RepeatsGem = RepeatsGem;//重复宝石
            gsf.AllGems2 = AllGems2;//

            return gsf.Bits;
        }
    }
}
