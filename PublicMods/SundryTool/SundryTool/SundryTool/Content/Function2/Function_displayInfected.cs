using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using System.Threading.Tasks;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    internal class Function_displayInfected : ModPlayer
    {
        public class displayInfected_Unload : Mod
        {
            public override void Unload()
            {
                displayInfected_runing.val = false;
            }
        }

        public static GetSetReset<bool> displayInfected_runing = new GetSetReset<bool>(false, false);
        private static bool displayInfected_taskRuning = false;

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("displayInfected", displayInfected_runing),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(displayInfected_runing, "关闭以取消", text: "显示感染方块"),
            };

            return uis;
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            a1_showInfected(This);
        }

        private static int showInfected_x = 0;
        private static int showInfected_y = 0;
        public static void a1_showInfected(Player player)
        {
            if (displayInfected_runing.val == false) return;
            if (displayInfected_taskRuning) return;

            _ = Task.Run(async () =>
            {
                if (displayInfected_taskRuning) return;
                displayInfected_taskRuning = true;

                Main.NewText("开始查找");

                showInfected_x = 0;
                showInfected_y = 0;

                try
                {
                    while (true)
                    {
                        if (displayInfected_runing.val == false)
                        {
                            displayInfected_runing.val = false;
                            displayInfected_taskRuning = false;
                            Main.NewText("取消查找");
                            return;
                        }

                        if (showInfected_x < Main.maxTilesX == false)
                        {
                            displayInfected_runing.val = false;
                            displayInfected_taskRuning = false;
                            Main.NewText("完成");
                            return;
                        }

                        if (Function_mapRevealer.playState.Update_noPlay)
                        {
                            displayInfected_runing.val = false;
                            displayInfected_taskRuning = false;
                            return;
                        }

                        Tile tile = Main.tile[showInfected_x, showInfected_y];

                        if (tile == null)
                        {
                            displayInfected_runing.val = false;
                            displayInfected_taskRuning = false;
                            Main.NewText("结束, 部分方块为null, 使用点亮全图功能可以加载全部方块");
                            return;
                        }

                        Color _c = Color.White;
                        if (Main.IsTileBiomeSightable(tile.type, (short)showInfected_x, (short)showInfected_y, ref _c))
                        {
                            Main.NewText("发现感染, 在地图上标记");
                            Main.Pings.Add(new Vector2(showInfected_x, showInfected_y));
                            await Task.Delay(1000);
                            continue;
                        }
                        else
                        {
                            if (Main.GameUpdateCount % 120 == 0) Main.Pings.Add(new Vector2(showInfected_x, showInfected_y));
                        }

                        ++showInfected_y;
                        if (showInfected_y < Main.maxTilesY == false)
                        {
                            showInfected_y = 0;
                            ++showInfected_x;
                        }
                    }
                }
                catch
                {
                    displayInfected_runing.val = false;
                    displayInfected_taskRuning = false;
                    Main.NewText("查找失败");
                }
            });
        }
    }
}
