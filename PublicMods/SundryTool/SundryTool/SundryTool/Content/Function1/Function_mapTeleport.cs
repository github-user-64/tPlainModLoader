using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    internal partial class Function_mapTeleport : ModPlayer
    {
        public static GetSetReset<bool> mapTeleport = new GetSetReset<bool>();

        public override void UpdatePrefix(Player This, int ThisI)
        {
            if (This != Main.LocalPlayer) return;
            if (mapTeleport.val == false) return;

            if (Main.mapFullscreen == false || Main.mouseRight == false) return;

            Vector2 centerPos = new Vector2(Main.screenWidth, Main.screenHeight) / 2;//原点
            ///不使用<see cref="Main.mouseX"/>, 该属性在缩放游戏画面时, 即使鼠标不动该属性也会变化
            Vector2 mouseCenterPos = new Vector2(PlayerInput.MouseX, PlayerInput.MouseY) - centerPos;//鼠标在原点的位置

            //Main.mapFullscreenPos
            //屏幕的中心点在地图的哪个位置, 这个点就在哪个位置

            Vector2 mouseWordPos = Main.mapFullscreenPos;//鼠标在世界中的位置
            mouseWordPos += mouseCenterPos / Main.mapFullscreenScale;
            mouseWordPos *= 16;

            int mapWidth = Main.maxTilesX * 16;
            int mapHeight = Main.maxTilesY * 16;
            float plyWidthHalf = This.width / 2;
            float plyHeightHalf = This.height / 2;

            if (mouseWordPos.X < 0) mouseWordPos.X = 0;
            else if (mouseWordPos.X + plyWidthHalf > mapWidth) mouseWordPos.X = mapWidth - plyWidthHalf;
            if (mouseWordPos.Y < 0) mouseWordPos.Y = 0;
            else if (mouseWordPos.Y + plyHeightHalf > mapHeight) mouseWordPos.Y = mapHeight - plyHeightHalf;

            This.Center = mouseWordPos;
            This.velocity = Vector2.Zero;
            This.fallStart = (int)(This.position.Y / 16f);//重置下落高度
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("mapTeleport", mapTeleport),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(mapTeleport, null, "Images/Buff_212", "地图传送"),
            };

            return uis;
        }
    }
}
