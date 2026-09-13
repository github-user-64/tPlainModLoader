using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchProjectile
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// <see cref="Projectile.Update(int)"/>前调用
        /// </summary>
        public virtual void UpdatePrefix(Projectile This, int i) { }
        /// <summary>
        /// <see cref="Projectile.Update(int)"/>后调用
        /// </summary>
        public virtual void UpdatePostfix(Projectile This, int i) { }
        /// <summary/>
        public virtual void KillPrefix(Projectile This) { }
        /// <summary/>
        public virtual void KillPostfix(Projectile This) { }
        /// <summary/>
        public virtual void SetDefaultsPrefix(Projectile This, int Type) { }
        /// <summary/>
        public virtual void SetDefaultsPostfix(Projectile This, int Type) { }
        /// <summary/>
        public virtual void NewProjectilePostfix(int result, IEntitySource spawnSource,
            float X, float Y, float SpeedX, float SpeedY,
            int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2, NewProjectileModifier modifer)
        { }
        /// <summary/>
        public virtual Color AI_203_GetLightningColor(Projectile This, Color color) => color;
    }
}
