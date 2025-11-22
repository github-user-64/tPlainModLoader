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

                    Utils.ForHelp(mo.inheritance_patchMain, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchPlayer, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchNPC, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchItem, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchProjectile, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchTileLightScanner, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchRemadeChatMonitor, item => item.Initialize(), ex => exMess(mo, ex));
                },
                mo =>
                {
                    stateText = $"添加模组补丁:{mo.info?.name ?? mo.config.key}";

                    Utils.ForHelp(mo.inheritance_mod, item => item.AddPatch(modPatch), ex => exMess2(mo, ex));

                    try
                    {
                        ContentPatch.typePatch.Get<PatchMain>().AddRange(mo.inheritance_patchMain);
                        ContentPatch.typePatch.Get<PatchPlayer>().AddRange(mo.inheritance_patchPlayer);
                        ContentPatch.typePatch.Get<PatchNPC>().AddRange(mo.inheritance_patchNPC);
                        ContentPatch.typePatch.Get<PatchItem>().AddRange(mo.inheritance_patchItem);
                        ContentPatch.typePatch.Get<PatchProjectile>().AddRange(mo.inheritance_patchProjectile);
                        ContentPatch.typePatch.Get<PatchTileLightScanner>().AddRange(mo.inheritance_patchTileLightScanner);
                        ContentPatch.typePatch.Get<PatchRemadeChatMonitor>().AddRange(mo.inheritance_patchRemadeChatMonitor);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(exMess2(mo, ex), ex);
                    }
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
