using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 组合ui: <see cref="UI.UIScrollViewer"/>, <see cref="UI.UIStackPanel"/>
    /// </summary>
    public class UIScrollViewer2 : UIElement
    {
        /// <summary/>
        public readonly UIScrollViewer UIScrollViewer = null;
        /// <summary/>
        public readonly UIStackPanel UIStackPanel = null;
        /// <summary/>
        public float ItemMargin { get => UIStackPanel.ItemMargin; set => UIStackPanel.ItemMargin = value; }

        /// <param name="scrollbarIsLeft">滚动条是否在左边</param>
        public UIScrollViewer2(bool scrollbarIsLeft = false)
        {
            UIScrollViewer = new UIScrollViewer(scrollbarIsLeft);
            UIScrollViewer.Width.Set(0f, 1f);
            UIScrollViewer.Height.Set(0f, 1f);
            Append(UIScrollViewer);

            UIStackPanel = new UIStackPanel();
            UIStackPanel.Width.Set(0f, 1f);
            UIStackPanel.IsAutoUpdateSize = true;
            UIScrollViewer.SetChild(UIStackPanel);
        }

        /// <summary/>
        public void AddChild(UIElement uie)
        {
            if (uie == null) return;

            UIStackPanel.Append(uie);
        }

        /// <summary/>
        public void ClearChild()
        {
            UIStackPanel.RemoveAllChildren();
        }
    }
}
