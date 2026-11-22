using System;
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
            UIImageButton play = new UIImageButton(_buttonPlayTexture);
            SetButton(play, () => Language.GetTextValue("UI.Play"), PlayGame);
            UILeftButtons.Append(play);

            if (Info.WorldGeneratorVersion != 0L)
            {
                UIImageButton seed = new UIImageButton(_buttonSeedTexture);
                SetButton(seed, () => Language.GetTextValue("UI.CopySeed", Info.GetFullSeedText(allowCropping: true)), CopySeed);
                UILeftButtons.Append(seed);
            }

            UIImageButton del = new UIImageButton(_buttonDeleteTexture);
            del.VAlign = 1f;
            del.HAlign = 1f;
            SetButton(del, () => Language.GetTextValue("UI.Delete"), Delete);
            Append(del);
        }

        protected void SetButton(UIElement uie, Func<string> OnGetText, Action OnClick)
        {
            uie.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                OnClick();
            };
            uie.OnMouseOver += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                SetLabel(OnGetText());
            };
            uie.OnMouseOut += (e, s) => SetLabel();
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
    }
}
