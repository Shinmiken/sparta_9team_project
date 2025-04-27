/*using System;
using System.Collections.Generic;

namespace sparta_9team_project
{
    public class PlayerInventory
    {
        private List<Item> inventoryList = new List<Item>();

        // 아이템 1개 추가하는 버전
        public void AddItem(Item item)
        {
            inventoryList.Add(item);
            Console.WriteLine($"[인벤토리 추가] {item.Name} 1개");
        }

        // 아이템 여러 개 추가하는 버전
        public void AddItem(Item item, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                inventoryList.Add(item);
            }
            Console.WriteLine($"[인벤토리 추가] {item.Name} {amount}개");
        }

        // 인벤토리 출력하는 함수
        public void ShowInventory()
        {
            Console.WriteLine("\n[인벤토리 목록]");
            foreach (var item in inventoryList)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
    }
}*/  
