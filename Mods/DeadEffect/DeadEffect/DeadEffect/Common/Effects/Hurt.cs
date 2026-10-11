using Microsoft.Xna.Framework;
using tContentPatch;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace DeadEffect.Common.Effects
{
    internal class Hurt : PatchPlayer
    {
        protected ValChange[] vs = null;

        public override void Initialize()
        {
            ValChange[] vs = new ValChange[Main.maxPlayers];
            for (int i = 0; i < vs.Length; ++i) vs[i] = new ValChange(10f, 0.2f);
            this.vs = vs;

            TriggerEffect.TriggerHurt += OnHurt;
        }

        protected void OnHurt(bool content, bool sound, Player This, double result, PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter, bool dodgeable)
        {
            if (result <= 0) return;

            int playerI = This.whoAmI;

            if (vs?.IndexInRange(playerI) != true) return;
            ValChange v = vs[playerI];

            if (result > 90)
            {
                if (content) v.Set(160);
                if (sound) SoundEngine.PlayTrackedSound(Resources.Sounds.Long, This.Center);
                return;
            }

            if (content) v.Set(90);
            if (sound) SoundEngine.PlayTrackedSound(Resources.Sounds.Short, This.Center);
        }

        public override void UpdatePostfix(Player This, int playerI)
        {
            if (Main.dedServ == true) return;

            if (vs?.IndexInRange(playerI) != true) return;
            ValChange v = vs[playerI];

            if (v.MaxVal <= 0) return;

            if (This.dead == true || This.active != true) v.Set(0);
            else v.Update();

            if (v.MaxVal <= 0)
            {
                This.fullRotation = 0;
                return;
            }

            This.fullRotation = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 360f * v.NowVal).ToRotation();
            This.fullRotationOrigin = new Vector2(This.width / 2f, This.height);
        }
    }
}
