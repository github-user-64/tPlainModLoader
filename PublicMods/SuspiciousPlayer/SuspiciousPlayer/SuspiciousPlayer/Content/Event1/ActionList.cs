using System.Collections.Generic;
using tContentPatch;

namespace SuspiciousPlayer.Content.Event1
{
    internal class ActionList : ModMain
    {
        public static ActionState state = new ActionState(run);
        private static List<int> stateId = new List<int>();
        private static int index = 0;

        public static void run()
        {
            if (stateId.Count < 1)
            {
                Event.SetEventState(Event.EventState_None);
                return;
            }

            if (index < 0) index = 0;
            else if (index >= stateId.Count) index = 0;

            ++index;
            Event.SetEventState(stateId[index - 1]);
        }

        public static void SetList(List<int> stateId)
        {
            ActionList.stateId = stateId;
            index = 0;
        }
    }
}
