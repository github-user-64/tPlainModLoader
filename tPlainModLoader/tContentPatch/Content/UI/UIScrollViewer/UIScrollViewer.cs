using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 有滚动条的ui
    /// </summary>
    public class UIScrollViewer : UIElement
    {
        /// <summary/>
        public readonly UIList UIList = null;
        /// <summary/>
        public readonly UIScrollbar UIScrollbar = null;

        /// <param name="scrollbarIsLeft">滚动条是否在左边</param>
        public UIScrollViewer(bool scrollbarIsLeft = false)
        {
            UIScrollbar = new UIScrollbar();
            UIScrollbar.Height.Set(-12f, 1f);
            UIScrollbar.HAlign = scrollbarIsLeft ? 0f : 1f;
            UIScrollbar.VAlign = 0.5f;
            Append(UIScrollbar);

            UIList = new UIList();
            UIList.Width.Set(-25f, 1f);
            UIList.Height.Set(0f, 1f);
            UIList.HAlign = scrollbarIsLeft ? 1f : 0f;
            UIList.SetScrollbar(UIScrollbar);
            Append(UIList);
        }

        /// <summary/>
        public void SetChild(UIElement uie)
        {
            UIList.Clear();
            if (uie == null) return;
            UIList.Add(uie);
        }
    }
}
