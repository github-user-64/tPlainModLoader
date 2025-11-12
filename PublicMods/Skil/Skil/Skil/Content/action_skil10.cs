using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;

namespace Skil.Content
{
    public class skil10 : ModPlayer
    {
        //技能10, 一堆物品聚合在一起爆炸
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get3("skil10", Enable),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get2(Enable, ico:"Images/Buff_105", text:"技能10")
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_skil10(This);
            else a1_skil10_false(This);
        }

        private static int skil10_state = 0;
        private static int skil10_state1_count = 0;
        private static int skil10_state2_count = 0;
        private static int skil10_state3_count = 0;
        private static Vector2 skil10_position;

        public static void a1_skil10(Player player)
        {
            if (player == null || player.active == false || player.dead == true)
            {
                skil10_state = 0;
                return;
            }

            if (skil10_state == 0)
            {
                if (Main.mouseLeft == false || player.mouseInterface == true) return;

                skil10_position = Main.MouseWorld;
                skil10_state1_count = 0;
                skil10_state = 1;
            }
            else
            if (skil10_state == 1)
            {
                if (Main.GameUpdateCount % 4 != 0) return;

                ParticleOrchestrator.BroadcastOrRequestParticleSpawn((ParticleOrchestraType)24, new ParticleOrchestraSettings
                {
                    PositionInWorld = skil10_position,
                });

                Vector2 p = skil10_position;
                Vector2 v = Vector2.UnitX;
                v = v.RotatedBy(Utils.getRand(0, (int)MathHelper.TwoPi) + Utils.getRandFloat());
                v *= Utils.getRand(50, 333);

                p += v;

                Item item = new Item();
                item.SetDefaults(Utils.getRand(1, Terraria.ID.ItemID.Count));

                Chest.VisualizeChestTransfer(p, skil10_position, item, 0);

                if (++skil10_state1_count > 30)
                {
                    skil10_state2_count = 0;
                    skil10_state = 2;
                }
            }
            else
            if (skil10_state == 2)
            {
                if (Main.GameUpdateCount % 5 != 0) return;

                Vector2 p = skil10_position;
                p.X += Utils.getRand(-20, 20);
                p.Y += Utils.getRand(-20, 20);

                float scale = 1 + Utils.getRandFloat();

                _ = Projectile.NewProjectile(null, p, Vector2.Zero, 953, SkilListControl1.damage.val, 1, ai1: scale);

                if (++skil10_state2_count > 6)
                {
                    skil10_state3_count = Utils.getRand(10, 30);
                    skil10_state = 3;
                }
            }
            else
            if (skil10_state == 3)
            {
                Vector2 v = -Vector2.UnitY * Utils.getRand(4, 9);
                v = v.RotatedBy(Utils.getRand(-45, 45) * MathHelper.TwoPi / 360);

                _ = Projectile.NewProjectile(null, skil10_position, v, 968, SkilListControl1.damage.val, 1, ai1: Utils.getRand(0, 24));

                if (--skil10_state3_count < 1) skil10_state = 0;
            }
        }

        public static void a1_skil10_false(Player player)
        {
            if (player == null) return;

            skil10_state = 0;
        }
    }
}
