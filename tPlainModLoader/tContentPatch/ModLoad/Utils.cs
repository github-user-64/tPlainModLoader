using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace tContentPatch.ModLoad
{
    internal static class Utils
    {
        internal static List<targetType> CreateInstance<targetType>(Assembly assembly)
        {
            //Type[] type = assembly.GetExportedTypes();
            Type[] type = assembly.GetTypes();

            List<Type> types = type.ToList().FindAll(
                t => t.IsClass && t.IsAbstract == false &&
                typeof(targetType).IsAssignableFrom(t));

            if (types.Count < 1) return null;

            List<targetType> instance = new List<targetType>();

            foreach (Type t in types)
            {
                targetType obj = (targetType)Activator.CreateInstance(t);
                instance.Add(obj);
            }

            return instance;
        }

        internal static void ForHelp<T>(List<T> list, Action<T> item, Func<Exception, string> mess)
        {
            if (list == null || item == null) return;

            for (int i = 0; i < list.Count; ++i)
            {
                try
                {
                    item.Invoke(list[i]);
                }
                catch (Exception ex)
                {
                    throw new Exception(mess?.Invoke(ex), ex);
                }
            }
        }
    }
}
