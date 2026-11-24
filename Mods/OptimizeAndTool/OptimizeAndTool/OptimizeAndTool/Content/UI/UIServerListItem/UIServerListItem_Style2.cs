using Microsoft.Xna.Framework;
using OptimizeAndTool.Content.ServerList;
using System;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem
    {
        public static void Style2(UIServerListItem ui)
        {
            ui.Height.Set(80f, 0f);
            ui.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            ui.BorderColor = new Color(89, 116, 213) * 0.7f;
            ui.SetPadding(6f);

            ui.UIBackStyle.Append(new UIServerListItemStyle2(ui));
        }

        public class UIServerListItemStyle2 : UIState
        {
            public readonly UIServerListItem ParentUI = null;
            public ServerInfo Info => ParentUI.Info;

            public UIServerListItemStyle2(UIServerListItem parent)
            {
                ParentUI = parent ?? throw new ArgumentNullException(nameof(parent));
            }

            protected void GetDifficulty(out string expertText, out Color gameModeColor) => ParentUI.GetDifficulty(out expertText, out gameModeColor);
            protected void TryCopyText(string text) => ParentUI.TryCopyText(text);
            protected void SetLabel(string txt = null) => ParentUI.SetLabel(txt);
            protected void SetUILabel(UIElement ui, Func<string> OnGetText) => ParentUI.SetUILabel(ui, OnGetText);
        }
    }
}
