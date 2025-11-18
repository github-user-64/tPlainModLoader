using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Utils;

namespace tContentPatch.ModLoad
{
    /// <summary>
    /// 模组对象
    /// </summary>
    public class ModObject
    {
        public ModObject(ModConfig config)
        {
            if (config == null) throw new System.ArgumentNullException(nameof(config));

            this.config = config;
        }

        /// <summary>
        /// 模组文件夹
        /// </summary>
        public string modPath = null;
        /// <summary>
        /// 模组程序集
        /// </summary>
        public Assembly assembly = null;
        /// <summary>
        /// 模组加载配置
        /// </summary>
        public ModConfig config = null;
        /// <summary>
        /// 模组信息
        /// </summary>
        public ModInfo info = null;
        /// <summary>
        /// 继承了<see cref="Mod"/>的类
        /// </summary>
        public List<Mod> inheritance_mod = null;
        /// <summary>
        /// 继承了<see cref="ModSetting"/>的类
        /// </summary>
        public List<ModSetting> inheritance_setting = null;
        /// <summary>
        /// 继承了<see cref="PatchMain"/>的类
        /// </summary>
        public List<PatchMain> inheritance_patchMain = null;
        /// <summary>
        /// 继承了<see cref="PatchPlayer"/>的类
        /// </summary>
        public List<PatchPlayer> inheritance_patchPlayer = null;
        public List<PatchNPC> inheritance_patchNPC = null;
        public List<PatchItem> inheritance_patchItem = null;
        public List<PatchProjectile> inheritance_patchProjectile = null;
        public List<PatchTileLightScanner> inheritance_patchTileLightScanner = null;
        public List<PatchRemadeChatMonitor> inheritance_patchRemadeChatMonitor = null;

        /// <summary>
        /// 复制模组对象的字段, <see cref="config"/>,<see cref="info"/>也为复制对象
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public static ModObject Copy(ModObject mo)
        {
            mo = CopyClass.CopyField(mo, mo.config);
            if (mo.config != null) mo.config = ModConfig.Copy(mo.config);
            if (mo.info != null) mo.info = ModInfo.Copy(mo.info);

            return mo;
        }
    }
}
