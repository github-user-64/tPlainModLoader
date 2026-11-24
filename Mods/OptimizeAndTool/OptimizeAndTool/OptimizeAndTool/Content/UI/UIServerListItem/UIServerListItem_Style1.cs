using Microsoft.Xna.Framework;
using OptimizeAndTool.Content.ServerList;
using System;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
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

            ui.UIBackStyle.Append(new UIServerListItemStyle1(ui));
        }

        public class UIServerListItemStyle1 : UIState
        {
            private UIElement _worldIcon = null;

            public readonly UIServerListItem ParentUI = null;
            public ServerInfo Info => ParentUI.Info;

            public UIServerListItemStyle1(UIServerListItem parent)
            {
                ParentUI = parent ?? throw new ArgumentNullException(nameof(parent));

                //

                _worldIcon = parent.GetIcoUI(parent.Info);
                Append(_worldIcon);

                //

                UIStackPanel sp = new UIStackPanel();
                sp.Width.Set(-(_worldIcon.Left.Pixels + _worldIcon.Width.Pixels), 1f);
                sp.HAlign = 1f;
                sp.IsAutoUpdateSize = true;
                Append(sp);

                UIServerName name = new UIServerName(parent.Info);
                name.Width.Set(-6f, 1f);
                name.HAlign = 1f;
                name.MarginTop = -4f;
                parent.SetUILabel(name, () => $"世界名：{parent.Info.GetWorldName(allowCropping: true)}");
                sp.Append(name);

                UIElementEmpty uiee = new UIElementEmpty();
                uiee.MarginTop = 2f;
                sp.Append(uiee);

                //

                UIStackPanel infos = new UIStackPanel();
                infos.Width.Set(0f, 1f);
                infos.Height.Set(27f, 0f);
                infos.MarginTop = 4f;
                infos.PaddingLeft = 6f;
                infos.Horizontal = true;
                infos.ItemMargin = 5f;
                sp.Append(infos);

                #region 游戏模式
                GetDifficulty(out var expertText, out var gameModeColor);

                UIPanel2 gameMode = GetInfoUI(expertText, gameModeColor);
                gameMode.Width.Set(100f, 0f);
                infos.Append(gameMode);
                #endregion

                #region 世界大小
                float worldSizeWidth = 100f;
                if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
                {
                    worldSizeWidth += 40f;
                }

                UIPanel2 worldSize = GetInfoUI(Language.GetTextValue("UI.WorldSizeFormat", Info.WorldSizeName), Color.White);
                worldSize.Width.Set(worldSizeWidth, 0f);
                SetUILabel(worldSize, () => $"世界大小：{Info.WorldSizeX}x{Info.WorldSizeY}");
                infos.Append(worldSize);
                #endregion

                #region 地址端口
                UIPanel2 ipPort = GetInfoUI(text: null);
                ipPort.Width.Set(-(gameMode.Width.Pixels + worldSize.Width.Pixels + (infos.ItemMargin * 2f)), 1f);
                infos.Append(ipPort);

                UIStackPanel ipPortSP = new UIStackPanel();
                ipPortSP.Height.Set(0f, 1f);
                ipPortSP.HAlign = 0.5f;
                ipPortSP.VAlign = 0.5f;
                ipPortSP.Horizontal = true;
                ipPortSP.IsAutoUpdateSize = true;
                ipPort.Append(ipPortSP);

                UITextButton ip = GetCopyTextUI(() => Info.IP, 0.8f);
                ip.VAlign = 0.5f;
                ipPortSP.Append(ip);

                UIText uit = new UIText(":", 0.8f);
                uit.VAlign = 0.5f;
                ipPortSP.Append(uit);

                UITextButton port = GetCopyTextUI(() => Info.Port.ToString(), 0.8f);
                port.VAlign = 0.5f;
                ipPortSP.Append(port);
                #endregion
            }

            protected UIPanel2 GetInfoUI(string text = null, Color textColor = default)
            {
                UIPanel2 ui = new UIPanel2();
                ui.Height.Set(0f, 1f);
                ui.BackgroundColor = ui.BorderColor = new Color(43, 56, 101);

                if (text == null) return ui;

                UIText uit = new UIText(text);
                uit.TextColor = textColor;
                uit.HAlign = 0.5f;
                uit.VAlign = 0.5f;
                ui.Append(uit);

                return ui;
            }

            protected UITextButton GetCopyTextUI(Func<string> OnGetText, float textScale = 1f)
            {
                string text = null;

                UITextButton ui = new UITextButton(string.Empty);
                ui.OnUpdate += _ =>
                {
                    text = OnGetText();

                    string uitext = text ?? string.Empty;
                    if (uitext != ui.UIText.Text) ui.UIText.SetText(uitext, textScale, false);
                };
                ui.OnClick += () =>
                {
                    TryCopyText(text);
                    SetLabel("已复制");
                };
                SetUILabel(ui, () => $"复制：{text}");

                return ui;
            }

            protected void GetDifficulty(out string expertText, out Color gameModeColor) => ParentUI.GetDifficulty(out expertText, out gameModeColor);
            protected void TryCopyText(string text) => ParentUI.TryCopyText(text);
            protected void SetLabel(string txt = null) => ParentUI.SetLabel(txt);
            protected void SetUILabel(UIElement ui, Func<string> OnGetText) => ParentUI.SetUILabel(ui, OnGetText);
        }
    }
}
