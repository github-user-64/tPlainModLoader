using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 按键绑定
    /// </summary>
    public class UIKeybinding : UIMouseText
    {
        /// <summary/>
        public readonly UIText UIText = null;

        /// <summary/>
        public readonly Utils.UIVal<string> UV = null;
        /// <summary/>
        public string Bind { get => UV.Val; set => UV.SetUIVal(value); }
        /// <summary/>
        public Func<string> OnGetBind { set => UV.OnGetVal = value; }
        /// <summary/>
        public Action<string> OnSetBind { set => UV.OnSetVal = value; }
        /// <summary/>
        public Action<string> OnBindChanged { get => UV.OnValChanged; set => UV.OnValChanged = value; }
        /// <summary/>
        public bool IsBinding = false;

        /// <summary/>
        public UIKeybinding()
        {
            UIText = new UIText(Lang.menu[195].Value, 0.8f, false);
            UIText.TextColor = Color.Gray;
            Append(UIText);

            UV = new Utils.UIVal<string>(UpdateText);
            UV.SetUIVal(null, true);
        }

        /// <summary/>
        public void UpdateText()
        {
            string Bind = this.Bind;

            if (Bind == null)
            {
                UIText.SetText(Lang.menu[195].Value);
                return;
            }

            Bind = Bind.Trim();
            if (Bind == string.Empty) Bind = "<>";
            UIText.SetText(Bind);
        }

        /// <summary/>
        public virtual void StartBind()
        {
            if (IsBinding) return;

            SoundEngine.PlaySound(SoundID.MenuTick);
            IsBinding = true;
        }

        /// <summary/>
        public virtual void SetBind(string bind = null)
        {
            Bind = Bind == bind ? null : bind;
        }

        /// <summary/>
        public virtual void UpdateBind()
        {
            if (IsBinding != true) return;

            if (PlayerInputUtils.KeyDown.Count < 1) return;
            IsBinding = false;

            SetBind(PlayerInputUtils.KeyDown[0]);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UV.UpdateUI();
            UpdateBind();

            UIText.TextColor = IsBinding ? Color.Gold : IsMouseHovering ? Color.White : Bind == null ? Color.Gray : Color.White;

            MinWidth = UIText.MinWidth;
            MinHeight = UIText.MinHeight;
        }

        /// <inheritdoc/>
        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);

            StartBind();
        }
    }
}
