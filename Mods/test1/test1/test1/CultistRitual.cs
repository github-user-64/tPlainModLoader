using tContentPatch;
using Terraria;

namespace test1
{
    internal class CultistRitual : ModPlayer
    {
        public override void UpdatePrefix(Player This, int playerI)
        {
            Main.time = 20000;

            //NPC n = Main.npc.FirstOrDefault(i => i.active && i.type == 439);
            //if (n == null)
            //{
            //    //n = Main.npc[NPC.NewNPC(null, (int)This.position.X, (int)This.position.Y, 439)];
            //}
            ////n.position = This.position - new Vector2(400, 0);
            ////n.ai[0] = 5;
            ////n.ai[1] = 31;

            //NPC n = Main.npc.FirstOrDefault(i => i.active && i.type == 459);
            //if (n != null) Main.NewText($"{n.ai[0]}, {n.ai[1]}, {n.ai[2]}, {n.ai[3]}");
        }
    }

    //public class drawN : ModMain
    //{
    //    public override void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers)
    //    {
    //        int index = gameInterfaceLayers.FindIndex(i => i.Name == "Vanilla: Inventory");
    //        if (index == -1) return;
    //        gameInterfaceLayers.Insert(index, new LegacyGameInterfaceLayer("666", () =>
    //        {
    //            foreach (NPC n in Main.npc)
    //            {
    //                if (n.active == false) continue;

    //                string text = $"{n.whoAmI}\n[{n.ai[0]}]\n[{n.ai[1]}]\n[{n.ai[2]}]\n[{n.ai[3]}]";

    //                Utils.DrawBorderString(Main.spriteBatch, text,
    //                    new Vector2(n.position.X - Main.screenPosition.X, n.position.Y + n.height - Main.screenPosition.Y), Color.Red);
    //            }

    //            return true;
    //        }, InterfaceScaleType.Game));
    //    }
    //}
}
