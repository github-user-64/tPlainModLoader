using System.Reflection;

namespace tContentPatch.Patch
{
    /// <summary>
    /// 修补方法的详细方式请访问<para/>
    /// https://harmony.pardeike.net/articles/intro.html<para/>
    /// https://harmony.pardeike.net/<para/>
    /// https://github.com/pardeike/Harmony/wiki
    /// </summary>
    public interface IAddPatch
    {
        /// <summary>
        /// 在方法之前
        /// </summary>
        /// <param name="original"></param>
        /// <param name="prefix"></param>
        void AddPrefix(MethodBase original, MethodInfo prefix);
        /// <summary>
        /// 在方法之后
        /// </summary>
        /// <param name="original"></param>
        /// <param name="prefix"></param>
        void AddPostfix(MethodBase original, MethodInfo prefix);
    }
}
