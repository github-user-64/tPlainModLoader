using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria.UI;

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
                UIBuild.get2(lightHack, null, "Images/Buff_9", "透视照明"),
            };

            return uis;
        }

        public override void GetTileLightPostfix(int x, int y, ref Vector3 outputColor)
        {
            if (lightHack.val != true) return;

            outputColor += (Vector3.One - outputColor) / 100f * 100f;
        }
    }
}
