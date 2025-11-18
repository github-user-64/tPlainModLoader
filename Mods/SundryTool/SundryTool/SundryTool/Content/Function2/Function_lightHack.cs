using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;
using Terraria.Utilities;

namespace SundryTool.Content.Function2
{
    internal class Function_lightHack : PatchTileLightScanner
    {
        public static GetSetReset<bool> lightHack = new GetSetReset<bool>();

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("lightHack", lightHack),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(lightHack, text: "透视照明"),
            };

            return uis;
        }

        public override void ApplyTileLightPrefix(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
        {
            if (lightHack.val == false) return;

            lightColor = Vector3.One;
        }
    }
}
