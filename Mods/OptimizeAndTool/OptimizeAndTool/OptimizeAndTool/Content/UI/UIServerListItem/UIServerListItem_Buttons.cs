using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using tContentPatch.Content.UI;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem : AWorldListItem
    {
        protected virtual void InitializeButtons()
        {
            UILeftButtons.Append(GetButton(PlayGame, _buttonPlayTexture, () => Language.GetTextValue("UI.Play")));

            if (Info.WorldGeneratorVersion != 0L)
            {
                UILeftButtons.Append(GetButton(
                    CopySeed,
                    _buttonSeedTexture,
                    () => Language.GetTextValue("UI.CopySeed", Info.GetFullSeedText(allowCropping: true))));
            }

            //

            UIRightButtons.Append(GetTextButton(Up, "上", () => $"选中时按{Keys.Up}也可移动"));
            UIRightButtons.Append(GetTextButton(Down, "下", () => $"选中时按{Keys.Down}也可移动"));

            UIElement del = GetButton(Delete, _buttonDeleteTexture, () => Language.GetTextValue("UI.Delete"));
            del.MarginLeft = 6f;
            UIRightButtons.Append(del);
        }

        protected UIElement GetButton(Action OnClick, Asset<Texture2D> texture, Func<string> OnGetText = null)
        {
            Terraria.GameContent.UI.Elements.UIImageButton ui = new Terraria.GameContent.UI.Elements.UIImageButton(texture);
            ui.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                OnClick();
            };
            if (OnGetText != null) SetUILabel(ui, OnGetText);

            return ui;
        }

        protected UIElement GetTextButton(Action OnClick, string text, Func<string> OnGetText = null)
        {
            UITextButton ui = new UITextButton(text);
            ui.OnClick += OnClick;
            if (OnGetText != null) SetUILabel(ui, OnGetText);

            return ui;
        }

        protected virtual void PlayGame()
        {

        }

        protected virtual void CopySeed()
        {
            TryCopyText(Info.GetFullSeedText());
            SetLabel(Language.GetTextValue("UI.SeedCopied"));
        }

        protected virtual void Delete() => OnDel?.Invoke();

        protected virtual void Down() => OnDown?.Invoke();

        protected virtual void Up() => OnUp?.Invoke();
    }
}
