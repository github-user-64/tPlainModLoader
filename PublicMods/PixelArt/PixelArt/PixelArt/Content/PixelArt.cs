using Microsoft.Xna.Framework;
using PixelArt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace PixelArt.Content
{
    public partial class PixelArt
    {
        public struct PixelInfo
        {
            public PixelInfo(Color color, int x, int y)
            {
                this.color = color;
                this.x = x;
                this.y = y;
            }

            public Color color;
            public int x;
            public int y;
        }

        public static bool Loaded { get; protected set; } = false;
        //
        public static bool PixelInfoLoading { get; protected set; } = false;//像素信息加载中
        public static bool PixelInfoLoaded { get; protected set; } = false;//像素信息加载完成
        public static bool SpawIng { get; protected set; } = false;//像素画生成中
        public static bool SpawPosSelecting = false;//生成位置选择中

        public static GetSetReset<string> LoadPath = new GetSetReset<string>("img.png", "img.png");//加载路径
        public static GetSetReset<int> SpawSpeed = new GetSetReset<int>(1, 1);//每次更新的生成次数
        public static GetSetReset<bool> SpawDistance = new GetSetReset<bool>(false, false);//玩家靠近生成
        public static GetSetReset<int> SpawDistance_val = new GetSetReset<int>(20, 20);
        public static GetSetReset<bool> SetSelectedItem = new GetSetReset<bool>(false, false);//设置玩家手中物品
        public static GetSetReset<int> SpawPosSelectV = new GetSetReset<int>(0, 0, v => v < 0 ? 0 : (v > 3 ? 3 : v));//生成位置选择方向
        //
        private static Point16 spawPos = Point16.Zero;//生成位置
        //
        private static List<PixelInfo> pixelInfo = null;
        private static List<PixelInfo> pixelInfo_copy = null;
        private static int pixelWidth = 0;
        private static int pixelHeight = 0;
        private static List<Item> wallItemIds = null;
        private static CancellationTokenSource pixelInfoLoad_cts = null;


        private static void initialize()
        {
            Loaded = false;

            EndSpaw();

            ClearPixelInfo();

            SpawPosSelecting = false;

            LoadWallItemId();

            Loaded = true;
        }

        public static void Update(Player player)
        {
            if (Loaded == false)
            {
                initialize();
                if (Loaded == false) return;
            }

            if (SpawPosSelecting)
            {
                Update_spawPosSelect(player);
                return;
            }

            if (SpawIng)
            {
                int count = SpawSpeed.val;

                if (count < 0)
                {
                    if (Main.GameUpdateCount % -count == 0) count = 1;
                }

                for (int i = 0; i < count && SpawIng; ++i) Update_spaw(player);
            }
        }

        private static void Update_spawPosSelect(Player player)
        {
            if (Main.mouseRight == true)
            {
                SpawPosSelecting = false;

                CombatText.NewText(player.getRect(), Color.Red, "取消选择", true, false);
                return;
            }

            Vector2 mouse = Main.MouseWorld;
            Vector2 v = mouse;
            if (SpawPosSelectV.val == 1) v.X -= pixelWidth * 16;
            else
            if (SpawPosSelectV.val == 2) v.Y -= pixelHeight * 16;
            else
            if (SpawPosSelectV.val == 3) { v.X -= pixelWidth * 16; v.Y -= pixelHeight * 16; }

            spawPos = Terraria.Utils.ToTileCoordinates16(v);

            if (Main.mouseLeft == true && Main.mouseLeftRelease == false && player.mouseInterface == false)
            {
                SpawPosSelecting = false;

                CombatText.NewText(new Rectangle((int)mouse.X, (int)mouse.Y, 1, 1), Color.Green, $"在{spawPos.X}, {spawPos.Y}生成", true, false);
            }
        }

        private static void Update_spaw(Player player)
        {
            if (pixelInfo_copy.Count < 1)
            {
                SpawIng = false;
                Main.NewText("生成完成");
                return;
            }

            PixelInfo? pi = null;
            if (SpawDistance.val)
            {
                float min = SpawDistance_val.val;

                for (int i = 0; i < pixelInfo_copy.Count; ++i)
                {
                    PixelInfo p = pixelInfo_copy[i];
                    float d = Vector2.Distance(new Vector2(spawPos.X + p.x, spawPos.Y + p.y), player.Center / 16);
                    if (d < min)
                    {
                        min = d;
                        pi = p;
                    }
                }
            }
            else
            {
                pi = pixelInfo_copy.First();
            }
            if (pi == null) return;

            int x = spawPos.X + pi.Value.x;
            int y = spawPos.Y + pi.Value.y;

            if (WorldGen.InWorld(x, y) == false)
            {
                pixelInfo_copy.Remove(pi.Value);
                return;
            }

            if (Main.tile[x, y]?.wall > 0)
            {
                pixelInfo_copy.Remove(pi.Value);
                Update_spaw(player);
                return;
            }

            Item item = LookupColorSimilarWallItem(pi.Value.color);
            if (item == null)
            {
                pixelInfo_copy.Remove(pi.Value);
                return;
            }

            if (SetSelectedItem.val)
            {
                if (player.inventory[player.selectedItem].type != item.type)
                {
                    player.inventory[player.selectedItem].SetDefaults(item.type);

                    if (Main.netMode == 1) return;//停一下等服务端同步数据
                }
            }

            pixelInfo_copy.Remove(pi.Value);

            WorldGen.PlaceWall(x, y, item.createWall);

            if (Main.netMode == 1) Utils.updateData_placeWall(x, y);
        }

        private static void LoadWallItemId()
        {
            wallItemIds = new List<Item>();

            Item item = new Item();

            for (int i = 0; i < ItemID.Count; ++i)
            {
                if (ItemID.Sets.Deprecated[i]) continue;//已弃用

                item.SetDefaults(i);
                if (item.type < 1 || item.type >= ItemID.Count) continue;

                if (item.createWall < 1) continue;

                wallItemIds.Add(item);
                item = new Item();
            }
        }

        #region 加载
        public static void LoadPixelInfo()
        {
            _ = LoadPixelInfoAsync().ContinueWith(t =>
            {
                if (t.IsCanceled) return;
                if (t.Result == null) return;
                Main.NewText($"{t.Result}");
            });
        }

        private static async Task<string> LoadPixelInfoAsync()
        {
            PixelInfoLoading = true;

            ClearPixelInfo();

            pixelInfoLoad_cts = new CancellationTokenSource();

            return await Task<string>.Factory.StartNew((token) =>
            {
                try
                {
                    List<PixelInfo> v = LoadImgToPixelInfo(LoadPath.val, ref pixelWidth, ref pixelHeight);

                    if (Loaded == false) return null;

                    ((CancellationToken)token).ThrowIfCancellationRequested();

                    pixelInfo = v;
                    PixelInfoLoading = false;
                    PixelInfoLoaded = true;

                    return "加载完成";
                }
                catch (Exception ex)
                {
                    PixelInfoLoading = false;

                    if (Loaded == false) return null;

                    ((CancellationToken)token).ThrowIfCancellationRequested();

                    return $"加载失败, {ex.Message}";
                }
            }, pixelInfoLoad_cts.Token);
        }

        public static void CancelLoadPixelInfo()
        {
            pixelInfoLoad_cts?.Cancel();
            pixelInfoLoad_cts = null;

            PixelInfoLoading = false;
        }
        #endregion

        #region 生成
        public static void StartSpaw()
        {
            for (int i = 0; i < pixelInfo.Count; ++i)
            {
                PixelInfo pi = pixelInfo[i];
                if (WorldGen.InWorld(spawPos.X + pi.x, spawPos.Y + pi.y)) continue;
                Main.NewText("超出世界部分将跳过");
                break;
            }

            SpawIng = true;
            pixelInfo_copy = new List<PixelInfo>();
            for (int i = 0; i < pixelInfo.Count; ++i) pixelInfo_copy.Add(pixelInfo[i]);
        }

        public static void EndSpaw()
        {
            SpawIng = false;
        }
        #endregion

        private static void ClearPixelInfo()
        {
            PixelInfoLoaded = false;
            pixelInfo?.Clear();
            pixelInfo = null;
        }

        public static void Draw()
        {
            if (SpawPosSelecting)
            {
                Vector2 positionWord = new Vector2(spawPos.X, spawPos.Y) * 16;
                Vector2 size = new Vector2(pixelWidth, pixelHeight) * 16;

                Color borderColor = new Color(40, 250, 80);

                DrawUtils.Draw_rectangle(positionWord, positionWord + size, borderColor, borderColor * 0.35f);
            }
        }
    }
}
