using System;

namespace SuspiciousPlayer.Content.Event1
{
    internal class ActionState
    {
        public bool norun => !runing;
        public bool runing = false;
        private Action run = null;
        private Action end = null;

        public ActionState(Action run = null, Action end = null)
        {
            this.run = run;
            this.end = end;
        }

        public void Run()
        {
            runing = true;
            run?.Invoke();
        }
        public void End()
        {
            runing = false;
            end?.Invoke();
        }
    }
}
