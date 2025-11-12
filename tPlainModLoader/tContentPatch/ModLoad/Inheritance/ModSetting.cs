using System;
using System.Linq;
using System.Reflection;
using tContentPatch.ModLoad;
using tContentPatch.Utils;
using Terraria.UI;

namespace tContentPatch
{
    /// <summary>
    /// 模组设置
    /// </summary>
    public abstract class ModSetting
    {
        /// <summary>
        /// 需要保存
        /// </summary>
        public bool NeedSave { get; protected set; } = false;
        /// <summary>
        /// 设置项名称
        /// </summary>
        public virtual string Name => null;
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title => null;
        /// <summary>
        /// 文件相对位置, 不设置则不保存
        /// </summary>
        public virtual string FilePath => null;
        /// <summary>
        /// 是否在模组设置UI中
        /// </summary>
        public virtual bool HasUI => true;
        /// <summary>
        /// 数据类型
        /// </summary>
        public virtual Type DataType => null;
        /// <summary>
        /// 在<see cref="Mod.Load"/>之后调用, 根据<see cref="FilePath"/>读取文件, 读取失败为<see langword="null"/>
        /// </summary>
        /// <param name="v"></param>
        public virtual void Load(object v) { }
        /// <summary>
        /// 获取设置界面
        /// </summary>
        /// <returns></returns>
        public virtual UIElement GetUI() => null;
        /// <summary>
        /// 设为默认
        /// </summary>
        public virtual void SetDefault() { }
        /// <summary>
        /// 获取需要保存的数据
        /// </summary>
        /// <returns>需要保存的数据</returns>
        public virtual object GetSaveData() => null;
        /// <summary>
        /// 保存
        /// </summary>
        public virtual void Save()
        {
            if (NeedSave == false) return;

            Assembly assembly = GetType().Assembly;
            ModObject mo = LoaderControl.GetModObjects()?.FirstOrDefault(i => i.assembly == assembly);

            ModFile.SaveFileTry(FilePath, file =>
            {
                MyJson1.Save(GetSaveData(), file);
                return true;
            }, mo);

            NeedSave = false;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public virtual object Read()
        {
            Assembly assembly = GetType().Assembly;
            ModObject mo = LoaderControl.GetModObjects()?.FirstOrDefault(i => i.assembly == assembly);

            object v = null;
            ModFile.ReadFileTry(FilePath, file =>
            {
                v = MyJson1.Get2(file, DataType);
                return true;
            }, mo);
            return v;
        }
    }
}
