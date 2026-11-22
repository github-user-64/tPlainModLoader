using OptimizeAndTool.Utils;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    /// <summary>
    /// 可以移动项位置的<see cref="UIStackPanel"/>
    /// </summary>
    public class UIStackPanelMove : UIStackPanel
    {
        public virtual void AddItem(UIElement ui, int index)
        {
            Append(ui);

            for (int i = Elements.Count - 1; ; --i)
            {
                if (i < 0) return;

                if (Elements[i] != ui) continue;

                Elements.RemoveAt(i);
                break;
            }

            Elements.Add(ui, index);
        }

        public virtual void UpItem(UIElement ui) => Elements.OffUp(ui);
        public virtual void DownItem(UIElement ui) => Elements.OffDown(ui);

        /// <summary>
        /// <see cref="UIElement.Elements"/>查找<seealso cref="List{T}.IndexOf(T)"/>
        /// </summary>
        public virtual int ElementsIndexOf(UIElement ui)
        {
            return Elements.IndexOf(ui);
        }
    }
}
