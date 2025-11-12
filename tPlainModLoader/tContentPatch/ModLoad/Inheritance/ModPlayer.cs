using Terraria;

namespace tContentPatch
{
    public abstract class ModPlayer
    {
        /// <summary>
        /// <see cref="Mod.Loaded"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// <see cref="Player.Update(int)"/>前调用
        /// </summary>
        /// <param name="This"></param>
        /// <param name="playerI"></param>
        public virtual void UpdatePrefix(Player This, int playerI) { }
        /// <summary>
        /// <see cref="Player.Update(int)"/>后调用
        /// </summary>
        /// <param name="This"></param>
        /// <param name="playerI"></param>
        public virtual void UpdatePostfix(Player This, int playerI) { }
        public virtual void UpdateArmorSetsPostfix(Player This, int playerI) { }
    }
}
