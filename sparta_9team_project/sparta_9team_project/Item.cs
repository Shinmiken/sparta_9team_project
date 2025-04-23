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
        공격력증가,
        방어력증가,
    }

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    // [Item] - 부모 클래스 
    public class Item
    {
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
        public virtual void UseItem(Player player) 
        {
            if (Counts <= 0)
            {
                Console.WriteLine($"{player.Name}의 소지품에 {Name}이 없습니다.");
                return;
            }
        }
    }


    // [ItemDataBase] - 아이템 데이터베이스
    public static class ItemDataBase
    {
        // 무기

        // 방어구

        // 소모품
        public static Consumable smallHealingPotion = new Consumable(30, "힐링포션(소)", ItemType.소모품, ConsumableEffect.체력회복, 1, "체력을 30 회복합니다.");
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

        // UseItem() 사용법:
        // 01) ItemDataBase.smallHealingPotion.UseItem(player);
        public override void UseItem(Player player)
        {
            base.UseItem(player);                                                       // 아이템이 인벤토리에 없다면
            
            switch (EffectType)                                                         // 아이템이 인벤토리에 있다면 && ConsumableEffect 타입 확인
            {
                case ConsumableEffect.체력회복:
                    int needsHeal = player.MaxHp - player.Hp;                           // 현재 체력과 최대 체력의 차이
                    int actualHealAmount = Math.Min(EffectAmount, needsHeal);           // 실제 회복량
                    player.Hp += actualHealAmount;                                      // 플레이어 체력 회복

                    Counts -= 1;                                                        // 사용된 아이템 개수 감소
                    Console.WriteLine($"{player.Name}이(가) {Name}을(를) 사용했습니다.");
                    break;

                    //아이템 추가 가능
            }

        }
    }
}
