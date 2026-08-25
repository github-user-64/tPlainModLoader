using AccessoryBox.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace AccessoryBox
{
    internal class ModifyInterfaceLayers : PatchMain
    {
        public static ModifyInterfaceLayers Instance { get; protected set; }
        private UIState state = null;
        private UserInterface ui = null;
        private BoxWindow window = null;
        private static IBoxConsole console = null;

        public override void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers)
        {
            Instance = this;

            int index = gameInterfaceLayers.FindIndex(i => i.Name == "Vanilla: Inventory");
            if (index == -1) return;

            state = new UIState();
            ui = new UserInterface();

            //在物品栏前插入
            gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer(
                "StaticTile.AccessoryBox: Inventory Prefix",
                () =>
                {
                    ui.Draw(Main.spriteBatch, Main.gameTimeCache);//绘制ui
                    return true;
                },
                InterfaceScaleType.UI));

            window = new BoxWindow(console, "饰品箱", 350, 200);
        }

        public override void UpdateUIStatesPostfix(GameTime gameTime)
        {
            if (ui == null) return;

            if (Main.gameMenu || Main.mapFullscreen)//在游戏外或打开大地图时
            {
                ui.SetState(null);//禁用ui
                return;
            }

            ui.SetState(state);//启用ui
            ui.Update(gameTime);//更新ui
        }

        public void SwitchWindow()
        {
            if (window.IsOpen) window.Close();//从state删除
            else window.Open(state);//添加到state
        }

        public static void SetConsole(IBoxConsole console)
        {
            ModifyInterfaceLayers.console = console;
        }
    }
}
