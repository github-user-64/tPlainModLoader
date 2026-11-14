using Microsoft.Xna.Framework;
using ReLogic.Localization.IME;
using ReLogic.OS;
using System;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    public partial class UITextBox
    {
        /// <summary/>
        protected virtual void UpdateInput()
        {
            if (Focus == false) return;

            (string, string) ss = GetSelectText();

            //输入法没东西时, 因为使用tPlainModLoader启动时输入法不会正常显示, 所以使用CompositionString
            if (Platform.Get<IImeService>().CompositionString.Length < 1)
            {
                UpdateInput_Delete(ref ss.Item2);
            }

            #region
            Terraria.GameInput.PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            string s = Main.GetInputText(ss.Item1);

            CalculatedStyle size = GetDimensions();
            DrawIME.NeedIME = true;
            DrawIME.IME_P = new Vector2(size.X, size.Y + size.Height + 36f);
            #endregion

            if (MaxInputLength < 0) s = CheckInputText(s, MaxInputLength);
            else s = CheckInputText(s, (int)MathHelper.Clamp(MaxInputLength - ss.Item2.Length, 0, MaxInputLength));

            int select = s?.Length ?? 0;
            s = $"{s}{ss.Item2}";//不用+=防止null

            if (s == DisplayText) return;

            DisplayText = s;
            Select = select;
        }

        /// <summary>
        /// 向后删除文本
        /// </summary>
        protected virtual void UpdateInput_Delete(ref string s)
        {
            int count = InputUtils.DownCount(Microsoft.Xna.Framework.Input.Keys.Delete);
            if (count != 1 && count < 30) return;

            if (s.Length > 0) s = s.Substring(1, s.Length - 1);
        }

        /// <summary/>
        protected virtual string CheckInputText(string s = null, int maxInputLength = -1)
        {
            if (s == null) return null;
            if (maxInputLength < 0) return s;
            if (maxInputLength >= s.Length) return s;

            return s.Substring(0, Math.Min(s.Length, maxInputLength));
        }

        /// <summary>
        /// 在<see cref="Text"/>更新时更新<see cref="DisplayText"/>
        /// </summary>
        protected virtual void UpdateDisplayTextWhenText()
        {
            //有焦点且不实时更新文本时
            if (Focus == true && NoUpdateDisplayTextWhenHasFocus == true) return;

            DisplayText = Text;
        }

        /// <summary>
        /// 在<see cref="DisplayText"/>更新时更新<see cref="Text"/>
        /// </summary>
        protected virtual void UpdateTextWhenDisplayText()
        {
            if (IsUpdateTextWhenDisplayText) Text = DisplayText;
        }

        /// <summary>
        /// 在<see cref="Focus"/>为<see langword="false"/>时更新<see cref="Text"/>
        /// </summary>
        protected virtual void UpdateTextWhenLostFocus()
        {
            if (IsUpdateTextWhenLostFocus) Text = DisplayText;
        }
    }
}
