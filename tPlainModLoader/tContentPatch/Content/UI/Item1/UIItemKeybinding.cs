using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.Item1
{
    /// <summary>
    /// 按键绑定
    /// </summary>
    public class UIItemKeybinding : UIItem
    {
        /// <summary/>
        public readonly UIKeybinding UIKeybinding = null;

        /// <summary/>
        public string Bind { get => UIKeybinding.Bind; set => UIKeybinding.Bind = value; }
        /// <summary/>
        public Func<string> OnGetBind { set => UIKeybinding.OnGetBind = value; }
        /// <summary/>
        public Action<string> OnSetBind { set => UIKeybinding.OnSetBind = value; }
        /// <summary/>
        public Action<string> OnBindChanged { get => UIKeybinding.OnBindChanged; set => UIKeybinding.OnBindChanged = value; }

        /// <summary/>
        public UIItemKeybinding(Texture2D ico = null, string title = null) : base(ico, title)
        {
            UIKeybinding = new UIKeybinding();
            UIKeybinding.Width.Set(0f, 1f);
            UIKeybinding.Height.Set(0f, 1f);
            UIKeybinding.UIText.HAlign = 1f;
            UIKeybinding.UIText.VAlign = 0.5f;
            Append(UIKeybinding);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UITitle.TextColor = UIKeybinding.UIText.TextColor;
        }
    }
}
