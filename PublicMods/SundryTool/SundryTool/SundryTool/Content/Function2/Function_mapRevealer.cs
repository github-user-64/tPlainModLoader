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
    public class Function_mapRevealer : ModPlayer
    {
        public class mapRevealer_Unload : Mod
        {
            public override void Unload()
            {
                MapRevealer_runing.val = false;
            }
        }

        public class playState : ModMain
        {
            public static bool _Update_noPlay = true;
            public static bool Update_noPlay = true;
            public override void UpdatePrefix(GameTime gameTime)
            {
                _Update_noPlay = true;
            }

            public override void UpdatePostfix(GameTime gameTime)
            {
                Update_noPlay = _Update_noPlay;
            }
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("mapRevealer", MapRevealer_runing),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(MapRevealer_runing, "关闭以取消", text: "点亮全图"),
            };

            return uis;
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            playState._Update_noPlay = false;

            if (This != Main.LocalPlayer) return;

            a1_MapRevealer(This);
        }

        public static GetSetReset<bool> MapRevealer_runing = new GetSetReset<bool>(false, false);
        private static bool MapRevealer_taskRuning = false;

        private static void a1_MapRevealer(Player player)
        {
            if (MapRevealer_runing.val == false) return;
            if (MapRevealer_taskRuning) return;

            if (Main.netMode == 1)
            {
                MapRevealer_runing.val = true;
                MapRevealer_taskRuning = true;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        Main.NewText("加载方块并点亮中");

                        await a1_MapRevealer_Task(player);

                        Main.NewText("点亮完成");
                    }
                    catch
                    {
                        Main.NewText("点亮失败");
                    }
                    finally
                    {
                        MapRevealer_taskRuning = false;
                        MapRevealer_runing.val = false;
                    }
                });
            }
            else if (Main.netMode == 0)
            {
                MapRevealer_runing.val = true;
                MapRevealer_taskRuning = true;

                Main.NewText("点亮中");
                lightMap();
                Main.NewText("点亮完成");

                MapRevealer_runing.val = false;
                MapRevealer_taskRuning = false;
            }
        }
        private static async Task a1_MapRevealer_Task(Player player)
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y] != null) continue;

                    while (Main.tile[x, y] == null)
                    {
                        if (playState.Update_noPlay || MapRevealer_runing.val == false)
                        {
                            lightMap();
                            return;
                        }

                        Vector2 v = new Vector2(x, y) * 16;
                        v.X = MathHelper.Clamp(v.X, 41 * 16, (Main.maxTilesX - 42) * 16 - player.width);
                        v.Y = MathHelper.Clamp(v.Y, 41 * 16, (Main.maxTilesY - 42) * 16 - player.height);
                        player.position = v;

                        NetMessage.SendData(13, number: player.whoAmI);

                        await Task.Delay(5);
                    }
                }
            }

            lightMap();
        }
        private static void lightMap()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (playState.Update_noPlay)
                    {
                        Main.refreshMap = true;
                        return;
                    }

                    if (WorldGen.InWorld(i, j))
                        Main.Map.Update(i, j, 255);
                }
            }
            Main.refreshMap = true;
        }
    }
}
