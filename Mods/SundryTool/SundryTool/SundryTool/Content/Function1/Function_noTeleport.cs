using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Patch;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_noTeleport : Mod
    {
        public static GetSetReset<bool> noTeleport = new GetSetReset<bool>();

        public override void AddPatch(IAddPatch addPatch)
        {
            addPatch.AddPrefix(typeof(Player).GetMethod("Teleport"), typeof(Function_noTeleport).GetMethod("Teleport"));
        }

        public static bool Teleport(Player __instance, Vector2 newPos, int Style = 0, int extraInfo = 0)
        {
            if (__instance != Main.LocalPlayer) return true;
            return noTeleport.val == false;
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("noTeleport", noTeleport),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(noTeleport, "重新加载模组后该功能会失控", "Images/Buff_88", "禁用传送"),
            };

            return uis;
        }
    }
}
