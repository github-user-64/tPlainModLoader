using Microsoft.Xna.Framework;
using System;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 开关
    /// </summary>
    public partial class UISwitch : UIMouseText
    {
        /// <summary/>
        public readonly Utils.UIVal<bool> UV = null;
        /// <summary/>
        public virtual bool IsChecked { get => UV.Val; set => UV.SetUIVal(value); }
        /// <summary/>
        public virtual Func<bool> OnGetIsChecked { set => UV.OnGetVal = value; }
        /// <summary/>
        public virtual Action<bool> OnSetIsChecked { set => UV.OnSetVal = value; }
        /// <summary/>
        public virtual Action<bool> OnIsCheckChanged { get => UV.OnValChanged; set => UV.OnValChanged = value; }

        /// <summary/>
        public readonly UIElement UI1 = null;
        /// <summary>
        /// ui的背景
        /// </summary>
        public readonly UIElement UIBackStyle = null;

        /// <summary>
        /// 设置样式
        /// </summary>
        public Action<UISwitch> Style = Style1;
        private bool StyleLoaded = false;

        /// <summary/>
        public UISwitch()
        {
            UIBackStyle = new UIState();
            Append(UIBackStyle);

            UI1 = new UIState();
            UI1.OnLeftClick += (e, s) => UV.SetUIVal(!IsChecked);
            Append(UI1);

            UV = new Utils.UIVal<bool>(null);
            UV.SetUIVal(false, true);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (StyleLoaded == false)
            {
                StyleLoaded = true;
                ApplyBackStyle();
            }

            base.Update(gameTime);

            UV.UpdateUI();
        }

        /// <summary>
        /// 设置页面背景
        /// </summary>
        protected virtual void ApplyBackStyle()
        {
            if (Style == null) return;

            Style(this);
            Recalculate();
        }
    }
}
