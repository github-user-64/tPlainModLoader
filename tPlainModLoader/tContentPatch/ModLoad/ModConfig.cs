using tContentPatch.Utils;

namespace tContentPatch.ModLoad
{
    /// <summary>
    /// 模组配置
    /// </summary>
    public class ModConfig
    {
        /// <summary>
        /// 用于识别mod的id, 必填
        /// </summary>
        public string key = null;
        /// <summary>
        /// 版本
        /// </summary>
        public string version = null;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isEnable = false;
        /// <summary>
        /// 用于加载的dll路径, 为<see langword="null"/>不加载dll
        /// </summary>
        public string dllPath = null;
        /// <summary>
        /// 该mod必须的前置mod的key
        /// </summary>
        public string[] frontModKeys = null;

        /// <summary>
        /// 复制模组配置
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public static ModConfig Copy(ModConfig mc)
        {
            if (mc == null) return null;

            mc = CopyClass.CopyField(mc);
            if (mc.frontModKeys == null) return mc;

            string[] fmk = mc.frontModKeys;
            mc.frontModKeys = new string[fmk.Length];
            for (int i = 0; i < fmk.Length; ++i) mc.frontModKeys[i] = fmk[i];

            return mc;
        }
    }
}
