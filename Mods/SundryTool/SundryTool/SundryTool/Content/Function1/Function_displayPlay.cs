using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    internal class Function_displayPlay
    {
        public static GetSetReset<bool> displayPlay = new GetSetReset<bool>();

        public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (displayPlay.val == false) return;

            Player player = Main.LocalPlayer;

            for (int i = 0; i < Main.player?.Length; ++i)
            {
                Player p = Main.player[i];
                if (p == null) continue;
                if (p.active == false) continue;
                if (p == player) continue;

                Vector2 abBetween = p.Center - player.Center;
                Vector2 textP = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

                float textP_lenght = (Math.Min(Main.screenWidth, Main.screenHeight) / 2) - 64;
                textP_lenght = Math.Min(textP_lenght, abBetween.Length());
                if (textP_lenght < 5) textP_lenght = 10;

                textP += abBetween.SafeNormalize(Vector2.Zero) * textP_lenght;

                Terraria.Utils.DrawBorderString(Main.spriteBatch,
                    $"{p.name ?? "null"}:{(int)abBetween.Length() / 16}:{p.whoAmI}",
                    textP, p.hostile ? Color.Red : Color.White, anchorx: 0.5f, anchory: 0.5f);
            }
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("displayPlay", displayPlay),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(displayPlay, null, "Images/Buff_17", "显示玩家位置"),
            };

            return uis;
        }
    }
}
