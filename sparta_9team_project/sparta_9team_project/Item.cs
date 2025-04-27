namespace sparta_9team_project
{
    public enum ItemType
    {
        무기,
        방어구,
        소모품,
    }


    public enum ConsumableEffect
    {
        체력회복,
        마나회복,
        공격력증가,
        방어력증가,
    }

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    // [Item] - 부모 클래스 
    public abstract class Item
    {
        public Player player = PlayerManager.instance.mainPlayer;                                                  // 로컬변수에 플레이어 싱글톤 저장
        public Inventory invenManager = InventoryManager.Instance.PlayerInventory;                                 // 로컬변수에 인벤토리 싱글톤 저장
        public Dictionary<string, Item> inventory = InventoryManager.Instance.PlayerInventory.inventory;            // 로컬변수에 인벤토리 리스트 싱글톤 저장

        // [Fields]
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int Counts { get; set; } = 1; // 아이템 개수
        public string Description { get; set; }

        // [Constructor]
        public Item(string name, ItemType type, int counts, string description)
        {
            Name = name;
            Type = type;
            Counts = counts;
            Description = description;
        }

        // [Methods]
        public virtual void UseItem(Item item) { }
    }


    // [ItemDataBase] - 아이템 데이터베이스
    public static class ItemDataBase
    {
        // 아이템 저장고
        // 무기
        public static Dictionary<string, Item> weaponStorage;

        // 방어구
        public static Dictionary<string, Item> armorStorage;

        // 소모품      
        public static Dictionary<string, Item> consumableStorage;
        public static Consumable smallHealingPotion;
        
        // 소모품 - 퀘스트 아이템
        public static Item fish;
        public static Item glassPiece;
        public static Item blessing9jo;
        public static Item catnip;

        static ItemDataBase()
        {
            smallHealingPotion = new Consumable(30, "힐링포션(소)", ItemType.소모품, ConsumableEffect.체력회복, 1, "체력을 30 회복합니다.");

            // 퀘스트용 아이템 추가
            fish = new Consumable(0, "생선", ItemType.소모품, ConsumableEffect.None, 1, "고양이한테 생선을 맡기면 ?");
            glassPiece = new Consumable(0, "유리조각", ItemType.소모품, ConsumableEffect.None, 1, "꽤 큰 유리조각이다.");
            blessing9jo = new Consumable(0, "⟡༺༒9조의 축복༒༻⟡", ItemType.소모품, ConsumableEffect.None, 1, "이제 미르는 누구도 무섭지 않아요 !");
            catnip = new Consumable(0, "캣닢", ItemType.소모품, ConsumableEffect.None, 1, "아가냥이가 세상에서 제일 좋아하는 풀이라고 한다.")       

            consumableStorage = new Dictionary<string, Item>
            {
                [smallHealingPotion.Name] = smallHealingPotion,
                [fish.Name] = fish,
                [glassPiece.Name] = glassPiece,
                [blessing9jo.Name] = blessing9jo,
                [catnip.Name] = catnip
            };
        }

    }
    

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    // [Item]클래스를 상속받은 자식 클래스들
    public class Weapon : Item
    {  
        // [Fields]
        public int AttackPower { get; set; }

        // [Constructor]
        public Weapon(int attackPower, string name, ItemType type, int counts, string description ) : base(name, ItemType.무기, counts, description)
        {
            AttackPower = attackPower;
        }
    }


    public class Armor : Item
    {
        // [Fields]
        public int DefensePower { get; set; }

        // [Constructor]
        public Armor (int defensePower, string name, ItemType type, int counts, string description) : base(name, ItemType.방어구, counts, description)
        {
            DefensePower = defensePower;
        }
    }


    public class Consumable : Item
    {
        // [Fields]
        public int EffectAmount { get; set; }
        public ConsumableEffect EffectType { get; set; }

        // [Constructor]
        public Consumable(int effectAmount, string name, ItemType type, ConsumableEffect effectType, int counts, string description) : base(name, ItemType.소모품, counts, description)
        {
            EffectAmount = effectAmount;
            EffectType = effectType;
        }

        // [Methods]
        public bool IsConsumable(Item item)
        {
            // 존재하는 아이템이 소모품타입인지 체크
            if (Type == ItemType.소모품)         
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UseItem(Item item)
        {
            bool contains = invenManager.HasItem(item);                                                          // 아이템이 인벤토리에 있는지 체크
                
            if (contains)                                                                                        // 아이템이 인벤토리에 있다면 && 소모품이라면               
            {
                switch (EffectType)                                                                              // ConsumableEffect 타입 확인
                {
                    case ConsumableEffect.체력회복:                                                               // ConsumableEffect 타입 == 체력회복인 경우 
                        int needsHeal = player.MaxHp - player.Hp;                                                // 현재 체력과 최대 체력의 차이
                        int actualHealAmount = Math.Min(EffectAmount, needsHeal);                                // 실제 회복량
                        player.Hp += actualHealAmount;                                                           // 플레이어 체력 회복

                        Counts -= 1;                                                                             // 사용된 아이템 개수 감소
                        Console.WriteLine($"{player.Name}이(가) {Name}을(를) 사용했습니다.\n회복을 완료했습니다.");
                        break;
                    case ConsumableEffect.공격력증가:
                        player.Atk += EffectAmount;                                                      // 플레이어 공격력 증가
                        break;
                    case ConsumableEffect.방어력증가:
                        player.Def += EffectAmount;                                                      // 플레이어 방어력 증가
                        break;
                        //아이템 추가 가능
                }
            }
            else                                                                  
            {   
                Console.WriteLine($"해당 아이템이 부족합니다.");                                                           // 아이템이 인벤토리에 없다면
                return;
            }

        }

        public void ShowOnlyConsumables()
        {
            // 인벤토리가 비었나 검사
            // 안 비었다면 인벤토리에서 소모품만 필터링
            // 필터링 후 소모품만 있는 딕셔너리에 저장
            // 소모품만 있는 딕셔너리에서 아이템 이름과 개수 출력
            Dictionary<string, Item> onlyConsumables = new Dictionary<string, Item>();

            bool isEmpty = invenManager.IsEmpty();                               // 인벤토리 비었는지 체크

            if (!isEmpty)
            {
                foreach (var (item, count) in inventory)                         // 인벤토리에서 소모품 필터링
                {
                    Item itemName = ItemDataBase.consumableStorage[item];        // 아이템 객체의 아이템 이름값 접근
                    bool yes = IsConsumable(itemName);                           // 타입이 소모품인지 체크
                    if (item == itemName.Name && yes)
                    {
                        onlyConsumables[item] = count;                           // 소모품만 있는 딕셔너리에 저장
                    }
                }

                int i = 1;
                string index = "";
                foreach (var (item, count) in onlyConsumables)                   // 소모품만 있는 딕셔너리 출력
                {
                    index = i.ToString();
                    Console.WriteLine($"{index:D2}{item}: {count}개");
                    i++;// 아이템 이름과 개수 출력
                }

                return;
            }
            else
            {
                Console.WriteLine($"{player.Name}의 소지품함이 현재 비어있습니다");
                return;
            }
        }
    }
}
