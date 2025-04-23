namespace sparta_9team_project
{
    public enum ItemType
    {
        무기,
        방어구,
        소모품,
    }

    // [Item] 클래스 - 부모 클래스 & 아이템 데이터베이스
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
                Counts = 0;
                Console.WriteLine($"{Name}이(가) 없습니다.");
            }
        }
    }

    public class ItemDataBase
    {
        // 무기

        // 방어구

        // 소모품
        public static Consumable smallHealingPotion = new Consumable(30, "힐링포션(소)", ItemType.소모품, 1, "체력을 30 회복합니다.");
    }

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
        public int EffectAmount { get; set; }
        public Consumable (int effectAmount, string name, ItemType type, int counts, string description) : base(name, ItemType.소모품, counts, description)
        {
            EffectAmount = effectAmount;
        }

        public override void UseItem(Player player)
        {
            base.UseItem(player);

            Counts -= 1; // 사용한 아이템 개수 
            Console.WriteLine($"{player.Name}이(가) {Name}을(를) 사용했습니다.");

        }
    }
}
