using CommandHelp;
using CommandHelp.Exceptions;
using System;
using System.Collections.Generic;

namespace tContentPatch.Command
{
    /// <summary>
    /// 指令工具
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 运行指令
        /// </summary>
        /// <param name="command">一段指令</param>
        /// <param name="cos"></param>
        /// <returns>异常信息</returns>
        public static string CommandRun(string command, List<CommandObject> cos)
        {
            CommandException ex = RunCommand.ParseRun(command, cos);

            if (ex == null) return null;

            string msg = null;

            if (ex as CommandLackException != null || ex.InnerException as CommandLackException != null)
            {
                msg = $"\n指令缺失:{ex.ExceptionMessage}";
                if (ex.Line > -1)
                    msg += $"\n位于:{command.Substring(0, ex.Line)}<";
            }
            else
                if (ex as CommandParseException != null || ex.InnerException as CommandParseException != null)
            {
                msg = $"\n指令错误:{ex.ExceptionMessage}";
                if (ex.Line > -1)
                    msg += $"\n位于: {command.Substring(0, ex.Line)}>{command.Substring(ex.Line)?.TrimStart()}<";
            }
            else
            {
                msg = $"\n指令错误:{ex.ExceptionMessage}";
                if (ex.Line > -1)
                    msg += $"\n位于: {command.Substring(0, ex.Line)}>{command.Substring(ex.Line)?.TrimStart()}<";
            }

            return msg;
        }

        public static CommandObject GetCO_OutputCOList(List<CommandObject> cos, string tip = null)
        {
            CommandMethod help = new CommandMethod("?");
            help.Runing += args =>
            {
                string s = null;
                foreach (CommandObject i in cos)
                {
                    if (i == help) continue;

                    if (s == null)
                    {
                        s = $"{i.Text}";
                    }
                    else
                    {
                        s += $", {i.Text}";
                    }
                }

                if (s == null) s = "no cmd";
                if (tip != null) s = $"{s}//{tip}";
                Console.WriteLine(s);
            };

            return help;
        }
    }
}
