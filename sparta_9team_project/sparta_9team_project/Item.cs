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
    public class Item
    {
        public Player player = PlayerManager.instance.mainPlayer; // 플레이어 싱글톤 활성화
        public Dictionary<string, int> inventory = InventoryManager.Instance.PlayerInventory.inventory;            // 플레이어 인벤토리 엑세스
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
        // 무기

        // 방어구

        // 소모품
        public static Consumable smallHealingPotion = new Consumable(30, "힐링포션(소)", ItemType.소모품, ConsumableEffect.체력회복, 1, "체력을 30 회복합니다.");

        // [Methods]
    }

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    // [Item]클래스를 상속받은 자식 클래스들
    public class Weapon : Item
    {
        public int AttackPower { get; set; }
        public Weapon(int attackPower, string name, ItemType type, int counts, string description ) : base(name, ItemType.무기, counts, description)
        {
            AttackPower = attackPower;
        }
    }

    public class Armor : Item
    {
        public int DefensePower { get; set; }
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
        public Consumable (int effectAmount, string name, ItemType type, ConsumableEffect effectType, int counts, string description) : base(name, ItemType.소모품, counts, description)
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
            bool contains = ContainsItem(item);                                                                 // 아이템이 인벤토리에 있는지 체크
            bool isConsumable = IsConsumable(item);                                                             // 아이템이 소모품인지 체크   
                
            if (!contains)                                                                                      // 아이템이 인벤토리에 없다면
            {
                Console.WriteLine($"포션이 부족합니다.");
                return;
            }
            else if (contains && isConsumable)                                                                   // 아이템이 인벤토리에 있다면 && 소모품이라면
            {
                switch (EffectType)                                                                              // ConsumableEffect 타입 확인
                {
                    case ConsumableEffect.체력회복:
                        int needsHeal = player.MaxHp - player.Hp;                                                // 현재 체력과 최대 체력의 차이
                        int actualHealAmount = Math.Min(EffectAmount, needsHeal);                                // 실제 회복량
                        player.Hp += actualHealAmount;                                                           // 플레이어 체력 회복

                        Counts -= 1;                                                                             // 사용된 아이템 개수 감소
                        Console.WriteLine($"{player.Name}이(가) {Name}을(를) 사용했습니다.\n회복을 완료했습니다.");
                        break;

                        //아이템 추가 가능
                }

            }

        }

        public void ShowConsumables()
        {
            // 인벤토리가 비었나 검사
            // 인벤토리에서 소모품만 필터링
            // 필터링 후 소모품만 있는 딕셔너리에 저장
            // 소모품만 있는 딕셔너리에서 아이템 이름과 개수 출력
            Dictionary<string, int> onlyConsumables = new Dictionary<string, int>();

            bool contains = ContainsItem()

            


        }
    }
}
