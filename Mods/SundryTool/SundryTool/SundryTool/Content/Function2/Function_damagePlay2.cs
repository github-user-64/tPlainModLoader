using CommandHelp;
using Microsoft.Xna.Framework.Graphics;
using SundryTool.Content.UI;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using tContentPatch;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_damagePlay2 : PatchMain
    {
        public static GetSetReset<int> damagePlay_damage = new GetSetReset<int>(777, 777);
        public static GetSetReset<int> damagePlay_play = new GetSetReset<int>(-1, -1);
        public static GetSetReset<int> reason_play = new GetSetReset<int>(0, Main.player.Length - 1, GetSetReset.GetIntFunc(0, Main.player.Length - 1));
        public static GetSetReset<int> reason_proj = new GetSetReset<int>(0, 0, GetSetReset.GetIntFunc(0, ProjectileID.Count - 1));
        public static GetSetReset<int> reason_npc = new GetSetReset<int>(0, 0);
        public static GetSetReset<int> reason_other = new GetSetReset<int>(0, 0);
        public static GetSetReset<string> reason_custom = new GetSetReset<string>(null, null);

        public override void Initialize()
        {
            damagePlay_play.OnValUpdate += v => Utils.OutputPlayerName(damagePlay_play.val);
            reason_play.OnValUpdate += v => Utils.OutputPlayerName(reason_play.val);
            reason_proj.OnValUpdate += v =>
            {
                string s = $"[{Lang.GetProjectileName(v).Value}]";
                Console.WriteLine(s);
                Main.NewText(s);
            };
            reason_npc.OnValUpdate += v => Utils.OutputNPCName(reason_npc.val);
        }

        public static void damage(int by)
        {
            if (damagePlay_play.val >= Main.player.Length) return;

            PlayerDeathReason reason = null;
            switch (by)
            {
                case 0: reason = PlayerDeathReason.ByPlayer(reason_play.val); break;
                case 1: reason = ByProjectile(reason_play.val, reason_proj.val); break;
                case 2: reason = PlayerDeathReason.ByNPC(reason_npc.val); break;
                case 3: reason = PlayerDeathReason.ByOther(reason_other.val); break;
                case 4: reason = PlayerDeathReason.ByCustomReason(reason_custom.val); break;
                default: reason = PlayerDeathReason.ByPlayer(255); break;
            }

            if (damagePlay_play.val == -1)
            {
                for (int i = 0; i < 255; ++i)
                {
                    NetMessage.SendPlayerHurt(i, reason, damagePlay_damage.val, -1, true, true, -1, -1, -1);
                }
            }
            else
            {
                if (damagePlay_play.val < 0) return;

                NetMessage.SendPlayerHurt(damagePlay_play.val, reason, damagePlay_damage.val, -1, true, true, -1, -1, -1);
            }
        }

        public static List<CommandObject> GetCO()
        {
            CommandObject co = new CommandObject("damagePlay2");
            co.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(co.SubCommand));

            CommandMethod damage = new CommandMethod("invoke");
            damage.Runing += v => Function_damagePlay2.damage(-1);
            co.SubCommand.Add(damage);

            co.SubCommand.Add(new CommandHRA<int>("play", damagePlay_play, new CommandInt()));
            co.SubCommand.Add(new CommandHRA<int>("damage", damagePlay_damage, new CommandInt()));

            List<CommandObject> cos = new List<CommandObject>
            {
                co,
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>();

            string ico = "Images/Buff_30";
            Texture2D texture = Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            UIDrawer a = new UIDrawer(ico, "伤害玩家2");
            a.Add(new UIItemTextBoxBind<int>(damagePlay_damage, int.Parse, texture, "伤害") { MouseText = "<int>" });
            a.Add(new UIItemTextBoxBind<int>(damagePlay_play, int.Parse, texture, "玩家id") { MouseText = "<int>" });
            a.Add(new UIItemTextBoxBind<int>(reason_play, int.Parse, texture, "来源玩家") { MouseText = "玩家id<int>" });
            a.Add(new UIItemTextBoxBind<int>(reason_proj, int.Parse, texture, "来源射弹") { MouseText = "射弹type<int>" });
            a.Add(new UIItemTextBoxBind<int>(reason_npc, int.Parse, texture, "来源npc") { MouseText = "npc id<int>" });
            a.Add(new UIItemTextBoxBind<int>(reason_other, int.Parse, texture, "来源其它") { MouseText = "<int>" });
            a.Add(new UIItemTextBoxBind<string>(reason_custom, v => v, texture, "来源自定义") { MouseText = "<string>" });
            a.Add(UIBuild.get4("造成来自玩家伤害", () => damage(0), "来源(玩家)", ico));
            a.Add(UIBuild.get4("造成来自射弹伤害", () => damage(1), "来源(玩家, 射弹)", ico));
            a.Add(UIBuild.get4("造成来自npc伤害", () => damage(2), "来源(npc)", ico));
            a.Add(UIBuild.get4("造成来自其它伤害", () => damage(3), "来源(其它)", ico));
            a.Add(UIBuild.get4("造成来自自定义伤害", () => damage(4), "来源(自定义)", ico));

            uis.Add(a);

            return uis;
        }

        public static PlayerDeathReason ByProjectile(int playerIndex, int projectileType)
        {
            Type type = typeof(PlayerDeathReason);
            FieldInfo _sourcePlayerIndex = type.GetField("_sourcePlayerIndex", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo _sourceProjectileLocalIndex = type.GetField("_sourceProjectileLocalIndex", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo _sourceProjectileType = type.GetField("_sourceProjectileType", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo _sourceItemType = type.GetField("_sourceItemType", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo _sourceItemPrefix = type.GetField("_sourceItemPrefix", BindingFlags.NonPublic | BindingFlags.Instance);

            PlayerDeathReason playerDeathReason = new PlayerDeathReason();
            _sourcePlayerIndex.SetValue(playerDeathReason, playerIndex);
            _sourceProjectileLocalIndex.SetValue(playerDeathReason, 0);
            _sourceProjectileType.SetValue(playerDeathReason, projectileType);

            if (playerIndex >= 0 && playerIndex <= 255)
            {
                _sourceItemType.SetValue(playerDeathReason, Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].type);
                _sourceItemPrefix.SetValue(playerDeathReason, Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].prefix);
            }

            return playerDeathReason;
        }
    }
}
