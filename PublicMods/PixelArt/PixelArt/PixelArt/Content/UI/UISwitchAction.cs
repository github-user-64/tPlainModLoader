using Microsoft.Xna.Framework;
using System;
using tContentPatch.Content.UI;

namespace PixelArt.Content.UI
{
    internal class UISwitchAction : UISwitch
    {
        private Func<bool> func = null;

        public UISwitchAction(Func<bool> func, Action open, Action close)
        {
            this.func = func;

            SetVal(this.func());

            OnLeftClick += (e, s) =>
            {
                if (func()) close();
                else open();
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool v = func();
            if (v == GetVal()) return;

            SetVal(v);
        }
    }
}
