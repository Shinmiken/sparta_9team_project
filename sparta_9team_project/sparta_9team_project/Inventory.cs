namespace sparta_9team_project
{
    class Inventory
    {
        // 플레이어 싱글톤 활성화
        Player player = PlayerManager.instance.mainPlayer;


        // 플레이어의 인벤토리 리스트
        private List<Item> items = new List<Item>()
        {
            // 포션은 기본적으로 3개 있습니다.
            ItemDataBase.smallHealingPotion,
            ItemDataBase.smallHealingPotion,
            ItemDataBase.smallHealingPotion
        };


        // 인벤토리 [Methods]
        public void AddItem(Item item)
        {
            if (items.Contains(item))
            {
                item.Counts++;
                return;
            }
            else
            {
                items.Add(item);
            }

            Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이(가) 추가되었습니다.");
        }
        public void RemoveOneByOne(Item item)
        {
            if (items.Contains(item))
            {
                item.Counts--;
                if (item.Counts <= 0)
                {
                    items.Remove(item);
                }
            }
            else
            {
                Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이 없습니다.");
            }
        }
        public void RemoveAll(Item item )
        {
            if (items.Contains(item))
            {
                items.Remove(item);
            }
            else
            {
                 Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이 없습니다.");
            }
        }
    }
}
