using System;
using System.Diagnostics;
using System.Reflection;

namespace tContentPatch.Patch
{
    internal class AddPatch : IAddPatch
    {
        public readonly string patchId = null;

        public AddPatch(string patchId)
        {
            this.patchId = patchId;
        }

        public void AddPrefix(MethodBase original, MethodInfo prefix)
        {
            try
            {
                PatchUtil.AddPatchPrefix(patchId, original, prefix);

                Debug.WriteLine($"添加补丁[{prefix?.DeclaringType}.{prefix?.Name}]到[{original?.Name}]成功");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"添加补丁[{prefix?.DeclaringType}.{prefix?.Name}]到[{original?.Name}]失败:{ex.Message}");
                throw new Exception("添加补丁失败", ex);
            }
        }

        public void AddPostfix(MethodBase original, MethodInfo postfix)
        {
            try
            {
                PatchUtil.AddPatchPostfix(patchId, original, postfix);

                Debug.WriteLine($"添加补丁[{postfix?.DeclaringType}.{postfix?.Name}]到[{original?.Name}]成功");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"添加补丁[{postfix?.DeclaringType}.{postfix?.Name}]到[{original?.Name}]失败:{ex.Message}");
                throw new Exception("添加补丁失败", ex);
            }
        }

        public void AllPatch()
        {
            PatchUtil.AllPatch(patchId);
        }
    }
}
