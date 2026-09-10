using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelArt.Content.ColorToWall;
using PixelArt.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Utils1 = PixelArt.Utils;

namespace PixelArt.Content
{
    public partial class PixelArt
    {
        public class PixelInfo
        {
            public PixelInfo(Color color, int x, int y)
            {
                this.color = color;
                this.x = x;
                this.y = y;
                this.item = ItemID.None;
                this.wall = WallID.None;
                this.paint = PaintID.None;
            }

            public Color color;
            public int x;
            public int y;
            public int item;
            public ushort wall;
            public byte paint;
        }

        public static bool Loaded { get; protected set; } = false;
        //
        public static bool PixelInfoLoading { get; protected set; } = false;//像素信息加载中
        public static bool PixelInfoLoaded { get; protected set; } = false;//像素信息加载完成
        public static bool SpawIng { get; protected set; } = false;//像素画生成中
        public static bool SpawPosSelecting = false;//生成位置选择中
        public static bool SwitchPathing = false;//选择路径中

        public static GetSetReset<string> LoadPath = new GetSetReset<string>("img.png", "img.png");//加载路径
        public static GetSetReset<int> SpawSpeed = new GetSetReset<int>(1, 1);//每次更新的生成次数
        public static GetSetReset<int> SpawDistance_val = new GetSetReset<int>(20, 20);
        public static GetSetReset<bool> SetSelectedItem = new GetSetReset<bool>(false, false);//设置玩家手中物品
        public static GetSetReset<int> SpawPosSelectV = new GetSetReset<int>(0, 0, v => v < 0 ? 0 : (v > 3 ? 3 : v));//生成位置选择方向
        public static GetSetReset<bool> DisplayWall = new GetSetReset<bool>(false, false);//显示预览
        public static GetSetReset<int> LoadType = new GetSetReset<int>(0, 0, v => v < 0 ? 0 : (v > 1 ? 1 : v));//加载方式
        public static GetSetReset<bool> LoadUsePaint = new GetSetReset<bool>(false, false);//加载时使用油漆
        //
        private static Point16 spawPos = Point16.Zero;//生成位置
        //
        private static List<PixelInfo> pixelInfo = null;
        private static int pixelInfo_index = 0;
        private static int pixelWidth = 0;
        private static int pixelHeight = 0;
        private static List<Item> wallItemIds = null;
        private static IGetColorWall[] gcw = null;
        private static CancellationTokenSource pixelInfoLoad_cts = null;


        private static void Close()
        {
            Loaded = false;

            CancelLoadPixelInfo();

            EndSpaw();

            ClearPixelInfo();

            gcw = null;

            SpawPosSelecting = false;
        }

        private static void InitPixelArt()
        {
            Close();

            LoadWallItemId();

            //
            List<ColorWall> cws = ItemToColorWall(wallItemIds, LoadUsePaint.val);

            gcw = new IGetColorWall[] { new Get1(), new Get2() };
            foreach (IGetColorWall i in gcw) i.Init(cws);
            //

            Loaded = true;
        }

        public static void Update(Player player)
        {
            if (Loaded == false && Main.dedServ == false)
            {
                InitPixelArt();
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
            for (int i = 0; Update_spaw_Place(player) && i < 2000; ++i) ;
        }

        private static bool Update_spaw_Place(Player player)
        {
            if (pixelInfo_index < pixelInfo.Count != true)
            {
                SpawIng = false;
                Main.NewText("生成完成");
                return false;
            }

            PixelInfo pi = pixelInfo[pixelInfo_index];
            ++pixelInfo_index;
            if (pi == null) return true;
            if (pi.wall == WallID.None) return true;

            int x = spawPos.X + pi.x;
            int y = spawPos.Y + pi.y;

            //

            if (pi.paint == PaintID.None)
            {
                return !Utils.TileUtils.PlaceWall(x, y, pi.wall, pi.item, player, SetSelectedItem.val);
            }

            bool rv = !Utils.TileUtils.PlaceWall(x, y, pi.wall, pi.item, player, SetSelectedItem.val);
            rv &= !Utils.TileUtils.PaintWall(x, y, pi.paint, player, SetSelectedItem.val);

            return rv;
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

        public static void SwitchPath(Action fun)
        {
            if (SwitchPathing) return;
            SwitchPathing = true;

            try
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        string path = Utils1.Utils.GetFileName();
                        if (path == null)
                        {
                            Main.NewText("取消选择");
                            return;
                        }

                        LoadPath.val = path;
                        Main.NewText($"位置:{LoadPath.val}");

                        fun?.Invoke();
                    }
                    finally
                    {
                        SwitchPathing = false;
                    }
                });

                t.Name = "PixelArt SwitchPath";
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch
            {
                SwitchPathing = false;
                Main.NewText("选择失败");
            }
        }

        #region 加载
        public static void LoadPixelInfo()
        {
            IGetColorWall gcw = PixelArt.gcw[LoadType.val];

            _ = LoadPixelInfoAsync(gcw).ContinueWith(t =>
            {
                if (t.IsCanceled) return;
                if (t.Result == null) return;
                Main.NewText($"{t.Result}");
            });
        }

        private static async Task<string> LoadPixelInfoAsync(IGetColorWall gcw)
        {
            PixelInfoLoading = true;

            string path = LoadPath.val;
            ClearPixelInfo();

            pixelInfoLoad_cts = new CancellationTokenSource();

            return await Task<string>.Factory.StartNew((token) =>
            {
                try
                {
                    List<PixelInfo> v = LoadImgToPixelInfo(path, ref pixelWidth, ref pixelHeight, gcw, (CancellationToken)token);

                    if (Loaded == false) return null;

                    ((CancellationToken)token).ThrowIfCancellationRequested();

                    pixelInfo = v;
                    PixelInfoLoading = false;
                    PixelInfoLoaded = true;

                    LoadWallTextureAssets(pixelInfo);//加载下, 不然绘制的时候纹理没加载就不绘制了

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

        private static void LoadWallTextureAssets(List<PixelInfo> pis)
        {
            foreach (PixelInfo i in pis)
            {
                if (i == null) continue;
                if (i.wall < WallID.Count == false) continue;

                Main.instance.LoadWall(i.wall);
            }
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
            int x = spawPos.X;
            int y = spawPos.Y;
            if (WorldGen.InWorld(x, y))
            {
                x += pixelWidth - 1;
                y += pixelHeight - 1;
                if (WorldGen.InWorld(x, y) == false) Main.NewText("超出世界部分将跳过");
            }
            else
            {
                Main.NewText("超出世界部分将跳过");
            }

            pixelInfo_index = 0;
            SpawIng = true;
        }

        public static void EndSpaw()
        {
            SpawIng = false;
        }
        #endregion

        private static void ClearPixelInfo()
        {
            PixelInfoLoaded = false;
            pixelWidth = 0;
            pixelHeight = 0;
            pixelInfo?.Clear();
            pixelInfo = null;
        }

        public static void Draw()
        {
            if (SpawPosSelecting)
            {
                Rectangle rect = new Rectangle(spawPos.X, spawPos.Y, pixelWidth, pixelHeight);

                Color borderColor = new Color(40, 250, 80);

                DrawUtils.Draw_rectangle(rect, borderColor, borderColor * 0.35f, 2);
            }

            if (DisplayWall.val && PixelInfoLoaded) DrawWall();
        }

        private static void DrawWall()
        {
            Point p = Main.screenPosition.ToTileCoordinates();
            Point s = Main.ScreenSize.ToVector2().ToTileCoordinates();

            int add = 0;
            Rectangle rect1 = new Rectangle(p.X - add, p.Y - add, s.X + add * 2, s.Y + add * 2);
            Rectangle rect2 = new Rectangle(spawPos.X, spawPos.Y, pixelWidth, pixelHeight);

            List<PixelInfo> pis = GetIntersectsTile(rect1, rect2, pixelInfo);
            if (pis == null) return;

            foreach (PixelInfo i in pis)
            {
                if (i.wall < WallID.Count == false) continue;

                Asset<Texture2D> asset = TextureAssets.Wall[i.wall];
                if (asset?.Value == null) continue;

                Vector2 pos = (new Point(spawPos.X + i.x, spawPos.Y + i.y)).ToWorldCoordinates(0, 0) - Main.screenPosition;
                Rectangle src = new Rectangle((32 + 4) * 2 + 8, (32 + 4) * 1 + 8, 16, 16);
                Main.spriteBatch.Draw(asset.Value, pos, src, Color.White * 0.5f);
            }
        }

        private static List<PixelInfo> GetIntersectsTile(Rectangle scope, Rectangle size, List<PixelInfo> pis)
        {
            Rectangle rect = Rectangle.Intersect(scope, size);
            if (rect.IsEmpty) return null;

            rect.X -= size.X;
            rect.Y -= size.Y;

            List<PixelInfo> list = new List<PixelInfo>();

            for (int i = 0; i < rect.Height; i++)
            {
                for (int j = 0; j < rect.Width; j++)
                {
                    int index = (rect.Y + i) * size.Width;
                    index += (rect.X + j);

                    if (pis.IndexInRange(index) != true) continue;
                    if (pis[index] == null) continue;

                    list.Add(pis[index]);
                }
            }

            return list;
        }
    }
}
