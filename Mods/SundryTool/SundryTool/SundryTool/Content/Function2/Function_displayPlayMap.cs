using CommandHelp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    internal class Function_displayPlayMap : PatchMain
    {
        public static GetSetReset<bool> displayPlayMap = new GetSetReset<bool>(true, true);
        public static GetSetReset<int> displayPlayMap_mapStyle1_extraSize = new GetSetReset<int>(50, 50);

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
                UIBuild.get1(displayPlayMap, displayPlayMap_mapStyle1_extraSize, UITextBox.StringToInt, "小地图额外大小<int>", "Images/Buff_12", "地图显示全部玩家"),
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

                DrawPlayerHead(Main.player[i]);
            }
        }

        private static void DrawPlayerHead(Player player)
        {
            //没绘制的玩家
            bool v = (Main.LocalPlayer.hostile || player.hostile) && (Main.LocalPlayer.team != player.team || player.team == 0);
            v |= player.invis;

            if (Main.mapFullscreen)
            {
                float scale = Main.UIScale;

                Vector2 centerPos = new Vector2(Main.screenWidth, Main.screenHeight) / 2;//原点
                Vector2 pos = centerPos - Main.mapFullscreenPos * Main.mapFullscreenScale;
                pos += player.Center / 16 * Main.mapFullscreenScale;
                pos.X -= 6;
                pos.Y -= 2;
                pos.Y -= 2f - Main.mapFullscreenScale / 5f * 2f;

                if (v) DrawHead(player, pos, scale);

                MouseHoveringText(pos, 1f, $"{player.name}:{player.whoAmI}");

                return;
            }

            if (Main.mapStyle == 1)
            {
                float scale = (Main.mapMinimapScale * 0.25f * 2f + 1f) / 3f;
                if (scale > 1f) scale = 1f;

                //小地图中心在世界的位置, 世界原点
                Vector2 wordCenter = Main.screenPosition;
                wordCenter.X += PlayerInput.RealScreenWidth / 2;
                wordCenter.Y += PlayerInput.RealScreenHeight / 2;

                //玩家与世界原点的偏移
                Vector2 playOff = player.Center - wordCenter;

                //绘制位置
                Vector2 pos = new Vector2(Main.miniMapX + Main.miniMapWidth / 2, Main.miniMapY + Main.miniMapHeight / 2);
                pos += playOff / 16 * Main.mapMinimapScale;
                pos.X -= 6f;
                pos.Y -= 6f;
                pos.Y -= 2f - Main.mapMinimapScale / 5f * 2f;

                if (InMiniMapRange(pos, displayPlayMap_mapStyle1_extraSize.val) != true) return;

                Matrix transformMatrix2 = Main.UIScaleMatrix;
                Matrix matrix = Matrix.CreateScale(Main.MapScale);
                transformMatrix2 *= matrix;

                if (v || InMiniMapRange(pos, 0) != true)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix2);

                    DrawHead(player, pos, scale);
                }

                MouseHoveringText(pos * transformMatrix2.M11, Main.mapMinimapScale * 0.4f, $"{player.name}:{player.whoAmI}");

                return;
            }

            if (Main.mapStyle == 2)
            {
                float scale = (Main.mapOverlayScale * 0.2f * 2f + 1f) / 3f;
                if (scale > 1f) scale = 1f;
                scale *= Main.UIScale;

                //地图中心在世界的位置, 世界原点
                Vector2 wordCenter = Main.screenPosition;
                wordCenter.X += PlayerInput.RealScreenWidth / 2;
                wordCenter.Y += PlayerInput.RealScreenHeight / 2;

                //玩家与世界原点的偏移
                Vector2 playOff = player.Center - wordCenter;

                Vector2 pos = new Vector2(Main.screenWidth, Main.screenHeight) / 2;
                pos += playOff / 16 * Main.mapOverlayScale;
                pos.X -= 6;
                pos.Y -= 2;
                pos.Y -= 2f - Main.mapOverlayScale / 5f * 2f;

                if (v) DrawHead(player, pos, scale);

                MouseHoveringText(pos, scale, $"{player.name} : {player.whoAmI}");
            }
        }

        private static bool InMiniMapRange(Vector2 pos, int extraSize)
        {
            return pos.X > Main.miniMapX + 6 - extraSize &&
                pos.X < Main.miniMapX + Main.miniMapWidth - 16 + extraSize &&
                pos.Y > Main.miniMapY + 6 - extraSize &&
                pos.Y < Main.miniMapY + Main.miniMapHeight - 14 + extraSize;
        }

        private static void MouseHoveringText(Vector2 pos, float scale, string text)
        {
            float sizeR = 14 * scale;

            float x1 = pos.X - sizeR;
            float y1 = pos.Y - sizeR;
            float x2 = pos.X + sizeR;
            float y2 = pos.Y + sizeR;

            if (Main.mouseX >= x1 && Main.mouseX <= x2 && Main.mouseY >= y1 && Main.mouseY <= y2) Main.instance.MouseText(text);
        }

        private static void DrawHead(Player player, Vector2 pos, float scale)
        {
            if (player.invis)
            {
                Texture2D texture = TextureAssets.Buff[BuffID.Invisibility].Value;

                Main.spriteBatch.Draw(texture, pos + new Vector2(3f), null, Color.White, 0f, texture.Size() / 2f, MathHelper.Clamp(scale - 0.1f, 0.1f, 1f),
                    SpriteEffects.None, 0f);
            }

            Main.MapPlayerRenderer.DrawPlayerHead(Main.Camera, player, pos, 255, scale, Main.teamColor[player.team]);
        }
    }
}
