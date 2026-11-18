using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace tContentPatch.Content.UI
{
    public partial class UISwitch
    {
        /// <summary>
        /// 文本和圆点
        /// </summary>
        public static void Style1(UISwitch ui)
        {
            ui.Width.Set(50f, 0f);
            ui.Height.Set(20f, 0f);

            UIText text = new UIText("-", 0.8f);
            text.VAlign = 0.5f;
            ui.UIBackStyle.Append(text);

            Asset<Texture2D> img = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", AssetRequestMode.ImmediateLoad);
            Rectangle Frame = img.Frame(horizontalFrames: 2);

            UIImageFramed ico = new UIImageFramed(img, Frame);
            ico.HAlign = 1f;
            ico.VAlign = 0.5f;
            ui.UIBackStyle.Append(ico);

            //

            Action<bool> update = v =>
            {
                v = ui.IsChecked;

                text.SetText(v ? "开" : "关");

                Rectangle f = Frame;
                if (v) f.X += f.Height;

                ico.SetFrame(f);
            };

            ui.OnIsCheckChanged += update;
            ui.UI1.OnLeftClick += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
            ui.UI1.OnMouseOver += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                text.TextColor = ico.Color = Colors.FancyUIFatButtonMouseOver;
            };
            ui.UI1.OnMouseOut += (e, s) => text.TextColor = ico.Color = Color.White;

            //

            update(ui.IsChecked);
        }

        /// <summary>
        /// 一个方框
        /// </summary>
        public static void Style2(UISwitch ui)
        {
            UISwitchStyle2 styleui = new UISwitchStyle2(ui);
            ui.UIBackStyle.Append(styleui);

            ui.Width = styleui.Width;
            ui.Height = styleui.Height;
            styleui.Width.Set(0f, 1f);
            styleui.Height.Set(0f, 1f);

            ui.UI1.OnLeftClick += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
            ui.UI1.OnMouseOver += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
