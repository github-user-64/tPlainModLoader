using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.UI;
using WandsTool.Content;

namespace WandsTool
{
    /// <summary>
    /// 哥们别看了, 这里都是直接移植的陈年老屎
    /// 原本想优化下的但看了下还不如直接重写
    /// </summary>
    public class feces : PatchMain
    {
        private static wandsPanel ui = null;

        static feces()
        {
            ui = new wandsPanel();
        }

        public override void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers)
        {
            int index = gameInterfaceLayers.FindIndex(i => i.Name == "Vanilla: Laser Ruler");
            if (index != -1)
            {
                ++index;
                gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer(
                    "StaticTile.WandsTool: Laser Ruler Postfix Game",
                    () =>
                    {
                        if (gameMain.Wand_isEnable)
                        {
                            Wands.Draw();
                        }
                        return true;
                    },
                    InterfaceScaleType.Game));

                gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer(
                    "StaticTile.WandsTool: Laser Ruler Postfix UI",
                    () =>
                    {
                        if (gameMain.UI_WandsPanel1_isOpen && gameMain.Wand_isEnable)
                        {
                            ui.Update(Main.gameTimeCache);
                            ui.update(Main.gameTimeCache);
                            ui.Draw(Main.spriteBatch, Main.gameTimeCache);
                        }

                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public override void DoUpdateInWorldPostfix(Stopwatch sw)
        {
            if (gameMain.Wand_isEnable)
            {
                if (Main.mouseRight && Main.mouseRightRelease)
                {
                    gameMain.UI_WandsPanel1_isOpen = !gameMain.UI_WandsPanel1_isOpen;

                    if (gameMain.UI_WandsPanel1_isOpen) ui.isReset = true;
                }

                Wands.Update();

                WandAction.Update();
                
            }
            else
            {
                gameMain.UI_WandsPanel1_isOpen = false;

                Wands.Reset();
            }
        }
    }
}
