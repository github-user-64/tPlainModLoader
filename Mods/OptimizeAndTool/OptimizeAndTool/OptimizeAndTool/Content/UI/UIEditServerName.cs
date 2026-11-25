using Microsoft.Xna.Framework;
using OptimizeAndTool.Content.ServerList;
using System;
using tContentPatch.Content.UI;

namespace OptimizeAndTool.Content.UI
{
    public class UIEditServerName : UITextBox
    {
        public readonly ServerInfo Info = null;

        public UIEditServerName(ServerInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));

            DefaultDisplayText = Info.GetWorldName(allowCropping: true);
            MaxInputLength = 50;
            Style = v =>
            {
                Style3(v);
                v.UITextColor = v.UITextColorDeft = Color.White;
            };

            OnGetText = () => Info.DisplayName;
            OnSetText = v => Info.DisplayName = v;
        }
    }
}
