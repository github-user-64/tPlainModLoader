using System;

namespace tContentPatch.Utils
{
    internal static class ConsoleUtils
    {
        private static bool canClear = true;

        public static void Clear()
        {
            if (canClear == false) return;

            try
            {
                Console.Clear();
                canClear = true;
            }
            catch
            {
                canClear = false;
            }
        }
    }
}
