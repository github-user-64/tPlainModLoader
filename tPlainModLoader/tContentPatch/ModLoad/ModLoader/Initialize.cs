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

                    ModObject copyMo = ModObject.Copy(mo);

                    Utils.ForHelp(mo.inheritance_mod, item => item.Load(copyMo), ex => exMess(mo, ex));
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

                    Utils.ForHelp(mo.inheritance_netPacket, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchMain, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchPlayer, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchNPC, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchItem, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchProjectile, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchTileLightScanner, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchRemadeChatMonitor, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchWorldFile, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchNetMessage, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchMessageBuffer, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchChest, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchRemoteClient, item => item.Initialize(), ex => exMess(mo, ex));

                    Utils.ForHelp(mo.inheritance_patchWorldGen, item => item.Initialize(), ex => exMess(mo, ex));
                },
                mo =>
                {
                    string text = $"注册网络包:{mo.info?.name ?? mo.config.key}";
                    stateText = text;

                    try
                    {
                        Content.Network.ModNetworkPacket.Register(mo.inheritance_netPacket);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"失败:{text}", ex);
                    }
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
                        ContentPatch.typePatch.Get<PatchWorldFile>().AddRange(mo.inheritance_patchWorldFile);
                        ContentPatch.typePatch.Get<PatchNetMessage>().AddRange(mo.inheritance_patchNetMessage);
                        ContentPatch.typePatch.Get<PatchMessageBuffer>().AddRange(mo.inheritance_patchMessageBuffer);
                        ContentPatch.typePatch.Get<PatchChest>().AddRange(mo.inheritance_patchChest);
                        ContentPatch.typePatch.Get<PatchRemoteClient>().AddRange(mo.inheritance_patchRemoteClient);
                        ContentPatch.typePatch.Get<PatchWorldGen>().AddRange(mo.inheritance_patchWorldGen);
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
