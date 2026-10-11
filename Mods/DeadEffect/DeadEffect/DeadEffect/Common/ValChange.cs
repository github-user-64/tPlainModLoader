using System;

namespace DeadEffect.Common
{
    public class ValChange
    {
        public float MaxVal { get; protected set; }
        public float NowVal { get; protected set; }
        public float MaxChange = 0;
        public float NowChange = 0;
        private bool direction;

        public ValChange(float maxChange, float nowChange)
        {
            MaxChange = maxChange;
            NowChange = nowChange;
        }

        public virtual void Set(float val) => NowVal = MaxVal = val;

        public virtual void Update()
        {
            if (MaxVal <= 0)
            {
                NowVal = MaxVal = 0;
                return;
            }

            if (Math.Abs(NowVal) >= MaxVal)
            {
                direction = NowVal < 0;
                MaxVal -= MaxChange;
                NowVal = direction ? -MaxVal : MaxVal;
            }

            float v = (float)Math.Ceiling(MaxVal * NowChange);

            NowVal += direction ? v : -v;
        }
    }
}
