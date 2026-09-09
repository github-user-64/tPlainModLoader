using HarmonyLib;
using System.Collections.Generic;
using tContentPatch.Content.Network;
using Terraria;
using Terraria.Localization;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(NetMessage))]
    internal class Patch_NetMessage : ListCopy<PatchNetMessage>
    {
        private static List<PatchNetMessage> mod = new List<PatchNetMessage>();

        public Patch_NetMessage() : base(mod) { }

        [HarmonyPatch("SendData")]
        [HarmonyPrefix]
        public static void SendDataPrefix(int msgType, int remoteClient, int ignoreClient, NetworkText text,
            int number, float number2, float number3, float number4, int number5, int number6, int number7)
        {
            mod.ForTry(item => item.SendDataPrefix(msgType, remoteClient, ignoreClient, text,
                number, number2, number3, number4, number5, number6, number7));
        }

        [HarmonyPatch("SendData")]
        [HarmonyPostfix]
        public static void SendDataPostfix(int msgType, int remoteClient, int ignoreClient, NetworkText text,
            int number, float number2, float number3, float number4, int number5, int number6, int number7)
        {
            mod.ForTry(item => item.SendDataPostfix(msgType, remoteClient, ignoreClient, text,
                number, number2, number3, number4, number5, number6, number7));

            if (msgType == Terraria.ID.MessageID.PlayerSpawn && Main.netMode == 1 && ContentPatch.NoPublic) NetTPMLModule.SendToServer();
        }

        [HarmonyPatch("SyncConnectedPlayer")]
        [HarmonyPrefix]
        public static void SyncConnectedPlayerPrefix(int plr)
        {
            if (Main.dedServ == false) return;

            mod.ForTry(item => item.SyncConnectedPlayerPrefix(plr));
        }

        [HarmonyPatch("SyncConnectedPlayer")]
        [HarmonyPostfix]
        public static void SyncConnectedPlayerPostfix(int plr)
        {
            if (Main.dedServ == false) return;

            mod.ForTry(item => item.SyncConnectedPlayerPostfix(plr));
        }

        /// <summary>
        /// 建议使用NetMessage.SyncOnePlayer
        /// <para/><see cref="NetMessage.SyncDisconnectedPlayer(int)"/>的修补有可能失效
        /// <para/>在服务端启动后才修补时这个就没效果了
        /// <para/>服务端是运行在另一个线程上的<see cref="Netplay.StartServer"/>
        /// <para/>这个方法就是由另一个线程调用的, 所以可能是因为这个原因导致的
        /// <para/>所以最好在有线程启动前修补
        /// </summary>
        [HarmonyPatch("SyncDisconnectedPlayer")]
        [HarmonyPrefix]
        public static void SyncDisconnectedPlayerPrefix(int plr)
        {
            if (Main.dedServ == false) return;

            mod.ForTry(item => item.SyncDisconnectedPlayerPrefix(plr));
        }

        /// <summary>
        /// 和<see cref="SyncDisconnectedPlayerPrefix(int)"/>一样的问题
        /// </summary>
        [HarmonyPatch("SyncDisconnectedPlayer")]
        [HarmonyPostfix]
        public static void SyncDisconnectedPlayerPostfix(int plr)
        {
            if (Main.dedServ == false) return;

            mod.ForTry(item => item.SyncDisconnectedPlayerPostfix(plr));
        }

        [HarmonyPatch("SyncOnePlayer")]
        [HarmonyPrefix]
        public static void SyncOnePlayerPrefix(int plr, int toWho, int fromWho)
        {
            if (Main.dedServ == false) return;

            mod.ForTry(item => item.SyncOnePlayerPrefix(plr, toWho, fromWho));
        }

        [HarmonyPatch("SyncOnePlayer")]
        [HarmonyPostfix]
        public static void SyncOnePlayerPostfix(int plr, int toWho, int fromWho)
        {
            if (Main.dedServ == false) return;

            mod.ForTry(item => item.SyncOnePlayerPostfix(plr, toWho, fromWho));
        }
    }
}
