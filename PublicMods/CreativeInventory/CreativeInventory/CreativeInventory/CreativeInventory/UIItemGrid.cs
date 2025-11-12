using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace CreativeInventory.CreativeInventory
{
    /// <summary>
    /// 
    /// </summary>
    public class UIItemGrid : UIElement
    {
        private Item _item;
        private int _itemSlotContext;


        public UIItemGrid(Item item, int itemSlotContext)
        {
            _item = item;
            _itemSlotContext = itemSlotContext;
            Width = new StyleDimension(48f, 0f);
            Height = new StyleDimension(48f, 0f);
        }


        private void HandleItemSlotLogic()
        {
            if (base.IsMouseHovering)
            {
                Player player = Main.LocalPlayer;
                if (player != null) player.mouseInterface = true;
                ItemSlot.OverrideHover(ref _item, _itemSlotContext);
                ItemSlot.LeftClick(ref _item, _itemSlotContext);
                ItemSlot.RightClick(ref _item, _itemSlotContext);
                ItemSlot.MouseHover(ref _item, _itemSlotContext);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //让没在显示区域的控件不要绘制, 防止一次加载太多资源导致卡顿
            if (IsOverflowHidden(this, Parent)) return;

            HandleItemSlotLogic();
            Vector2 position = GetDimensions().Center() + new Vector2(52f, 52f) * -0.5f * Terraria.Main.inventoryScale;
            ItemSlot.Draw(spriteBatch, ref _item, _itemSlotContext, position);
        }

        private bool IsOverflowHidden(Terraria.UI.UIElement uie, Terraria.UI.UIElement parent, float offx = 0, float offy = 0)
        {
            if (uie == null) return false;
            if (parent == null) return false;

            if (parent.OverflowHidden)
            {
                Rectangle parentR = parent.GetInnerDimensions().ToRectangle();

                float uiex = uie.Left.Pixels + offx;
                float uiey = uie.Top.Pixels + offy;

                if (parentR.Width <= uiex) return true;
                if (parentR.Height <= uiey) return true;
                if (0 >= uiex + uie.Width.Pixels) return true;
                if (0 >= uiey + uie.Height.Pixels) return true;
            }

            offx += parent.Left.Pixels;
            offy += parent.Top.Pixels;

            return IsOverflowHidden(uie, parent.Parent, offx, offy);
        }
    }
}
