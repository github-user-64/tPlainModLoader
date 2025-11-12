using CommandHelp;
using System.Collections.Generic;

namespace tContentPatch
{
    public abstract class Mod
    {
        /// <summary>
        /// 类被创建时调用
        /// </summary>
        public virtual void Load() { }
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>之后调用
        /// </summary>
        public virtual void Loaded() { }
        /// <summary>
        /// 模组被卸载时调用
        /// </summary>
        public virtual void Unload() { }
        /// <summary>
        /// 注意! 你可以使用这个做到更灵活的操作, 但是注意在重新加载模组时, 要修补到目标方法上的方法可能会是已卸载的模组上的方法<para/>
        /// 注意! 如果使用了该方法修补内容并要重新加载模组时, 建议直接重启tPlainModLoader<para/>
        /// 添加修补, 在<see cref="Loaded"/>之后调用<para/>
        /// 修补方法的详细方式请访问<para/>
        /// <a href="https://harmony.pardeike.net/articles/intro.html"/><para/>
        /// <a href="https://harmony.pardeike.net"/><para/>
        /// <a href="https://github.com/pardeike/Harmony/wiki"/></summary>
        /// <param name="addPatch"></param>
        public virtual void AddPatch(Patch.IAddPatch addPatch) { }
        /// <summary>
        /// 获取指令用于添加指令<para/>
        /// <see cref="CommandHelp"/>项目地址 <a href="https://github.com/github-user-64/CommandHelp"/>
        /// </summary>
        /// <returns></returns>
        public virtual List<CommandObject> GetCommands() => null;
    }
}
