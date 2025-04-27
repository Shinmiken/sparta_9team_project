using System;

namespace sparta_9team_project
{
    public static class DropManager
    {
        private static Random rng = new Random();

        public static bool TryDropCatnip()
        {
            int chance = rng.Next(0, 100);
            return chance < 5;  // 5% 확률 
        }
    }
}
