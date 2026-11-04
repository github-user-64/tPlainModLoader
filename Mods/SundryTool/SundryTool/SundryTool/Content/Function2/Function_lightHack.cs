using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using System.Reflection;
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

        //public override void ApplyTileLightPrefix(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
        //{
        //    if (lightHack.val == false) return;

        //    lightColor = Vector3.One;
        //}
    }
    [HarmonyPatch(typeof(TileLightScanner))]
    public static class Patch_GetTileLight
    {
        private static MethodInfo TargetMethod()
        {
            return typeof(TileLightScanner).GetMethod(
                "GetTileLight",
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new[] { typeof(int), typeof(int), typeof(Vector3).MakeByRefType() },
                null);
        }
        [HarmonyPostfix]
        private static void GetTileLightPostfix(ref Vector3 outputColor)
        {
            if (!Function_lightHack.lightHack.val) return;
            float b = 1f;        //亮度,0.0 - 1.0f可做成UI调节
            outputColor = new Vector3(outputColor.X + b, outputColor.Y + b, outputColor.Z + b);
        }
    }
}
