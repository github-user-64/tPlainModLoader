using Microsoft.Xna.Framework;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchRemadeChatMonitor
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary/>
        public virtual void DrawChatPrefix(bool drawingPlayerChat) { }
        /// <summary/>
        public virtual void DrawChatPostfix(bool drawingPlayerChat) { }
        /// <summary/>
        public virtual void AddNewMessagePrefix(ref string text, Color color, int widthLimitInPixels = -1) { }
    }
}
