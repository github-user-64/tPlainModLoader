using Terraria;

namespace tContentPatch
{
    public abstract class ModProjectile
    {
        /// <summary>
        /// <see cref="Mod.Loaded"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// <see cref="Projectile.Update(int)"/>前调用
        /// </summary>
        /// <param name="This"></param>
        /// <param name="i"></param>
        public virtual void UpdatePrefix(Projectile This, int i) { }
        /// <summary>
        /// <see cref="Projectile.Update(int)"/>后调用
        /// </summary>
        /// <param name="This"></param>
        /// <param name="i"></param>
        public virtual void UpdatePostfix(Projectile This, int i) { }
        /// <summary>
        /// <see cref="Projectile.Kill"/>前调用
        /// </summary>
        /// <param name="This"></param>
        public virtual void KillPrefix(Projectile This) { }
    }
}
