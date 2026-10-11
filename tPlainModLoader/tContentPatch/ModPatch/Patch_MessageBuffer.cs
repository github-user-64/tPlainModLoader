using HarmonyLib;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(MessageBuffer))]
    internal class Patch_MessageBuffer : ListCopy<PatchMessageBuffer>
    {
        private static List<PatchMessageBuffer> mod = new List<PatchMessageBuffer>();

        public Patch_MessageBuffer() : base(mod) { }

        [HarmonyPatch("GetData")]
        [HarmonyPrefix]
        public static bool GetDataPrefix(MessageBuffer __instance, int start, int length, int messageType)
        {
            messageType = __instance.readBuffer[start];

            if (__instance.reader == null) __instance.ResetReader();

            //

            __instance.reader.BaseStream.Position = start + 1;
            GetDataPrefix_14(__instance, start, length, messageType);

            //

            bool ok = true;
            mod.ForTry(item =>
            {
                __instance.reader.BaseStream.Position = start + 1;

                ok &= item.CanGetData(__instance, start, length, messageType);
            });

            //

            mod.ForTry(item =>
            {
                __instance.reader.BaseStream.Position = start + 1;

                item.GetDataPrefix(__instance, start, length, messageType);
            });

            __instance.reader.BaseStream.Position = start + 1;

            //

            if (ok == false)
            {
                if (__instance.whoAmI < Netplay.MaxConnections)
                {
                    Netplay.Clients[__instance.whoAmI].TimeOutTimer = 0;
                }
                else
                {
                    Netplay.Connection.TimeOutTimer = 0;
                }
                return false;
            }

            return true;
        }

        private static void GetDataPrefix_14(MessageBuffer __instance, int start, int length, int messageType)
        {
            if (messageType != MessageID.PlayerActive) return;
            if (Main.netMode != 1) return;

            int whoAmI = __instance.reader.ReadByte();
            if (Main.player?.IndexInRange(whoAmI) != true) return;

            bool activeNew = __instance.reader.ReadByte() == 1;

            if (activeNew == Main.player[whoAmI].active) return;

            if (activeNew)
            {
                mod.ForTry(item => item.OnPlayerConnect(whoAmI));
            }
            else
            {
                mod.ForTry(item => item.OnPlayerDisconnect(whoAmI));
            }
        }

        [HarmonyPatch("GetData")]
        [HarmonyPostfix]
        public static void GetDataPostfix(MessageBuffer __instance, int start, int length, int messageType)
        {
            messageType = __instance.readBuffer[start];

            mod.ForTry(item =>
            {
                __instance.reader.BaseStream.Position = start + 1;

                item.GetDataPostfix(__instance, start, length, messageType);
            });

            __instance.reader.BaseStream.Position = start + 1;
        }
    }
}
