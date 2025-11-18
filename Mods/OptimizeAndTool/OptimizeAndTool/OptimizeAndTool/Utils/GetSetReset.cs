using System;

namespace OptimizeAndTool.Utils
{
    public class GetSetReset<T>
    {
        public Action<T> OnValUpdate = null;
        private T _val;
        public T val
        {
            get => _val;
            set
            {
                if (_val == null && value == null) return;
                if (_val?.Equals(value) == true) return;
                _val = func == null ? value : func(value);
                OnValUpdate?.Invoke(_val);
            }
        }
        private T reset;
        private Func<T, T> func;

        public GetSetReset(T val = default, T reset = default, Func<T, T> func = null)
        {
            this.val = val;
            this.reset = reset;
            this.func = func;
        }

        public void Reset() => val = reset;
    }

    public class GetSetReset
    {
        public static Func<int, int> GetIntFunc(int min = int.MinValue, int max = int.MaxValue)
        {
            return v =>
            {
                if (v < min) return min;
                if (v > max) return max;
                return v;
            };

        }
    }
}
