using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace DeadEffect.Audio
{
    internal class MySoundStyle : SoundStyle
    {
        public override bool IsTrackable => true;
        private readonly int _MaxTrackedInstances = 0;
        public override int MaxTrackedInstances => _MaxTrackedInstances;
        public SoundEffect se = null;

        public MySoundStyle(SoundEffect se, int maxTrackedInstances)
        {
            this.se = se;
            _MaxTrackedInstances = maxTrackedInstances;
        }

        public override SoundEffect GetRandomSound() => se;
    }
}
