using Microsoft.Xna.Framework;
using tContentPatch.Content.UI;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem
    {
        public static void Style1(UIServerListItem ui)
        {
            ui.Height.Set(100f, 0f);
            ui.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            ui.BorderColor = new Color(89, 116, 213) * 0.7f;
            ui.SetPadding(6f);
            ui.PaddingTop = 8f;

            ui.UIButtons.VAlign = 1f;

            ui.Append(new UIServerListItemStyle1(ui));
        }

        public class UIServerListItemStyle1 : UIServerListItemStyleTemplate
        {
            private readonly UIElement UIWorldIcon = null;

            public UIServerListItemStyle1(UIServerListItem parent) : base(parent)
            {
                UIWorldIcon = ParentUI.GetIcoUI(Info);
                Append(UIWorldIcon);

                //

                UIStackPanel sp = new UIStackPanel();
                sp.Width.Set(-(UIWorldIcon.Left.Pixels + UIWorldIcon.Width.Pixels), 1f);
                sp.HAlign = 1f;
                sp.IsAutoUpdateSize = true;
                Append(sp);

                UIServerName name = new UIServerName(Info);
                name.Width.Set(-6f, 1f);
                name.HAlign = 1f;
                name.MarginTop = -4f;
                SetUILabel(name, () => $"世界名：{Info.GetWorldName(allowCropping: true)}");
                sp.Append(name);

                UIElementEmpty uiee = new UIElementEmpty();
                uiee.MarginTop = 2f;
                sp.Append(uiee);

                //显示信息
                UIStackPanel infos = new UIStackPanel();
                infos.Width.Set(0f, 1f);
                infos.Height.Set(27f, 0f);
                infos.MarginTop = 4f;
                infos.PaddingLeft = 6f;
                infos.Horizontal = true;
                infos.ItemMargin = 5f;
                sp.Append(infos);

                //游戏模式
                UIElement gameMode = GetGameModeUI();
                infos.Append(gameMode);

                //世界大小
                UIElement worldSize = GetWorldSizeUI();
                infos.Append(worldSize);

                //地址端口
                UIElement ipPort = GetIpPortUI();
                ipPort.Width.Set(-(gameMode.Width.Pixels + worldSize.Width.Pixels + (infos.ItemMargin * 2f)), 1f);
                infos.Append(ipPort);
            }

            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);

                this.UpdateContainer_Height();
            }
        }
    }
}
