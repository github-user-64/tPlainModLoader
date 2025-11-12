using CommandHelp;

namespace Skil.Utils
{
    internal class CommandString: CommandValue<string>
    {
        public override string Text => "<string>";
        protected override string ArgConvertThrow(string arg) => arg;
        protected override string GetDefault() => null;
    }
}
