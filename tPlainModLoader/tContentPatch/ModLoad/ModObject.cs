using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Utils;
using Terraria;

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
        /// 继承了<see cref="ModMain"/>的类
        /// </summary>
        public List<ModMain> inheritance_modMain = null;
        /// <summary>
        /// 继承了<see cref="ModPlayer"/>的类
        /// </summary>
        public List<ModPlayer> inheritance_modPlayer = null;
        public List<ModNPC> inheritance_modNPC = null;
        public List<ModItem> inheritance_modItem = null;
        public List<ModProjectile> inheritance_modProjectile = null;
        /// <summary>
        /// 继承了<see cref="ModTileLightScanner"/>的类
        /// </summary>
        public List<ModTileLightScanner> inheritance_tileLightScanner = null;
        /// <summary>
        /// 继承了<see cref="ModRemadeChatMonitor"/>的类
        /// </summary>
        public List<ModRemadeChatMonitor> inheritance_remadeChatMonitor = null;

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
