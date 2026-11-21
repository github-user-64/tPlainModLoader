using Microsoft.Xna.Framework;
using OptimizeAndTool.Content.ServerList;
using System;
using tContentPatch.Content.UI;
using Terraria.Audio;
using Terraria.ID;

namespace OptimizeAndTool.Content.UI
{
    public class UIServerName : UITextBox
    {
        public readonly ServerInfo Info = null;

        public UIServerName(ServerInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));

            string serverWorldName = Info.GetWorldName(allowCropping: true);

            DefaultDisplayText = serverWorldName;
            MouseText = serverWorldName;
            Style = StyleServerName;

            OnGetText = () => Info.DisplayText;
            OnSetText = v => Info.DisplayText = v;
        }

        protected static void StyleServerName(UITextBox ui)
        {
            UIPanel2 back = new UIPanel2();
            back.Width.Set(0f, 1f);
            back.Height.Set(0f, 1f);
            back.BackgroundColor = default;
            back.BorderColor = default;
            back.OnUpdate += _ =>
            {
                if (ui.Focus || ui.IsMouseHovering) back.BorderColor = Color.MidnightBlue;
                else back.BorderColor = default;

                back.BackgroundColor = ui.Focus ? Color.MidnightBlue * 0.6f : default;
            };
            ui.UIBackStyle.Append(back);

            ui.UITextColor = ui.UITextColorDeft = Color.White;
            ui.UITextColorDeft *= 0.9f;

            ui.UIText.SetPadding(6f);
            ui.OnGotFocus += () => SoundEngine.PlaySound(SoundID.MenuTick);
            ui.OnLostFocus += () => SoundEngine.PlaySound(SoundID.MenuClose);
            ui.OnMouseOver += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
