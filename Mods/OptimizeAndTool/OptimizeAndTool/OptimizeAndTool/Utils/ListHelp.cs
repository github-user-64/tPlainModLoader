using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace OptimizeAndTool.Utils
{
    public static class ListHelp
    {
        /// <summary>
        /// 将<paramref name="item"/>添加到<paramref name="index"/>处<br/>
        /// <paramref name="index"/>会被限制在范围内
        /// </summary>
        public static int Add<T>(this List<T> list, T item, int index)
        {
            index = (int)MathHelper.Clamp(index, 0, list.Count);

            if (index == list.Count) list.Add(item);
            else list.Insert(index, item);

            return index;
        }

        /// <summary>
        /// <paramref name="index1"/>和<paramref name="index2"/>的项交换位置<br/>
        /// 超出则不处理
        /// </summary>
        public static void MoveItem<T>(this List<T> list, int index1, int index2)
        {
            if (list.IndexInRange(index1) != true) return;
            if (list.IndexInRange(index2) != true) return;

            T item = list[index1];

            list[index1] = list[index2];
            list[index2] = item;
        }

        /// <summary>
        /// <paramref name="item"/>和<paramref name="index"/>处的项交换位置<br/>
        /// 超出则不处理
        /// </summary>
        public static void MoveItem<T>(this List<T> list, T item, int index)
        {
            list.MoveItem(list.IndexOf(item), index);
        }

        /// <summary>
        /// <c>index1</c>和<c>index2</c>处的项交换位置<br/>
        /// 超出则不处理<br/>
        /// <c>index1</c>是:<paramref name="item"/><br/>
        /// <c>index2</c>是:<c>index1</c>加<paramref name="off"/>
        /// </summary>
        public static void OffItem<T>(this List<T> list, T item, int off)
        {
            int index1 = list.IndexOf(item);

            list.MoveItem(index1, index1 + off);
        }

        /// <summary>
        /// 超出则不处理
        /// </summary>
        public static void OffUp<T>(this List<T> list, T item) => list.OffItem(item, -1);
        /// <summary>
        /// 超出则不处理
        /// </summary>
        public static void OffDown<T>(this List<T> list, T item) => list.OffItem(item, 1);
    }
}
