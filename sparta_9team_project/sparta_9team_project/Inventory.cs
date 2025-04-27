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
        public Dictionary<string, Item> inventory = new Dictionary<string, Item>();

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
            if (inventory.ContainsKey(item.Name) && item.Counts > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AddItem(Item item, int counts)
        {
            item.Counts += counts;
            Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이(가) 추가되었습니다.");
        }
        public void RemoveOneByOne(Item item)
        {
            // 만약 인벤토리에 아이템이 이미 있다면
            // 만약 있는 아이템의 카운트가 0보다 크다면
            // 아이템.카운트 -1
            // 만약 인벤토리에 아이템이 없다면
            // 아이템이 없다는 문구 출력
            if (HasItem(item))
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
            if (HasItem(item))
            {
                item.Counts = 0;
                inventory.Remove(item.Name);  
            }
            else
            {
                 Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이 없습니다.");
            }
        }
        // 우유 개수 확인용 - 황연주
        public int GetItemCount(string itemName)
        {
            if (inventory.ContainsKey(itemName))
            {
                return inventory[itemName].Counts;
            }
            return 0;
        }
        // 유리 조각 조합용 - 황연주
        public void CombineItems(string baseItemName)
        {
            int count = GetItemCount(baseItemName);
            if (count >= 5)
            {
                // 유리병 조합 성공
                RemoveAll(inventory[baseItemName]); // 기존 유리조각 삭제
                AddItem(new Item("유리병", ItemType.소모품, 1, "유리조각 5개를 모아 완성된 유리병입니다."), 1);
                Console.WriteLine($"[{baseItemName}] 5개를 모아 유리병을 완성했습니다!");
            }
            else
            {
                Console.WriteLine($"{baseItemName}이(가) 부족합니다. (현재 {count}/5)");
            }
        }    
    }
}
