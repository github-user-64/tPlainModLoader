using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;

namespace Skil.Content.skil15
{
    //圣骑士锤雷
    internal static class skil15_1
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> Mode = new GetSetReset<int>(0, 0, GetSetReset.GetIntFunc(0, 1));//应用于
        public static GetSetReset<int> Color = new GetSetReset<int>(-1, -1);//闪电颜色

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>()
            {
                UIBuild.get1(Enable, Mode, UITextBox.StringToInt, "0: 自己, 1: 全部<int>,", "Images/Item_1513", "触发闪电"),
                UIBuild.get6(Color, UITextBox.StringToInt, "颜色, 小于0随机<int>", "Images/Item_1513", "闪电颜色"),
            };

            return uis;
        }

        public static void Update(Projectile proj, Player localPlayer)
        {
            if (Enable.val == false) return;

            if (Mode.val == 1 || proj.owner == Main.myPlayer) ParticleSpawn(proj);
        }

        private static void ParticleSpawn(Projectile proj)
        {
            ParticleOrchestraType type = ParticleOrchestraType.StormLightning;
            int style = Utils.getRand(0, 1145);
            int color = GetLightningColor();

            ParticleOrchestrator.BroadcastOrRequestParticleSpawn(type, new ParticleOrchestraSettings
            {
                PositionInWorld = proj.Center,
                UniqueInfoPiece = color,
                MovementVector = new Vector2(style, 0f),
            });
        }

        public static int GetLightningColor()
        {
            if (Color.val < 0)
            {
                return (int)new Color(
                    Utils.getRand(byte.MinValue, byte.MaxValue),
                    Utils.getRand(byte.MinValue, byte.MaxValue),
                    Utils.getRand(byte.MinValue, byte.MaxValue)).PackedValue;
            }

            return Color.val;
        }
    }
}
