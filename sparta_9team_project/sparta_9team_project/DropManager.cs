using System;

namespace sparta_9team_project
{
    public static class DropManager
    {
        private static Random rng = new Random();

        public static bool TryDropCatnip()
        {
            int chance = rng.Next(0, 100);
            return chance < 5;
        }

        public static void TryGiveCatnip() 
        {
            if (TryDropCatnip())
            {
                InventoryManager.Instance.PlayerInventory.AddItem(ItemDataBase.catnip);
                Console.WriteLine("어디선가 캣닢 향기가 난다!");
            }
        }
    }
}
