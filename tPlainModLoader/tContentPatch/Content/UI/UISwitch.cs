using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 开关
    /// </summary>
    public class UISwitch : UIElement
    {
        public Action<bool> OnValUpdate = null;
        private bool val = false;
        private Action<bool> updateText = null;
        private Action<bool> updateImg = null;

        public UISwitch()
        {
            Width.Pixels = 50;

            UIText ui_text = new UIText("-", 0.8f);
            ui_text.VAlign = 0.5f;
            updateText = v => ui_text.SetText(v ? "开" : "关");

            ReLogic.Content.Asset<Texture2D> img = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", ReLogic.Content.AssetRequestMode.ImmediateLoad);
            int imgWidth = img.Width() / 2;

            UIImageFramed ui_img = new UIImageFramed(img, new Rectangle(0, 0, imgWidth, img.Height()));
            ui_img.HAlign = 1;
            ui_img.VAlign = 0.5f;
            updateImg = v => ui_img.SetFrame(new Rectangle(v ? imgWidth : 0, 0, imgWidth, img.Height()));

            UIElement uie = new UIElement();
            uie.Width.Precent = 1;
            uie.Height.Precent = 1;
            uie.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(12);
                SetVal(!val);
            };
            uie.OnMouseOver += (e, s) =>
            {
                SoundEngine.PlaySound(12);
                ui_text.TextColor = ui_img.Color = Colors.FancyUIFatButtonMouseOver;
            };
            uie.OnMouseOut += (e, s) => ui_text.TextColor = ui_img.Color = Color.White;

            updateText(val);
            updateImg(val);

            Append(ui_text);
            Append(ui_img);
            Append(uie);
        }

        public void SetVal(bool v)
        {
            if (v == val) return;
            val = v;
            OnValUpdate?.Invoke(val);
            updateText(val);
            updateImg(val);
        }

        public bool GetVal() => val;
    }
}
