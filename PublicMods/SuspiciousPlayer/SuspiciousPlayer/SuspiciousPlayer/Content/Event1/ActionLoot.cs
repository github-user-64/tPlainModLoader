using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 战利品
    /// </summary>
    internal class ActionLoot : ModMain
    {
        public static ActionState state = new ActionState(run);
        private static List<(int, int)> item = null;

        public static void run()
        {
            Func<int> getR = () => Utils.getRand(16, 32);

            List<(int, int)> item = new List<(int, int)>()
            {
                (ItemID.FragmentVortex, getR()),
                (ItemID.FragmentNebula, getR()),
                (ItemID.FragmentSolar, getR()),
                (ItemID.FragmentStardust, getR()),
            };

            if (NPC.downedMoonlord)
            {
                item.Add((ItemID.LunarOre, getR()));
                item.Add((ItemID.LunarOre, getR()));
                item.Add((ItemID.LunarOre, getR()));
                item.Add((ItemID.LunarOre, getR()));

                item.Add((ItemID.Meowmere, getR()));
                item.Add((ItemID.StarWrath, getR()));
                item.Add((ItemID.SDMG, getR()));
                item.Add((ItemID.Celeb2, getR()));
                item.Add((ItemID.LastPrism, getR()));
                item.Add((ItemID.LunarFlareBook, getR()));
            }

            ActionLoot.item = new List<(int, int)>();
            for (int i = item.Count; i > 0; --i)
            {
                int index = Utils.getRand(0, item.Count);

                ActionLoot.item.Add(item[index]);
                item.RemoveAt(index);
            }
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            if (Main.GameUpdateCount % 4 == 0)
            {
                ParticleOrchestrator.BroadcastOrRequestParticleSpawn((ParticleOrchestraType)24, new ParticleOrchestraSettings
                {
                    PositionInWorld = ActionLunarTowerDead.pos,
                });
            }

            if (Main.GameUpdateCount % 30 == 0)
            {
                Vector2 v = (-Vector2.UnitY).RotatedBy(Utils.getRand(-10, 10) / 10);
                v *= 16;

                Item.NewItem(null, ActionLunarTowerDead.pos, v,
                    item.First().Item1, item.First().Item2);

                item.RemoveAt(0);

                if (item.Count < 1) Event.SetEventState(Event.EventState_None);
            }
        }
    }
}
