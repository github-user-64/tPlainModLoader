using CommandHelp;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.UI;

namespace OptimizeAndTool.Content
{
    internal partial class Function : PatchPlayer
    {
        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>();
            cos.AddRange(CleanRepeatChat.GetCO());
            cos.AddRange(CopyChat.GetCO());
            cos.AddRange(ServerList.ServerList.GetCO());
            cos.AddRange(ItemToolTipAdditional.GetCO());
            cos.AddRange(DisplayProjectileInfo.GetCO());

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>();
            uis.AddRange(CleanRepeatChat.GetUI());
            uis.AddRange(CopyChat.GetUI());
            uis.AddRange(ServerList.ServerList.GetUI());
            uis.AddRange(ItemToolTipAdditional.GetUI());
            uis.Add(new UIItemTitle(Main.Assets.Request<Texture2D>("Images/Item_2799", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, "工具"));
            uis.AddRange(DisplayProjectileInfo.GetUI());

            return uis;
        }
    }
}
