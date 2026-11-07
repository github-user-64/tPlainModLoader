using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 组合ui: <see cref="UI.UIScrollViewer"/>, <see cref="UI.UIWrapPanel"/>
    /// </summary>
    public class UIScrollViewer3 : UIElement
    {
        /// <summary/>
        public readonly UIScrollViewer UIScrollViewer = null;
        /// <summary/>
        public readonly UIWrapPanel UIWrapPanel = null;
        /// <summary/>
        public float ItemMargin { get => UIWrapPanel.ItemMargin; set => UIWrapPanel.ItemMargin = value; }

        /// <param name="scrollbarIsLeft">滚动条是否在左边</param>
        public UIScrollViewer3(bool scrollbarIsLeft = false)
        {
            UIScrollViewer = new UIScrollViewer(scrollbarIsLeft);
            UIScrollViewer.Width.Set(0f, 1f);
            UIScrollViewer.Height.Set(0f, 1f);
            Append(UIScrollViewer);

            UIWrapPanel = new UIWrapPanel();
            UIWrapPanel.Width.Set(0f, 1f);
            UIWrapPanel.IsAutoUpdateSize = true;
            UIScrollViewer.SetChild(UIWrapPanel);
        }

        /// <summary/>
        public void AddChild(UIElement uie)
        {
            if (uie == null) return;

            UIWrapPanel.Append(uie);
        }

        /// <summary/>
        public void ClearChild()
        {
            UIWrapPanel.RemoveAllChildren();
        }
    }
}
