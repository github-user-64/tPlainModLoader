using Terraria;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Localization;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchPlayer
    {
        /// <summary>
        /// <see cref="Mod.Loaded"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// <see cref="Player.Update(int)"/>前调用
        /// </summary>
        public virtual void UpdatePrefix(Player This, int playerI) { }
        /// <summary>
        /// <see cref="Player.Update(int)"/>后调用
        /// </summary>
        public virtual void UpdatePostfix(Player This, int playerI) { }
        /// <summary>
        public virtual void UpdateEquipsPrefix(Player This, int playerI) { }
        /// <summary>
        public virtual void UpdateEquipsPostfix(Player This, int playerI) { }
        /// <summary/>
        public virtual void UpdateArmorSetsPostfix(Player This, int playerI) { }
        /// <summary>
        /// 保存玩家数据前, 单人和客户端有效
        /// </summary>
        public virtual void SavePlayerPrefix(PlayerFileData playerFile, bool skipMapSave) { }
        /// <summary>
        /// 保存玩家数据后, 单人和客户端有效
        /// </summary>
        public virtual void SavePlayerPostfix(PlayerFileData playerFile, bool skipMapSave) { }
        /// <summary>
        /// 能否掉落墓碑
        /// </summary>
        public virtual bool CanDropTombstone(Player This, long coinsOwned, NetworkText deathText, int hitDirection) => true;
        /// <summary>
        /// 被击中前
        /// </summary>
        public virtual void HurtPrefix(Player This, PlayerDeathReason damageSource,
            ref int Damage, ref int hitDirection, ref bool pvp, ref bool quiet, ref bool Crit, ref int cooldownCounter, ref bool dodgeable)
        { }
        /// <summary>
        /// 被击中后
        /// </summary>
        public virtual void HurtPostfix(Player This, ref double result, PlayerDeathReason damageSource,
            int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter, bool dodgeable)
        { }
        /// <summary>
        /// 如果返回<see langword="false"/>那么以下方法不会被调用:
        /// <para/>原版<see cref="Player.KillMe(PlayerDeathReason, double, int, bool)"/>
        /// <para/>不影响:
        /// <para/><see cref="KillMePrefix(Player, PlayerDeathReason, double, int, bool)"/>
        /// <para/><see cref="KillMePostfix(Player, PlayerDeathReason, double, int, bool)"/>
        /// </summary>
        public virtual bool KillMePrefix(Player This, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp) => true;
        /// <summary/>
        public virtual void KillMePostfix(Player This, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp) { }
    }
}
