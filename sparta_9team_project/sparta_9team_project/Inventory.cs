namespace sparta_9team_project
{
    public class InventoryManager
    {
        // 인벤토리 싱글톤 활성화
        private static InventoryManager _instance;
        public static InventoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InventoryManager();
                }
                return _instance;
            }
        }

        public Inventory PlayerInventory { get; private set; }
        private InventoryManager() => PlayerInventory = new Inventory();
    }

    public class Inventory
    {
        // 플레이어 싱글톤 활성화
        Player player = PlayerManager.instance.mainPlayer;


        // 플레이어의 인벤토리 리스트
        public Dictionary<string, int> inventory = new Dictionary<string, int>()
        {
            [ItemDataBase.smallHealingPotion.Name] = ItemDataBase.smallHealingPotion.Counts + 3, 
        };

        // 인벤토리 [Methods]
        public bool IsEmpty()
        {
            // 인벤토리가 비어있는지 확인
            if (inventory.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool HasItem(Item item)
        {
            // 아이템이 인벤토리에 있는지 확인
            if (inventory.ContainsKey(item.Name) && inventory[item.Name] > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool HasUsed(Item item) // 제작중. 완성 X
        {
            bool yes = HasItem(item);

            return yes;
        }
        public void AddItem(Item item)
        {
            // 만약 인벤토리에 이미 아이템이 있다면
            // 아이템.카운트 + 1
            // 만약 인벤토리에 아이템이 없다면
            // 아이템 추가 후 카운트 +1
            if (inventory.ContainsKey(item.Name))
            {
                item.Counts++;
                return;
            }
            else
            {
                inventory[item.Name] = item.Counts + 1;
            }

            Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이(가) 추가되었습니다.");
        }
        public void RemoveOneByOne(Item item)
        {
            // 만약 인벤토리에 아이템이 이미 있다면
            // 만약 있는 아이템의 카운트가 0보다 크다면
            // 아이템.카운트 -1
            // 만약 인벤토리에 아이템이 없다면
            // 아이템이 없다는 문구 출력
            if (inventory.ContainsKey(item.Name))
            {   
                if (item.Counts > 0)
                {
                    item.Counts--;
                }
                else if (item.Counts <= 0)
                {
                    RemoveAll(item);
                }
            }
            else
            {
                Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이 없습니다.");
            }
        }
        public void RemoveAll(Item item)
        {
            // 만약 아이템이 인벤토리에 있다면
            // 아이템.카운트 == 0, 아이템 인벤토리에서 삭제
            // 만약 아이템이 인벤토리에 없다면
            // 아이템이 없다는 문구 출력
            if (inventory.ContainsKey(item.Name))
            {
                item.Counts = 0;
                inventory.Remove(item.Name);  
            }
            else
            {
                 Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이 없습니다.");
            }
        }
    }
}
