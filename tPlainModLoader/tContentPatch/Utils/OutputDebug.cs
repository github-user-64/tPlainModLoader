using System;
using System.Diagnostics;
using System.Reflection;

namespace tContentPatch.Utils
{
    internal class OutputDebug
    {
        public static void OutputException(Exception ex)
        {
            MethodBase method = new StackTrace().GetFrame(1).GetMethod();
            Debug.WriteLine($"{method.ReflectedType.Name}.{method.Name}异常:{ex.Message}");
        }
    }
}
