using Microsoft.Xna.Framework;

namespace tContentPatch
{
    public abstract class ModRemadeChatMonitor
    {
        /// <summary>
        /// <see cref="Mod.Loaded"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        public virtual void DrawChatPostfix(bool drawingPlayerChat) { }
        public virtual void AddNewMessagePrefix(ref string text, Color color, int widthLimitInPixels = -1) { }
    }
}
