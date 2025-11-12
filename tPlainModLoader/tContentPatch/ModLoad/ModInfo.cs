using tContentPatch.Utils;

namespace tContentPatch.ModLoad
{
    /// <summary>
    /// 模组信息
    /// </summary>
    public class ModInfo
    {
        /// <summary>
        /// 模组名称
        /// </summary>
        public string name = null;
        /// <summary>
        /// 作者
        /// </summary>
        public string author = null;
        /// <summary>
        /// 模组描述
        /// </summary>
        public string description = null;
        /// <summary>
        /// 跳转到相关链接
        /// </summary>
        public string jumpPath = null;

        /// <summary>
        /// 复制模组信息
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public static ModInfo Copy(ModInfo mi)
        {
            if (mi == null) return null;

            return CopyClass.CopyField(mi);
        }
    }
}
