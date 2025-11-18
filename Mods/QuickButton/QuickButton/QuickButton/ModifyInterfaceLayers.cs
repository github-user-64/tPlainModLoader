using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace QuickButton
{
    public class ModifyInterfaceLayers : PatchMain
    {
        public static UIState ui_state { get; protected set; } = null;
        private static UserInterface ui = null;

        static ModifyInterfaceLayers()
        {
            ui = new UserInterface();
            ui_state = new UIState();
            ui.SetState(ui_state);
        }

        public override void UpdateUIStatesPostfix(GameTime gameTime)
        {
            ui.Update(gameTime);
        }

        public override void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers)
        {
            int index = gameInterfaceLayers.FindIndex(i => i.Name == "Vanilla: Inventory");
            if (index != -1)
            {
                gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer(
                "StaticTile.QuickButton: Inventory Prefix",
                () =>
                {
                    ui.Draw(Main.spriteBatch, Main.gameTimeCache);
                    return true;
                },
                InterfaceScaleType.UI));
            }
        }
    }
}
