using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.Item1
{
    /// <summary>
    /// 开关
    /// </summary>
    public class UIItemSwitch : UIItem
    {
        /// <summary/>
        public readonly UISwitch UISwitch = null;

        /// <summary>
        /// <see cref="UISwitch.IsChecked"/>
        /// </summary>
        public virtual bool IsChecked { get => UISwitch.IsChecked; set => UISwitch.IsChecked = value; }
        /// <summary/>
        public virtual Func<bool> OnGetIsChecked { set => UISwitch.OnGetIsChecked = value; }
        /// <summary/>
        public virtual Action<bool> OnSetIsChecked { set => UISwitch.OnSetIsChecked = value; }
        /// <summary/>
        public virtual Action<bool> OnIsCheckChanged { get => UISwitch.OnIsCheckChanged; set => UISwitch.OnIsCheckChanged = value; }

        /// <summary/>
        public UIItemSwitch(Texture2D ico = null, string title = null) : base(ico, title)
        {
            UISwitch = new UISwitch();
            UISwitch.Style = v =>
            {
                UISwitch.Style1(v);
                v.Height.Set(0f, 1f);
            };
            UISwitch.HAlign = 1f;
            UISwitch.VAlign = 0.5f;
            Append(UISwitch);
        }
    }
}
