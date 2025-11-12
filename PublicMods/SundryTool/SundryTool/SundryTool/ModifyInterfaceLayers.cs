using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace SundryTool
{
    public class ModifyInterfaceLayers : ModMain
    {
        public override void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers)
        {
            int index = gameInterfaceLayers.FindIndex(i => i.Name == "Vanilla: Inventory");
            if (index != -1)
            {
                gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer(
                    "StaticTile.SundryTool: InventoryPrefix",
                    () =>
                    {
                        Content.Function1.Function_displayPlay.Draw(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }
    }
}
