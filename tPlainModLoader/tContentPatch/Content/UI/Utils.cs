using System;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary/>
    public static class Utils
    {
        /// <summary>
        /// ui的值
        /// </summary>
        public class UIVal<T>
        {
            /// <summary>
            /// ui的值
            /// </summary>
            public T Val;
            /// <summary>
            /// 在<see cref="Val">Val</see>变更时调用
            /// </summary>
            public Action<T> OnValChanged = null;
            /// <summary>
            /// ui设置值时调用, 为<see langword="null"/>不调用
            /// </summary>
            public Action<T> OnSetVal = null;
            /// <summary>
            /// ui更新时从这获取值, 为<see langword="null"/>不更新
            /// </summary>
            public Func<T> OnGetVal = null;
            /// <summary>
            /// 更新ui
            /// </summary>
            public Action OnUpdateUI = null;

            /// <summary/>
            public UIVal(Action OnUpdateUI = null)
            {
                this.OnUpdateUI = OnUpdateUI;
            }

            /// <summary>
            /// ui更新, 用于<see cref="UIElement.Update(Microsoft.Xna.Framework.GameTime)"/><para/>
            /// <see cref="Val">Val</see>更新为<see cref="OnGetVal">OnGetVal</see>得到的值<br/>
            /// 如果值不同则调用<see cref="OnUpdateUI">OnUpdateUI</see>
            /// </summary>
            /// <param name="compelUpdateUI">强制更新ui</param>
            public void UpdateUI(bool compelUpdateUI = false)
            {
                bool hasUpdate = compelUpdateUI;

                if (OnGetVal != null)
                {
                    T val = OnGetVal();
                    if (Equals(val, Val) != true)
                    {
                        Val = val;
                        hasUpdate = true;

                        OnValChanged?.Invoke(Val);
                    }
                }

                if (hasUpdate) OnUpdateUI?.Invoke();
            }

            /// <summary>
            /// 设置ui的值<para/>
            /// 调用<see cref="OnSetVal">OnSetVal</see><br/>
            /// <see cref="Val">Val</see>更新为<paramref name="val"/><br/>
            /// 如果值不同则调用<see cref="OnUpdateUI">OnUpdateUI</see>
            /// </summary>
            /// <param name="val"></param>
            /// <param name="compelUpdateUI">强制更新ui</param>
            public void SetUIVal(T val, bool compelUpdateUI = false)
            {
                bool hasUpdate = compelUpdateUI;

                if (Equals(val, Val) != true)
                {
                    OnSetVal?.Invoke(val);
                    Val = val;
                    hasUpdate = true;

                    OnValChanged?.Invoke(Val);
                }

                if (hasUpdate) OnUpdateUI?.Invoke();
            }

            /// <summary/>
            public bool Equals(T v1, T v2)
            {
                if (v1 == null && v2 == null) return true;
                if (v1?.Equals(v2) == true) return true;
                return false;
            }
        }

        /// <summary>
        /// 将容器的大小设为子集的最大占位
        /// </summary>
        public static void UpdateContainer_Height(this UIElement uie)
        {
            float max = 0;

            foreach (UIElement element in uie.Children)
            {
                float v = element.Top.GetValue(uie.GetInnerDimensions().Height);
                float size = Math.Max(element.GetOuterDimensions().Height, element.Height.Pixels + element.MarginTop + element.MarginBottom);

                max = Math.Max(max, v + size);
            }

            uie.Height.Set(max + uie.PaddingTop + uie.PaddingBottom, 0);
            uie.Recalculate();
        }

        /// <summary>
        /// 将容器的大小设为子集的最大占位
        /// </summary>
        public static void UpdateContainer_Width(this UIElement uie)
        {
            float max = 0;

            foreach (UIElement element in uie.Children)
            {
                float v = element.Left.GetValue(uie.GetInnerDimensions().Width);
                float size = Math.Max(element.GetOuterDimensions().Width, element.Width.Pixels + element.MarginLeft + element.MarginRight);

                max = Math.Max(max, v + size);
            }

            uie.Width.Set(max + uie.PaddingLeft + uie.PaddingRight, 0);
            uie.Recalculate();
        }
    }
}
