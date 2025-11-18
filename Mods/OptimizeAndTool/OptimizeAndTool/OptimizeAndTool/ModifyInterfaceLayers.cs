using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace OptimizeAndTool
{
    public class ModifyInterfaceLayers : PatchMain
    {
        public static UIState ui_menu_state { get; private set; } = null;
        private static UserInterface ui_menu = null;

        static ModifyInterfaceLayers()
        {
            ui_menu = new UserInterface();
            ui_menu_state = new UIState();
            ui_menu.SetState(ui_menu_state);
        }

        public override void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers)
        {
            int index = gameInterfaceLayers.FindIndex(i => i.Name == "Vanilla: Laser Ruler");
            if (index != -1)
            {
                ++index;
                gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer(
                    "StaticTile.SundryTool: Laser Ruler Postfix",
                    () =>
                    {
                        Content.DisplayProjectileInfo.Draw();
                        return true;
                    },
                    InterfaceScaleType.Game));
            }
        }

        public override void UpdateUIStatesPostfix(GameTime gameTime)
        {
            ui_menu.Update(gameTime);
        }

        public override void DrawMenuPrefix(GameTime gameTime)
        {
            ui_menu.Draw(Main.spriteBatch, gameTime);
        }
    }
}
