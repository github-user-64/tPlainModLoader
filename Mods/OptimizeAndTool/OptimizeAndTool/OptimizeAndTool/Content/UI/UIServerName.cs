using OptimizeAndTool.Content.ServerList;
using System;
using tContentPatch.Content.UI;

namespace OptimizeAndTool.Content.UI
{
    public class UIServerName : UITextBox
    {
        public readonly ServerInfo Info = null;

        public UIServerName(ServerInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));

            DefaultDisplayText = Info.GetWorldName(allowCropping: true);
            Style = Style3;

            OnGetText = () => Info.DisplayName;
            OnSetText = v => Info.DisplayName = v;
        }
    }
}
