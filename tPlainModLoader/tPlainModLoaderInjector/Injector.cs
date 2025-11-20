using System;

namespace tPlainModLoaderInjector
{
    internal static class Injector
    {
        private static bool isStart = false;

        public static int InjectManaged(uint tID, string dllPath, Type type, Func<string, int> action, string args)
        {
            if (isStart) return 2;

            string namespaceAclass = type.FullName;
            string method = action.Method.Name;

            //附加一个进程到指定线程上运行
            _ = FastWin32.Diagnostics.Injector.InjectManaged(tID, dllPath, namespaceAclass, method, args, out int returnV);
            /// 附加到其它线程上的函数需要返回int, 需要string参数
            /// 附加到其它线程上的函数会脱离当前线程到指定线程上运行

            switch (returnV)
            {
                case 1:
                    isStart = true;
                    return 1;
                case 2: return 2;
                case -1: return -1;
                case -2: return -2;
                default: return 0;
            }
        }
    }
}
