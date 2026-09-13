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
            stateText = "加载模组";

            string exMess = "初始化模组失败:[{0}]:{1}";
            string exMess2 = "添加模组补丁失败:[{0}]:{1}";

            Action<ModObject>[] action = new Action<ModObject>[] {
                mo =>
                {
                    stateText = $"加载模组:{GetModName(mo)}";

                    ModObject copyMo = ModObject.Copy(mo);

                    Utils.ForHelp(mo.inheritance_mod, item => item.Load(copyMo), mo, "加载模组失败:[{0}]:{1}");
                },
                mo =>
                {
                    stateText = $"加载模组设置:{GetModName(mo)}";

                    Utils.ForHelp(mo.inheritance_setting, item => LoadModSet(mo, item), mo, "加载模组设置失败:[{0}]:{1}");
                },
                mo =>
                {
                    stateText = $"初始化模组:{GetModName(mo)}";

                    Utils.ForHelp(mo.inheritance_netPacket, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchMain, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchPlayer, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchNPC, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchItem, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchProjectile, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchTileLightScanner, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchRemadeChatMonitor, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchWorldFile, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchNetMessage, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchMessageBuffer, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchChest, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchRemoteClient, item => item.Initialize(), mo, exMess);

                    Utils.ForHelp(mo.inheritance_patchWorldGen, item => item.Initialize(), mo, exMess);
                },
                mo =>
                {
                    string text = $"注册网络包:{GetModName(mo)}";
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
                    stateText = $"添加模组补丁:{GetModName(mo)}";

                    Utils.ForHelp(mo.inheritance_mod, item => item.AddPatch(modPatch), mo, exMess2);

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
                        throw new Exception(string.Format(exMess2, GetModName(mo), ex.Message), ex);
                    }
                },
                mo =>
                {
                    stateText = $"完成加载模组:{GetModName(mo)}";

                    Utils.ForHelp(mo.inheritance_mod, item => item.Loaded(), mo, "完成加载模组失败:[{0}]:{1}");
                },
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

        private string GetModName(ModObject mo)
        {
            return mo.info?.name ?? mo.config.key;
        }
    }
}
