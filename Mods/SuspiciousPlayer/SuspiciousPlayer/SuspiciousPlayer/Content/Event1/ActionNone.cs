using tContentPatch;

namespace SuspiciousPlayer.Content.Event1
{
    internal class ActionNone : PatchMain
    {
        public static ActionState state = new ActionState(run, end);

        public static void run()
        {
            //清理
            ActionSpawnTile.ClearTile();//方块
            ActionSpawnLunarTower.ClearNPC();//柱子
            ActionCrimsonHeart.ClearHeart();//心脏
            Event.CanSpawnVirtualPlayer = true;//npc生成
            Event.CanSpawnNPC = true;//npc生成
            Event.CanSpawnNPC_SolarCrawltipede = true;//npc生成
            Event.player = null;//目标
        }

        public static void end()
        {
            Event.CanSpawnVirtualPlayer = false;
            Event.CanSpawnNPC = false;
            Event.CanSpawnNPC_SolarCrawltipede = false;
        }
    }
}
