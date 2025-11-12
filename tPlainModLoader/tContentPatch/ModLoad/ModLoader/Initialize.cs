using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;

namespace tContentPatch.ModLoad
{
    internal partial class ModLoader
    {
        private void Initialize_Mod(List<ModObject> mods)
        {
            progressV = 0;
            progressMax = 1;
            stateText = "初始化模组";

            Func<ModObject, Exception, string> exMess = (m, ex) => $"初始化模组[{m.info?.name ?? m.config.key}]失败:{ex.Message}";
            Func<ModObject, Exception, string> exMess2 = (m, ex) => $"添加模组[{m.info?.name ?? m.config.key}]的补丁失败:{ex.Message}";

            Action<ModObject>[] action = new Action<ModObject>[] {
                mo =>
                {
                    stateText = $"初始化模组:{mo.info?.name ?? mo.config.key}";

                    Utils.ForHelp(mo.inheritance_mod, item => item.Load(), ex => exMess(mo, ex));
                },
                mo =>
                {
                    stateText = $"初始化模组设置:{mo.info?.name ?? mo.config.key}";

                    Utils.ForHelp(mo.inheritance_setting, item => LoadModSet(mo, item), ex => exMess(mo, ex));
                },
                mo =>
                {
                    stateText = $"初始化模组:{mo.info?.name ?? mo.config.key}";

                    Utils.ForHelp(mo.inheritance_mod, item => item.Loaded(), ex => exMess(mo, ex));
                },
                mo =>
                {
                    stateText = $"初始化模组:{mo.info?.name ?? mo.config.key}";

                    Utils.ForHelp(mo.inheritance_modMain, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_modPlayer, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_modNPC, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_modItem, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_modProjectile, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_tileLightScanner, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_remadeChatMonitor, item => item.Initialize(), ex => exMess(mo, ex));
                },
                mo =>
                {
                    stateText = $"添加模组补丁:{mo.info?.name ?? mo.config.key}";

                    Utils.ForHelp(mo.inheritance_mod, item => item.AddPatch(modPatch), ex => exMess2(mo, ex));

                    Utils.ForHelp(mo.inheritance_modMain, item => ModPatch.Patch_ModMain.mod.Add(item), ex => exMess2(mo, ex));

                    Utils.ForHelp(mo.inheritance_modPlayer, item => ModPatch.Patch_ModPlayer.mod.Add(item), ex => exMess2(mo, ex));
                    
                    Utils.ForHelp(mo.inheritance_modNPC, item => ModPatch.Patch_ModNPC.mod.Add(item), ex => exMess2(mo, ex));

                    Utils.ForHelp(mo.inheritance_modItem, item => ModPatch.Patch_ModItem.mod.Add(item), ex => exMess2(mo, ex));

                    Utils.ForHelp(mo.inheritance_modProjectile, item => ModPatch.Patch_ModProjectile.mod.Add(item), ex => exMess2(mo, ex));

                    Utils.ForHelp(mo.inheritance_tileLightScanner, item => ModPatch.Patch_ModTileLightScanner.mod.Add(item), ex => exMess2(mo, ex));

                    Utils.ForHelp(mo.inheritance_remadeChatMonitor, item => ModPatch.Patch_ModRemadeChatMonitor.mod.Add(item), ex => exMess2(mo, ex));
                }
            };

            progressMax = mods.Count * action.Length;

            foreach (Action<ModObject> i in action)
            {
                CheckLoadCancel();

                foreach (ModObject mo in mods)
                {
                    i(mo);
                    ++progressV;
                }
            }
        }

        private void Initialize_SetupDrawInterfaceLayers()
        {
            progressV = 0;
            progressMax = 1;
            stateText = "初始化UI";

            FieldInfo fi = typeof(Main).GetField("_needToSetupDrawInterfaceLayers", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(Main.instance, true);

            progressV = 1;
        }
    }
}
