using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace tContentPatch.Content.UI
{
    public partial class UITextBox
    {
        /// <summary/>
        protected static readonly FieldInfo _textSize = typeof(UIText).GetField("_textSize", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary/>
        public Color UISelectColor = Color.White;

        /// <summary>
        /// 光标位置
        /// </summary>
        public int Select
        {
            get
            {
                return select = (int)MathHelper.Clamp(select, 0, DisplayText?.Length ?? 0);
            }
            set
            {
                select = value;
                ResetSelectTime();
            }
        }
        private int select = 0;
        private int SelectTime = 0;

        /// <summary/>
        protected virtual void DrawSelect(SpriteBatch spriteBatch)
        {
            if (Focus != true || SelectTime * 16 > 1000) return;

            (string, string) ss = GetSelectText();
            Vector2 size = GetUITextSize(ss.Item1);

            Rectangle rect = UIText.GetInnerDimensions().ToRectangle();
            rect.X += (int)size.X;
            rect.Width = 1;

            spriteBatch.Draw(Main.Assets.Request<Texture2D>("Images/UI/OnePixel").Value, rect, UISelectColor);
        }

        /// <summary/>
        protected virtual void UpdateSelect()
        {
            if (InputUtils.IsClick(Keys.Left))
            {
                --Select;
            }
            else if (InputUtils.IsClick(Keys.Right))
            {
                ++Select;
            }
            else if (InputUtils.IsClick(Keys.End))
            {
                Select = DisplayText?.Length ?? 0;
            }
            else if (InputUtils.IsClick(Keys.Home))
            {
                Select = 0;
            }
            else if (++SelectTime * 16 > 2000)
            {
                ResetSelectTime();
            }
        }

        /// <summary/>
        protected virtual Vector2 GetUITextSize(string txt = null)
        {
            if (txt == null) return Vector2.Zero;

            string origin = UIText.Text;
            UIText.SetText(txt);
            Vector2 size = (Vector2)_textSize.GetValue(UIText);
            UIText.SetText(origin);

            return size;
        }

        /// <summary>
        /// 获取被光标分割的<see cref="DisplayText"/>, <see cref="string"/>不会为<see langword="null"/>
        /// </summary>
        protected virtual (string, string) GetSelectText()
        {
            string txt = DisplayText ?? string.Empty;
            int select = Select;

            if (txt.Length < 1) return (string.Empty, string.Empty);
            if (select < 1) return (string.Empty, txt);

            string s1 = txt.Substring(0, select);
            string s2 = txt.Substring(select, txt.Length - select);

            return (s1, s2);
        }

        /// <summary/>
        protected virtual void ResetSelectTime()
        {
            SelectTime = 0;
        }
    }
}
