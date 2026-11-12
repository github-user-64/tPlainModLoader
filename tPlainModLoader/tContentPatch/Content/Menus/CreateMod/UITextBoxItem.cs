using System;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal class UITextBoxItem : UIStackPanel
    {
        public readonly UIText UITitle = null;
        public readonly UIText UITip = null;
        public readonly UITextBox UITextBox = null;
        public Func<string, string> GetTip = null;

        public UITextBoxItem(string title)
        {
            Width.Set(0f, 1f);
            Height.Set(0f, 0f);
            IsAutoUpdateSize = true;

            UIStackPanel sp = new UIStackPanel();
            sp.Width.Set(0f, 1f);
            sp.Height.Set(0f, 0f);
            sp.IsAutoUpdateSize = true;
            sp.Horizontal = true;
            sp.ItemMargin = 4f;
            sp.OnUpdate += UI.Utils.UpdateContainer_Height;
            Append(sp);

            //

            UITitle = new UIText(title);
            sp.Append(UITitle);

            UITip = new UIText(string.Empty);
            sp.Append(UITip);

            //

            UITextBox = new UITextBox();
            UITextBox.Style = UITextBox.Style2;
            UITextBox.MarginTop = 2f;
            Append(UITextBox);

            UITextBox.OnSetText = UpdateTip;
            UITextBox.OnLostFocus += () => UpdateTip(UITextBox.Text);
        }

        protected virtual void UpdateTip(string s)
        {
            string tip = (GetTip?.Invoke(s)) ?? string.Empty;

            UITip.SetText(tip);
        }
    }
}
