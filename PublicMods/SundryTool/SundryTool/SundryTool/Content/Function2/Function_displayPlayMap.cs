using CommandHelp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    internal class Function_displayPlayMap : ModMain
    {
        public static GetSetReset<bool> displayPlayMap = new GetSetReset<bool>(true, true);
        public static GetSetReset<int> displayPlayMap_mapStyle1_extraSize = new GetSetReset<int>(20, 20);

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get1("displayPlayMap", displayPlayMap, displayPlayMap_mapStyle1_extraSize, new CommandInt()),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(displayPlayMap, displayPlayMap_mapStyle1_extraSize, int.Parse, "小地图额外大小<int>", null, "地图显示全部玩家"),
            };

            return uis;
        }

        public override void DrawMapPostfix(GameTime gameTime)
        {
            if (displayPlayMap.val == false) return;

            Player player = Main.LocalPlayer;

            if (!Main.mapEnabled || !Main.mapReady) return;

            for (int i = 0; i < Main.player.Length; ++i)
            {
                if (Main.player[i] == null) continue;
                if (Main.player[i].active == false) continue;
                if (Main.player[i].dead) continue;
                if (Main.player[i] == player) continue;

                //没绘制的玩家
                bool v = (Main.player[Main.myPlayer].hostile || Main.player[i].hostile)
                         && (Main.player[Main.myPlayer].team != Main.player[i].team || Main.player[i].team == 0);

                if (v == false) continue;

                DrawPlayerHead(Main.player[i]);
            }
        }

        private static void DrawPlayerHead(Player player)
        {
            string text = null;

            if (Main.mapFullscreen)
            {
                Vector2 centerPos = new Vector2(Main.screenWidth, Main.screenHeight) / 2;//原点
                Vector2 wrawPos = centerPos - Main.mapFullscreenPos * Main.mapFullscreenScale;
                wrawPos += player.Center / 16 * Main.mapFullscreenScale;
                wrawPos.X -= 6;
                wrawPos.Y -= 2;
                wrawPos.Y -= 2f - Main.mapFullscreenScale / 5f * 2f;

                Main.MapPlayerRenderer.DrawPlayerHead(Main.Camera, player, wrawPos, 255, Main.UIScale, Main.teamColor[player.team]);

                if (isMouseHovering(wrawPos, 1)) text = $"{player.name}:{player.whoAmI}";
            }
            else
            if (Main.mapStyle == 1)
            {
                float num62 = (Main.mapMinimapScale * 0.25f * 2f + 1f) / 3f;
                if (num62 > 1f)
                {
                    num62 = 1f;
                }

                //小地图中心在世界的位置, 世界原点
                Vector2 wordCenter = Main.screenPosition;
                wordCenter.X += PlayerInput.RealScreenWidth / 2;
                wordCenter.Y += PlayerInput.RealScreenHeight / 2;

                //玩家与世界原点的偏移
                Vector2 playOff = player.Center - wordCenter;

                //绘制位置
                Vector2 wrawPos = new Vector2(Main.miniMapX + Main.miniMapWidth / 2, Main.miniMapY + Main.miniMapHeight / 2);
                wrawPos += playOff / 16 * Main.mapMinimapScale;
                wrawPos.X -= 6f;
                wrawPos.Y -= 6f;
                wrawPos.Y -= 2f - Main.mapMinimapScale / 5f * 2f;

                int extraSize = displayPlayMap_mapStyle1_extraSize.val;
                if (wrawPos.X > Main.miniMapX + 6 - extraSize &&
                    wrawPos.X < Main.miniMapX + Main.miniMapWidth - 16 + extraSize &&
                    wrawPos.Y > Main.miniMapY + 6 - extraSize &&
                    wrawPos.Y < Main.miniMapY + Main.miniMapHeight - 14 + extraSize)
                {
                    Matrix transformMatrix2 = Main.UIScaleMatrix;
                    Matrix matrix = Matrix.CreateScale(Main.MapScale);
                    transformMatrix2 *= matrix;

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix2);

                    Main.MapPlayerRenderer.DrawPlayerHead(Main.Camera, player, wrawPos, 255, num62, Main.teamColor[player.team]);

                    if (isMouseHovering(wrawPos * transformMatrix2.M11, Main.mapMinimapScale * 0.4f)) text = $"{player.name}:{player.whoAmI}";
                }
            }
            else
            if (Main.mapStyle == 2)
            {
                float num53 = (Main.mapOverlayScale * 0.2f * 2f + 1f) / 3f;
                if (num53 > 1f)
                {
                    num53 = 1f;
                }
                num53 *= Main.UIScale;

                //地图中心在世界的位置, 世界原点
                Vector2 wordCenter = Main.screenPosition;
                wordCenter.X += PlayerInput.RealScreenWidth / 2;
                wordCenter.Y += PlayerInput.RealScreenHeight / 2;

                //玩家与世界原点的偏移
                Vector2 playOff = player.Center - wordCenter;

                Vector2 wrawPos = new Vector2(Main.screenWidth, Main.screenHeight) / 2;
                wrawPos += playOff / 16 * Main.mapOverlayScale;
                wrawPos.X -= 6;
                wrawPos.Y -= 2;
                wrawPos.Y -= 2f - Main.mapOverlayScale / 5f * 2f;

                Main.MapPlayerRenderer.DrawPlayerHead(Main.Camera, player, wrawPos, 255, num53, Main.teamColor[player.team]);

                if (isMouseHovering(wrawPos, num53)) text = $"{player.name} : {player.whoAmI}";
            }

            if (text != null) Main.instance.MouseText(text);
        }

        private static bool isMouseHovering(Vector2 pos, float scale)
        {
            float sizeR = 14 * scale;

            float x1 = pos.X - sizeR;
            float y1 = pos.Y - sizeR;
            float x2 = pos.X + sizeR;
            float y2 = pos.Y + sizeR;

            return Main.mouseX >= x1 && Main.mouseX <= x2 && Main.mouseY >= y1 && Main.mouseY <= y2;
        }
    }
}
