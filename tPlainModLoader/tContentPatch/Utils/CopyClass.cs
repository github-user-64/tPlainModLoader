using System;
using System.Reflection;

namespace tContentPatch.Utils
{
    /// <summary>
    /// 复制类
    /// </summary>
    public static class CopyClass
    {
        /// <summary>
        /// 复制类中的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T CopyField<T>(T obj, params object[] args)
        {
            if (obj == null) return default;

            FieldInfo[] fis = typeof(T).GetFields();

            T robj = (T)Activator.CreateInstance(typeof(T), args);

            foreach (FieldInfo fi in fis)
            {
                fi.SetValue(robj, fi.GetValue(obj));
            }

            return robj;
        }
    }
}
