using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SundryTool.Content.UI
{
    internal class UIDrawer : UIStackPanel
    {
        private GetSetReset<bool> open = new GetSetReset<bool>();
        private UIElement ui = null;
        private UIStackPanel ui_sp = null;

        public UIDrawer(string ico = null, string text = null)
        {
            Width.Precent = 1f;
            IsAutoUpdateSize = true;

            ui = UIBuild.get2(open, null, ico, text);

            ui_sp = new UIStackPanel();
            ui_sp.Width.Precent = 1;
            ui_sp.IsAutoUpdateSize = true;

            open.OnValUpdate += a;
            open.OnValUpdate.Invoke(false);
        }

        private void a(bool obj)
        {
            if (obj)
            {
                RemoveAllChildren();
                Append(ui);
                Append(ui_sp);
            }
            else
            {
                RemoveAllChildren();
                Append(ui);
            }
        }

        public void Add(UIElement uie)
        {
            ui_sp.Append(uie);
        }
    }
}
