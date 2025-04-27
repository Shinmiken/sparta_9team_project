using System;
using System.Collections.Generic;

namespace sparta_9team_project
{
    public static class EncounterManager
    {
        private static Enemy[] currentEnemies;

        // 몬스터 체력 추적용
        public static void SetupEnemies(Enemy[] enemies)
        {
            currentEnemies = enemies;
        }

        // 체력 5 이하로 남은 몬스터 카운트용
        public static int CountLowHpEnemies(int hpThreshold = 5)
        {
            if (currentEnemies == null) return 0;

            int count = 0;
            foreach (var enemy in currentEnemies)
            {
                if (enemy != null && enemy.Hp > 0 && enemy.Hp <= hpThreshold)
                {
                    count++;
                }
            }
            return count;
        }

        // 캣닢 사용 체크 
        public static void CheckCatnipUsage()
        {
            var quest = QuestManager.AllQuests.Find(q => q.TITLE == "3 아가냥이와 친구가 되었어요 !");
            if (quest != null && quest.IS_ACCEPTED && !quest.IS_COMPLETED)
            {
                if (PlayerManager.instance.mainPlayer.HasUsedItem("캣닢"))
                {
                    quest.CURRENT_COUNT++; // 퀘스트 진행도 증가
                    Console.WriteLine("퀘스트 진행도 +1!");
                }
            }
        }
    }
}
