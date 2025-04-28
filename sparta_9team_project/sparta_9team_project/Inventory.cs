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
        public Dictionary<string, Item> inventory = new Dictionary<string, Item>()
        {
            [ItemDataBase.longSword.Name] = ItemDataBase.longSword,
            [ItemDataBase.shortarmor.Name] = ItemDataBase.shortarmor,
            [ItemDataBase.longarmor.Name] = ItemDataBase.longarmor,
            [ItemDataBase.shortSword.Name] = ItemDataBase.shortSword
        };

        // 인벤토리 [Methods]
        public bool IsEmpty()
        {
            // 인벤토리가 비어있는지 확인
            return inventory.Count == 0;
        }
        public bool HasItem(Item item)
        {
            // 아이템이 인벤토리에 있는지 확인
            return inventory.ContainsKey(item.Name) && inventory[item.Name].Counts > 0;
        }
        public void AddItem(Item item, int counts)
        {
            if (inventory.ContainsKey(item.Name))
            {
                inventory[item.Name].Counts += counts;
            }
            else
            {
                inventory[item.Name] = item;
                inventory[item.Name].Counts = counts;
            }

            Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이(가) 추가되었습니다.");
        }
        public bool RemoveOneByOne(Item item)
        {
            // 만약 인벤토리에 아이템이 이미 있다면
            // 만약 있는 아이템의 카운트가 0보다 크다면
            // 아이템.카운트 -1
            // 만약 인벤토리에 아이템이 없다면
            // 아이템이 없다는 문구 출력
            if (HasItem(item))
            {
                if (inventory[item.Name].Counts > 1)
                {
                    inventory[item.Name].Counts--;
                }
                else
                {
                    RemoveAll(item);
                }
                return true;
            }
            else
            {
                Console.WriteLine($"{item.Name}은(는) {player.Name}의 소지품에 존재하지 않습니다.");
            }
            return false;
        }
        public void RemoveAll(Item item)
        {
            // 만약 아이템이 인벤토리에 있다면
            // 아이템.카운트 == 0, 아이템 인벤토리에서 삭제
            // 만약 아이템이 인벤토리에 없다면
            // 아이템이 없다는 문구 출력
            if (!HasItem(item))
            {
                inventory[item.Name].Counts = 0;
                inventory.Remove(item.Name);  
            }
            else
            {
                 Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이 없습니다.");
            }
        }
    }
}
