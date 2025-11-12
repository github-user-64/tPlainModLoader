using Terraria;

namespace tContentPatch
{
    public abstract class ModNPC
    {
        /// <summary>
        /// <see cref="Mod.Loaded"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// <see cref="NPC.UpdateNPC(int)"/>前调用
        /// </summary>
        /// <param name="This"></param>
        /// <param name="i"></param>
        public virtual void UpdateNPCPrefix(NPC This, int i) { }
        /// <summary>
        /// <see cref="NPC.UpdateNPC(int)"/>后调用
        /// </summary>
        /// <param name="This"></param>
        /// <param name="i"></param>
        public virtual void UpdateNPCPostfix(NPC This, int i) { }
    }
}
