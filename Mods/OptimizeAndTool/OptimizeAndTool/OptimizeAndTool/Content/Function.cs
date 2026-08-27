using CommandHelp;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
            cos.AddRange(PatchGameViewMatrixZoomLimit.GetCO());
            cos.AddRange(TimeSet.GetCO());

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>();
            uis.Add(new UIItemTitle(Main.Assets.Request<Texture2D>("Images/Item_2799", AssetRequestMode.ImmediateLoad).Value, "工具"));
            uis.AddRange(DisplayProjectileInfo.GetUI());
            uis.Add(new UIItemTitle(Main.Assets.Request<Texture2D>("Images/Item_4766", AssetRequestMode.ImmediateLoad).Value, "缩放限制"));
            uis.AddRange(PatchGameViewMatrixZoomLimit.GetUI());
            uis.Add(new UIItemTitle(Main.Assets.Request<Texture2D>("Images/Item_15", AssetRequestMode.ImmediateLoad).Value, "时间"));
            uis.AddRange(TimeSet.GetUI());
            uis.Add(new UIItemTitle(Main.Assets.Request<Texture2D>("Images/Item_7", AssetRequestMode.ImmediateLoad).Value, "其它"));
            uis.AddRange(CleanRepeatChat.GetUI());
            uis.AddRange(CopyChat.GetUI());
            uis.AddRange(ServerList.ServerList.GetUI());
            uis.AddRange(ItemToolTipAdditional.GetUI());

            return uis;
        }
    }
}
