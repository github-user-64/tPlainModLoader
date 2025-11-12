using CommandHelp;
using Microsoft.Xna.Framework;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace OptimizeAndTool.Content
{
    internal class DisplayProjectileInfo
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(false, false);
        public static GetSetReset<bool> ptype = new GetSetReset<bool>(true, true);
        public static GetSetReset<bool> pwhoAmI = new GetSetReset<bool>(true, true);
        public static GetSetReset<bool> pvelocity = new GetSetReset<bool>(true, true);
        public static GetSetReset<float> pvelocity_val = new GetSetReset<float>(5, 5);
        public static GetSetReset<bool> pai = new GetSetReset<bool>(true, true);
        public static GetSetReset<bool> plocalAI = new GetSetReset<bool>(false, false);
        private static Color[] info_Colors = { Color.Green, Color.BlueViolet, Color.Gold, Color.Pink };

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("displayProjectileInfo", Enable,
                CommandBuild.get2("type", ptype),
                CommandBuild.get2("whoAmI", pwhoAmI),
                CommandBuild.get1("velocity", pvelocity, pvelocity_val, new CommandFloat()),
                CommandBuild.get2("ai", pai),
                CommandBuild.get2("localAI", plocalAI)),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(Enable, text: "显示射弹信息"),
                UIBuild.get2(ptype, text: "射弹type"),
                UIBuild.get2(pwhoAmI, text: "射弹whoAmI"),
                UIBuild.get1(pvelocity, pvelocity_val, float.Parse, "速度线的长度倍率<flost>", text: "射弹速度"),
                UIBuild.get2(pai, text: "射弹ai"),
                UIBuild.get2(plocalAI, text: "射弹localAI"),
            };

            return uis;
        }

        public static void Draw()
        {
            if (Enable.val == false) return;

            Player player = Main.LocalPlayer;

            for (int i = 0; i < Main.projectile?.Length; ++i)
            {
                Projectile p = Main.projectile[i];

                if (p == null) continue;
                if (p.active == false) continue;
                if (p.Center.Distance(player.Center) > Main.screenWidth / 2) continue;

                DrawInfo(p, info_Colors[i % info_Colors.Length]);
            }
        }

        private static void DrawInfo(Projectile p, Color color)
        {
            Rectangle rectangle = p.getRect();

            Vector2 p1 = new Vector2(rectangle.X, rectangle.Y);
            Vector2 p2 = new Vector2(rectangle.X + rectangle.Width, rectangle.Y);
            Vector2 p3 = new Vector2(rectangle.X, rectangle.Y + rectangle.Height);
            Vector2 p4 = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);

            Terraria.Utils.DrawLine(Main.spriteBatch, p1, p2, color, color, 2f);
            Terraria.Utils.DrawLine(Main.spriteBatch, p3, p4, color, color, 2f);
            Terraria.Utils.DrawLine(Main.spriteBatch, p1, p3, color, color, 2f);
            Terraria.Utils.DrawLine(Main.spriteBatch, p2, p4, color, color, 2f);

            if (pvelocity.val)
                Terraria.Utils.DrawLine(Main.spriteBatch, p.Center, p.Center + p.velocity * pvelocity_val.val, Color.Yellow, Color.Red, 2f);

            Vector2 textP = new Vector2(rectangle.X - Main.screenPosition.X, rectangle.Y + rectangle.Height - Main.screenPosition.Y);

            string text = null;

            if (ptype.val)
                text = stringHelp(text, $"id: {p.type}");
            if (pwhoAmI.val)
                text = stringHelp(text, $"whoAmI: {p.whoAmI}");
            if (pvelocity.val)
                text = stringHelp(text, $"v: {p.velocity}");
            if (pai.val)
                text = stringHelp(text, "ai: ", p.ai);
            if (plocalAI.val)
                text = stringHelp(text, "localAI: ", p.ai);

            if (text != null)
                Terraria.Utils.DrawBorderString(Main.spriteBatch, text, textP, color, anchorx: 0f, anchory: 0f);
        }

        private static string stringHelp(string s, string add, float[] vs = null)
        {
            s = s == null ? add : $"{s}\n{add}";

            for (int i = 0; i < vs?.Length; ++i)
            {
                s += (i + 1 < vs.Length == false) ? $"{vs[i]}" : $"{vs[i]}, ";
            }

            return s;
        }
    }
}
