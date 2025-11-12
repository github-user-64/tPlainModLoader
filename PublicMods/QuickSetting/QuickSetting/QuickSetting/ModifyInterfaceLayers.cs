using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace QuickSetting
{
    public class ModifyInterfaceLayers : ModMain
    {
        public static UIState ui_state { get; protected set; } = null;
        private static UserInterface ui = null;

        static ModifyInterfaceLayers()
        {
            ui = new UserInterface();
            ui_state = new UIState();
            ui.SetState(ui_state);
        }

        //public override void UpdateUIStatesPrefix(GameTime gameTime)
        //{
        //    ui.Update(gameTime);更新放在这会导致文本输入控件出问题
        //}

        public override void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers)
        {
            int index = gameInterfaceLayers.FindIndex(i => i.Name == "Vanilla: Inventory");
            if (index != -1)
            {
                gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer(
                "StaticTile.QuickSetting: Inventory Prefix",
                () =>
                {
                    ui.Update(Main.gameTimeCache);
                    ui.Draw(Main.spriteBatch, Main.gameTimeCache);
                    return true;
                },
                InterfaceScaleType.UI));
            }
        }
    }
}
