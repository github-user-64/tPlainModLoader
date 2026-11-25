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
        /// <summary>
        /// 样式模板
        /// </summary>
        public class UIServerListItemStyleTemplate : UIElement
        {
            public readonly UIServerListItem ParentUI = null;
            public ServerInfo Info => ParentUI.Info;

            public UIServerListItemStyleTemplate(UIServerListItem parent)
            {
                ParentUI = parent ?? throw new ArgumentNullException(nameof(parent));

                Width.Set(0f, 1f);
            }

            protected UIElement GetGameModeUI()
            {
                GetDifficulty(out var expertText, out var gameModeColor);

                UIPanel2 gameMode = GetInfoUI(expertText, gameModeColor);
                gameMode.Width.Set(100f, 0f);

                return gameMode;
            }

            protected UIElement GetWorldSizeUI()
            {
                float worldSizeWidth = 100f;
                if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
                {
                    worldSizeWidth += 40f;
                }

                UIPanel2 worldSize = GetInfoUI(Language.GetTextValue("UI.WorldSizeFormat", Info.WorldSizeName), Color.White);
                worldSize.Width.Set(worldSizeWidth, 0f);
                SetUILabel(worldSize, () => $"世界大小：{Info.WorldSizeX}x{Info.WorldSizeY}");

                return worldSize;
            }

            protected UIElement GetIpPortUI()
            {
                UIPanel2 ipPort = GetInfoUI(text: null);

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

                return ipPort;
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
