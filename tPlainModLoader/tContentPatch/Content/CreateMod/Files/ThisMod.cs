using tContentPatch;
using tContentPatch.ModLoad;

namespace {{ModName}}
{
    /// <summary>
    /// 原版游戏的ReLogic.dll文件无法直接引用, 可以用tModLoader那的dll替代<br/>
    /// 修改引用位置只需在ConfigPublic.targets文件中修改DllReLogic的值即可
    /// </summary>
    internal class ThisMod : Mod
    {
        public override void Load(ModObject mo)
        {

        }
    }
}
