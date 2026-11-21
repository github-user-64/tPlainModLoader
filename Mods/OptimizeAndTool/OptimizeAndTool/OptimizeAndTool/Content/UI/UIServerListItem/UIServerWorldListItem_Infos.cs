using Microsoft.Xna.Framework;
using tContentPatch.Content.UI;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerWorldListItem : AWorldListItem
    {
        protected virtual void InitializeInfos()
        {
            UIStackPanel sp = new UIStackPanel();
            sp.Width.Set(-(_worldIcon.Left.Pixels + _worldIcon.Width.Pixels), 1f);
            sp.HAlign = 1f;
            sp.IsAutoUpdateSize = true;
            Append(sp);

            UIServerName name = new UIServerName(info);
            name.Width.Set(-6f, 1f);
            name.HAlign = 1f;
            sp.Append(name);

            UIElementEmpty uiee = new UIElementEmpty();
            uiee.MarginTop = 5f;
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

            GetDifficulty(out var expertText, out var gameModeColor);
            UIPanel2Text gameMode = GetInfoUI(expertText);
            gameMode.Width.Set(100f, 0f);
            gameMode.UIText.TextColor = gameModeColor;
            infos.Append(gameMode);

            float worldSizeWidth = 100f;
            if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
            {
                worldSizeWidth += 40f;
            }
            UIPanel2Text worldSize = GetInfoUI(Language.GetTextValue("UI.WorldSizeFormat", info.WorldSizeName));
            worldSize.Width.Set(worldSizeWidth, 0f);
            worldSize.MouseText = $"{info.WorldSizeX}x{info.WorldSizeY}";
            infos.Append(worldSize);

            UIPanel2Text ipPort = GetInfoUI(string.Empty);
            ipPort.Width.Set(-(gameMode.Width.Pixels + worldSize.Width.Pixels + (infos.ItemMargin * 2f)), 1f);
            infos.Append(ipPort);

            //

            ipPort.RemoveChild(ipPort.UIText);

            UIStackPanel ipPortSP = new UIStackPanel();
            ipPortSP.Height.Set(0f, 1f);
            ipPortSP.HAlign = 0.5f;
            ipPortSP.VAlign = 0.5f;
            ipPortSP.Horizontal = true;
            ipPortSP.IsAutoUpdateSize = true;
            ipPort.Append(ipPortSP);

            UIText ip = GetCopyTextUI(info.IP, 0.8f);
            ip.VAlign = 0.5f;
            ipPortSP.Append(ip);

            UIText uit = new UIText(":", 0.8f);
            uit.VAlign = 0.5f;
            ipPortSP.Append(uit);

            UIText port = GetCopyTextUI(info.Port.ToString(), 0.8f);
            port.VAlign = 0.5f;
            ipPortSP.Append(port);
        }

        protected UIPanel2Text GetInfoUI(string text = null)
        {
            UIPanel2Text ui = new UIPanel2Text(text ?? string.Empty);
            ui.Height.Set(0f, 1f);
            ui.BackgroundColor = ui.BorderColor = new Color(43, 56, 101);

            return ui;
        }

        protected UIText GetCopyTextUI(string text = null, float textScale = 1f)
        {
            UIText ui = new UIText(text ?? string.Empty, textScale);
            ui.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                TryCopyText(text);
                SetLabel("已复制");
            };
            ui.OnMouseOver += (e, s) =>
            {
                ui.TextColor = Color.Yellow;
                SoundEngine.PlaySound(SoundID.MenuTick);
                SetLabel($"复制:{text}");
            };
            ui.OnMouseOut += (e, s) =>
            {
                ui.TextColor = Color.White;
                SetLabel();
            };

            return ui;
        }
    }
}
