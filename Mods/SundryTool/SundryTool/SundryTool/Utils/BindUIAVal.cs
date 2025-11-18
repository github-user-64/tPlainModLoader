using System;

namespace SundryTool.Utils
{
    internal interface IBindUIAVal<T>
    {
        /// <summary>
        /// UI值更新时, 用于更新绑定的值
        /// </summary>
        event Action<T> OnUIUpdate;
        void SetUIVal(T v);
    }
    internal interface IBindUIAVal<T, T2> : IBindUIAVal<T>
    {
        /// <summary>
        /// UI值更新时, 用于更新绑定的值
        /// </summary>
        event Action<T2> OnUIUpdate2;
        void SetUIVal2(T2 v);
    }

    internal static class BindUIAVal
    {
        public static void Bind<T>(GetSetReset<T> gsr, IBindUIAVal<T> uv)
        {
            uv.SetUIVal(gsr.val);

            uv.OnUIUpdate += v => gsr.val = v;
            gsr.OnValUpdate += v => uv.SetUIVal(v);
        }

        public static void Bind<T, T2>(GetSetReset<T> gsr, GetSetReset<T2> gsr2, IBindUIAVal<T, T2> uv)
        {
            Bind(gsr, uv);

            uv.SetUIVal2(gsr2.val);

            uv.OnUIUpdate2 += v => gsr2.val = v;
            gsr2.OnValUpdate += v => uv.SetUIVal2(v);
        }
    }
}
