using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 可输入的文本框
    /// </summary>
    public partial class UITextBox : UIElement
    {
        /// <summary/>
        public static void Style1(UITextBox ui)
        {
            UIPanel back = new UIPanel();
            back.Width.Set(0f, 1f);
            back.Height.Set(0f, 1f);
            back.BackgroundColor = Color.White;
            back.BorderColor = Color.White;

            ui.UIBackStyle.RemoveAllChildren();
            ui.UIBackStyle.Append(back);

            ui.UITextColor = Color.Black;
            ui.UITextColorDeft = Color.Gray;
            ui.UITextShadowColor = ui.UITextShadowColorDeft = back.BackgroundColor;
        }

        /// <summary/>
        public static void Style2(UITextBox ui)
        {
            UIPanel2 back = new UIPanel2();
            back.Width.Set(0f, 1f);
            back.Height.Set(0f, 1f);
            back.BackgroundColor = default;
            back.BorderColor = Color.MidnightBlue * 0.6f;
            back.OnUpdate += _ =>
            {
                back.BorderColor = Color.MidnightBlue;
                if (ui.Focus != true && ui.IsMouseHovering != true) back.BorderColor *= 0.6f;

                back.BackgroundColor = ui.Focus ? Color.MidnightBlue * 0.6f : default;
            };

            ui.UIBackStyle.RemoveAllChildren();
            ui.UIBackStyle.Append(back);

            //

            ui.UITextColor = Color.White;
            ui.UITextColorDeft = Color.MidnightBlue * 0.6f;
            ui.UITextShadowColor = ui.UITextShadowColorDeft = default;

            ui.UIText.SetPadding(6f);
            ui.OnGotFocus += () => SoundEngine.PlaySound(SoundID.MenuTick);
            ui.OnLostFocus += () => SoundEngine.PlaySound(SoundID.MenuClose);
            ui.OnMouseOver += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
