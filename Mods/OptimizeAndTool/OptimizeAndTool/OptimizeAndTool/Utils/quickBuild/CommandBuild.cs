using CommandHelp;

namespace OptimizeAndTool.Utils.quickBuild
{
    /// <summary>
    /// 创建一些预设的指令结构
    /// </summary>
    internal static class CommandBuild
    {
        /// <summary>
        /// help, reset, true, false, set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="gsr"></param>
        /// <param name="setGsr"></param>
        /// <param name="cos"></param>
        /// <returns></returns>
        public static CommandObject get1<T>(string text, GetSetReset<bool> gsr, GetSetReset<T> setGsr, params CommandObject[] cos)
        {
            CommandHRA<T> damage_set = new CommandHRA<T>("set", setGsr,
                cos);

            CommandHRA<bool> cmd1 = new CommandHRA<bool>(text, gsr,
                new CommandTrue(), new CommandFalse(), damage_set);

            return cmd1;
        }

        /// <summary>
        /// help, reset, true, false
        /// </summary>
        /// <param name="text"></param>
        /// <param name="gsr"></param>
        /// <returns></returns>
        public static CommandObject get2(string text, GetSetReset<bool> gsr)
        {
            CommandHRA<bool> cmd1 = new CommandHRA<bool>(text, gsr,
                new CommandTrue(), new CommandFalse());

            return cmd1;
        }

        /// <summary>
        /// help, reset, true, false, add
        /// </summary>
        /// <param name="text"></param>
        /// <param name="gsr"></param>
        /// <param name="cos"></param>
        /// <returns></returns>
        public static CommandObject get3(string text, GetSetReset<bool> gsr, params CommandObject[] cos)
        {
            CommandObject[] add = new CommandObject[cos.Length + 2];
            add[0] = new CommandTrue();
            add[1] = new CommandFalse();
            for (int i = 0; i < cos.Length; ++i) add[i + 2] = cos[i];

            CommandHRA<bool> cmd1 = new CommandHRA<bool>(text, gsr, add);

            return cmd1;
        }
    }
}
