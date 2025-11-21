using System;
using System.Linq;
using System.Reflection;
using tContentPatch;
using tContentPatch.Patch;
using Terraria.DataStructures;

namespace SuspiciousPlayer.Patch
{
    internal class PatchItem : Mod
    {
        public delegate void EventNewItem(int x, int y, int type);
        public static EventNewItem OnNewItem = null;

        public override void AddPatch(IAddPatch addPatch)
        {
            MethodInfo patch = typeof(PatchItem).GetMethod("NewItem");
            Type[] ts = patch.GetParameters().ToList().ConvertAll(i => i.ParameterType).ToArray();

            var a = typeof(Terraria.Item).GetMethod("NewItem", ts);

            addPatch.AddPostfix(typeof(Terraria.Item).GetMethod("NewItem", ts),
                patch);
        }

        public static void NewItem(IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack = 1, bool noBroadcast = false, int pfix = 0, bool noGrabDelay = false, bool reverseLookup = false)
        {
            OnNewItem?.Invoke(X, Y, Type);
        }
    }
}
