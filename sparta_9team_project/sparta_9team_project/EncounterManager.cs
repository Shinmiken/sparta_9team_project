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
    }
}
