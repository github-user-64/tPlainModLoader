using Microsoft.Xna.Framework;
using System;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModLoadingMenu
{
    internal class UILoadProgressBar : UIState
    {
        private UIPanel ui_panel = null;
        private UIText ui_tip = null;
        private UIProgressBar ui_progressBar = null;
        private UIButton1 ui_button_cancel = null;//取消加载
        private IModLoaderState loadState = null;
        private Action OnCancelLoad = null;

        public UILoadProgressBar()
        {
            ui_panel = new UIPanel();
            ui_panel.Width.Pixels = 650;
            ui_panel.Height.Pixels = 180;
            ui_panel.HAlign = 0.5f;
            ui_panel.VAlign = 0.5f;
            ui_panel.OverflowHidden = true;

            ui_tip = new UIText(string.Empty, 0.8f, true);
            ui_tip.Width.Precent = 1;
            ui_tip.Top.Pixels = 20;

            ui_progressBar = new UIProgressBar();
            ui_progressBar.Width.Set(-20, 1);
            ui_progressBar.Height.Pixels = 15;
            ui_progressBar.HAlign = 0.5f;
            ui_progressBar.VAlign = 0.5f;

            ui_button_cancel = new UIButton1("取消");
            ui_button_cancel.Top.Set(-1, 0);
            ui_button_cancel.HAlign = 0.5f;
            ui_button_cancel.VAlign = 1;
            ui_button_cancel.OnLeftClick += (e, s) => OnCancelLoad?.Invoke();

            Append(ui_panel);
            ui_panel.Append(ui_tip);
            ui_panel.Append(ui_progressBar);
            ui_panel.Append(ui_button_cancel);
        }

        public void InitializeLoader(IModLoaderState loadState, Action cancelLoad)
        {
            this.loadState = loadState;
            ui_tip.SetText(string.Empty);

            OnCancelLoad = cancelLoad;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            loadState.ProgressBar(out int val, out int max);
            ui_progressBar.SetVal(val, max);
            ui_tip.SetText(loadState.GetTip());
        }
    }
}
