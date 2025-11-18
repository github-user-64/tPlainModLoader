namespace WandsTool.Content
{
    public partial class gameMain
    {
        public static bool UI_WandsPanel1_isOpen = false;

        public static bool Wand_isEnable = false;
        public static int Wand_UpdateCount = 1;//更新次数
        public static bool Wand_isPlace = true;//是放置
        public static bool Wand_Tile = true;
        public static bool Wand_Wall = false;
        public static Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode Wand_ToolMode = 0;
        public static Wands.Shapes Wand_Shapes = Wands.Shapes.line;//形状
        public static WandAction.BlockType Wand_BlockType = WandAction.BlockType.Solid;//方向
    }
}
