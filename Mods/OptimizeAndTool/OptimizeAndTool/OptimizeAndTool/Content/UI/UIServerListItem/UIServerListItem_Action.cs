using ReLogic.OS;
using Terraria.Localization;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem
    {
        protected virtual void PlayGame()
        {

        }

        protected virtual void CopySeed()
        {
            TryCopyText(Info.GetFullSeedText());
            SetLabel(Language.GetTextValue("UI.SeedCopied"));
        }

        protected virtual void EditInfo()
        {

        }

        protected virtual void Delete() => OnDel?.Invoke();

        protected virtual void Down() => OnDown?.Invoke();

        protected virtual void Up() => OnUp?.Invoke();

        //

        protected void SetLabel(string txt = null)
        {
            UILabel.SetText(txt ?? string.Empty);
        }

        protected void TryCopyText(string text = null)
        {
            try
            {
                if (text != null) Platform.Get<IClipboard>().Value = text;
            }
            catch { }
        }
    }
}
