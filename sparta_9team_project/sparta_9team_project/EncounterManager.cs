using System;
using System.Collections.Generic;

namespace sparta_9team_project
{
    public static class EncounterManager
    {
        // 현재 전투 중인 몬스터 리스트
        public static List<Enemy> CurrentEnemies = new List<Enemy>();

        // 몬스터 리스트 초기화
        public static void SetupEnemies(Enemy[] enemiesArray)
        {
            CurrentEnemies.Clear();
            foreach (var enemy in enemiesArray)
            {
                CurrentEnemies.Add(enemy);
            }
        }

        // 살아있는 몬스터 수 세기
        public static int CountAliveEnemies()
        {
            int count = 0;
            foreach (var enemy in CurrentEnemies)
            {
                if (enemy.Hp > 0)
                    count++;
            }
            return count;
        }
    }
}
