using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace WandsTool.Content
{
    public partial class Wands
    {
        public enum Shapes
        {
            line,
            circular,
            rectangle,
        }


        private static Vector2 position1;
        private static Vector2 position2;
        private static bool selecting = false;
        private static List<Point> shapes = null;
        private static Shapes shapes_s = Shapes.line;


        public static void Reset()
        {
            selecting = false;
            shapes = null;
        }

        public static void Update()
        {
            Update_Select();
        }

        private static void Update_Select()
        {
            if (Main.mouseRight == true && selecting == true)
            {
                selecting = false;

                Terraria.Player player = Main.LocalPlayer;
                if (player == null) return;

                CombatText.NewText(player.getRect(), Color.Red,
                    $"取消{(gameMain.Wand_isPlace ? "放置" : "破坏")}", true, false);

                return;
            }

            if (Main.mouseLeft == true && Main.mouseLeftRelease == true && Main.mouseRight == false && selecting == false)
            {
                if (Main.LocalPlayer.mouseInterface == true) return;

                selecting = true;
                position1 = Main.MouseWorld;
            }

            if (selecting == false) return;

            position2 = Main.MouseWorld;

            if (Main.mouseLeft == false)
            {
                Update_Shapes();
                selecting = false;

                bool wire =
                    gameMain.Wand_ToolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Red) ||
                    gameMain.Wand_ToolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Green) ||
                    gameMain.Wand_ToolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Blue) ||
                    gameMain.Wand_ToolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Yellow) ||
                    gameMain.Wand_ToolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Actuator);

                bool wireLine = shapes_s == Shapes.rectangle;

                if (gameMain.Wand_Tile || gameMain.Wand_Wall)
                {
                    if (gameMain.Wand_isPlace)
                    {
                        WandAction.AddTile(shapes, gameMain.Wand_Tile, gameMain.Wand_Wall, gameMain.Wand_BlockType);
                    }
                    else
                    {
                        WandAction.DelTile(shapes, gameMain.Wand_Tile, gameMain.Wand_Wall);
                    }
                }

                if (wire)
                {
                    if (wireLine)
                    {
                        Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode toolMode = gameMain.Wand_ToolMode;

                        if (gameMain.Wand_isPlace == false) toolMode |= Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Cutter;

                        WandAction.AddWireLine(position1, position2, toolMode);
                    }
                    else
                    {
                        if (gameMain.Wand_isPlace)
                        {
                            WandAction.AddWire(shapes, gameMain.Wand_ToolMode);
                        }
                        else
                        {
                            WandAction.DelWire(shapes, gameMain.Wand_ToolMode);
                        }
                    }
                }
                
                shapes = null;
            }
        }

        private static void Update_Shapes()
        {
            shapes_s = gameMain.Wand_Shapes;

            switch (shapes_s)
            {
                case Shapes.line: shapes = WandUtils.GetShapes_line(position1, position2); break;
                case Shapes.circular: shapes = WandUtils.GetShapes_Circular(position1, position2); break;
                case Shapes.rectangle: shapes = WandUtils.GetShapes_Rectangle(position1, position2); break;
                default: break;
            }
        }

        public static void Draw()
        {
            if (selecting == false) return;

            if (Main.GameUpdateCount % 5 == 0 || shapes == null) Update_Shapes();

            if (shapes?.Count > 0 == false) return;

            //Color borderColor = new Color(135, 0, 180);//紫色
            //Color borderColor = new Color(250, 40, 80);//红色
            //Color borderColor = new Color(40, 250, 80);//绿色
            Color borderColor = gameMain.Wand_isPlace ? new Color(40, 250, 80) : new Color(250, 40, 80);
            Color backgroundColor = borderColor * 0.35f;

            //
            int w = (int)Math.Abs(Math.Floor(position1.X / 16) - Math.Floor(position2.X / 16)) + 1;
            int h = (int)Math.Abs(Math.Floor(position1.Y / 16) - Math.Floor(position2.Y / 16)) + 1;
            Terraria.Utils.DrawBorderString(Main.spriteBatch, $"{w} x {h}", new Vector2(Main.mouseX, Main.mouseY + 50), borderColor, anchorx: 0.5f, anchory: 0.5f);
            //

            switch (shapes_s)
            {
                case Shapes.line: WandUtils.Draw_line(shapes, position1, position2, borderColor, backgroundColor); break;
                case Shapes.circular: WandUtils.Draw_circular(shapes, position1, position2, borderColor, backgroundColor); break;
                case Shapes.rectangle: WandUtils.Draw_rectangle(shapes, position1, position2, borderColor, backgroundColor); break;
                default: break;
            }
        }
    }
}
