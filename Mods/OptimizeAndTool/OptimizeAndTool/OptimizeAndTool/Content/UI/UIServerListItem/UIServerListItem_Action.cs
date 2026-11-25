using OptimizeAndTool.Content.UI.Menus;
using ReLogic.OS;
using tContentPatch.Content;
using Terraria;
using Terraria.Localization;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem
    {
        protected virtual void PlayGame()
        {
            ServerList.ServerList.Join(Info);
        }

        protected virtual void CopySeed()
        {
            TryCopyText(Info.GetFullSeedText());
            SetLabel(Language.GetTextValue("UI.SeedCopied"));
        }

        protected virtual void EditInfo()
        {
            UIEditServerInfo ui = new UIEditServerInfo(Info, Main.MenuUI?.CurrentState);
            ui.NoGamepadSupport = true;//不支持游戏手柄, 取消按移动键鼠标会定位到ui上的功能

            GameUitl.OpenMenu(ui);
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
