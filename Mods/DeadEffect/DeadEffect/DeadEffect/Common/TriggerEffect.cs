using tContentPatch;
using Terraria;
using Terraria.DataStructures;

namespace DeadEffect.Common
{
    public class TriggerEffect : PatchPlayer
    {
        public delegate void TriggerHurtEventHandler(bool content, bool sound, Player This, double result, PlayerDeathReason damageSource,
            int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter, bool dodgeable);
        public delegate void TriggerDeadEventHandler(bool content, bool sound, Player This, PlayerDeathReason damageSource,
            double dmg, int hitDirection, bool pvp);

        public static event TriggerHurtEventHandler TriggerHurt = null;
        public static event TriggerDeadEventHandler TriggerDead = null;

        public override void HurtPostfix(Player This, ref double result, PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter, bool dodgeable)
        {
            if (This.dead == true) return;
            if (Config.Data?.EnableHurtEffect != true) return;
            if (This.active != true) return;
            if (Main.dedServ == true) return;

            double rv = result;

            TriggerHurt?.Invoke(Config.Data?.EffectContent == true, Config.Data?.EffectSound == true,
                This, rv, damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter, dodgeable);
        }

        public override void KillMePostfix(Player This, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp)
        {
            if (This.dead != true) return;
            if (Config.Data?.EnableDeadEffect != true) return;
            if (This.active != true) return;
            if (Main.dedServ == true) return;

            TriggerDead?.Invoke(Config.Data?.EffectContent == true, Config.Data?.EffectSound == true,
                This, damageSource, dmg, hitDirection, pvp);
        }
    }
}
