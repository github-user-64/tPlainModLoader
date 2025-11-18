using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using tContentPatch.Content.UI.ModSet;
using Terraria.GameContent.UI.Elements;

namespace QuickSetting.KeyBind
{
    internal class UIKeyBind : UIItem
    {
        public Action<string> OnKeyUpdate = null;
        private string _key = null;
        private UIText ui_text = null;
        private bool listening = false;

        public UIKeyBind(Texture2D ico = null, string text = null) : base(ico, text)
        {
            ui_text = new UIText(Terraria.Lang.menu[195].Value);
            ui_text.HAlign = 1;
            ui_text.VAlign = 0.5f;
            ui_text.TextColor = Color.Gray;

            OnLeftClick += (e, s) =>
            {
                if (listening) return;
                listening = true;

                ui_text.TextColor = Color.Gold;

                ListenInput.AddListenInputOne(v => SetKey(v));
            };

            Append(ui_text);
        }

        public void SetKey(string key)
        {
            if (key == _key) key = null;
            if (key == _key) return;

            _key = key;
            listening = false;

            ui_text.SetText(_key == null ? Terraria.Lang.menu[195].Value : _key);
            ui_text.TextColor = _key == null ? Color.Gray : Color.White;

            OnKeyUpdate?.Invoke(_key);
        }
    }
}
