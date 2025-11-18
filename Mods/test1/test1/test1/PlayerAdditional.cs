using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch;
using Terraria;

namespace test1
{
    internal class PlayerAdditional : Mod
    {
        public class Data
        {
            public List<Type> type = new List<Type>();
            public List<object> obj = new List<object>();
        }

        private static Data[] data = null;

        public override void Load()
        {
            data = new Data[Main.player.Length];
            for (int i = 0; i < data.Length; i++) data[i] = new Data();
        }

        public static T GetData<T>(int index)
        {
            Data data = PlayerAdditional.data[index];

            int typeI = data.type.IndexOf(typeof(T));
            if (typeI == -1) return default;

            try
            {
                return (T)data.obj[typeI];
            }
            catch
            {
                return default;
            }
        }

        public static T GetData<T>(Player player) => GetData<T>(player.whoAmI);

        public static void SetData<T>(int index, T obj)
        {
            Data data = PlayerAdditional.data[index];

            int typeI = data.type.IndexOf(typeof(T));
            if (typeI == -1)
            {
                data.type.Add(typeof(T));
                data.obj.Add(obj);
            }
            else
            {
                data.obj[typeI] = obj;
            }
        }
    }
}
