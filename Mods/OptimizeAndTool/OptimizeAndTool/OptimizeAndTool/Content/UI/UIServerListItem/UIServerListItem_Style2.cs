using Microsoft.Xna.Framework;
using tContentPatch.Content.UI;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem
    {
        public static void Style2(UIServerListItem ui)
        {
            UIStackPanel sp = new UIStackPanel();
            sp.Width.Set(0f, 1f);
            sp.IsAutoUpdateSize = true;
            sp.ItemMargin = 2f;
            ui.Append(sp);

            ui.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            ui.BorderColor = new Color(89, 116, 213) * 0.7f;
            ui.SetPadding(6f);
            ui.OnUpdate += _ => ui.UpdateContainer_Height();

            sp.Append(new UIServerListItemStyle2(ui));
            sp.Append(new UIElementEmpty());
            sp.Append(ui.UIButtons);
        }

        public class UIServerListItemStyle2 : UIServerListItemStyleTemplate
        {
            public UIServerListItemStyle2(UIServerListItem parent) : base(parent)
            {
                UITextBox text = new UITextBox();
                text.Width.Set(-2f, 0.6f);
                text.MaxInputLength = 50;
                text.Style = UITextBox.Style3;
                text.OnGetText = () => Info.DisplayName;
                text.OnSetText = v => Info.DisplayName = v;
                SetUILabel(text, () => $"世界名：{parent.Info.GetWorldName(allowCropping: true)}");
                Append(text);

                UIElement ipPort = GetIpPortUI();
                ipPort.Width = text.Width;
                ipPort.Width.Precent = 1f - ipPort.Width.Precent;
                ipPort.HAlign = 1f;
                Append(ipPort);
            }

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                this.UpdateContainer_Height();
            }
        }
    }
}
