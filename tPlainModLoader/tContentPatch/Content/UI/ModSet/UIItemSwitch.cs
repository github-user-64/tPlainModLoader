using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 开关
    /// </summary>
    public class UIItemSwitch : UIItem
    {
        /// <summary/>
        public readonly UISwitch UISwitch = null;
        /// <summary/>
        public bool IsChecked { get => UISwitch.IsChecked; set => UISwitch.IsChecked = value; }
        /// <summary/>
        public Func<bool> OnGetIsChecked { set => UISwitch.OnGetIsChecked = value; }
        /// <summary/>
        public Action<bool> OnSetIsChecked { set => UISwitch.OnSetIsChecked = value; }

        /// <summary/>
        public UIItemSwitch(Texture2D ico = null, string title = null) : base(ico, title)
        {
            UISwitch = new UISwitch();
            UISwitch.HAlign = 1f;
            Append(UISwitch);
        }
    }
}
