using System;
using System.Diagnostics;
using System.Reflection;

namespace tContentPatch.Utils
{
    internal static class OutputDebug
    {
        public static void OutputException(Exception ex, int stackTrace = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(stackTrace) ?? st.GetFrame(1);
            MethodBase method = sf.GetMethod();
            Debug.WriteLine($"{method.ReflectedType.Name}.{method.Name}异常:{ex.Message}");
        }
    }
}
