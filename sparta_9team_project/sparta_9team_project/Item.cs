﻿using System.ComponentModel;
using System.Text;

namespace sparta_9team_project
{
    public enum ItemType
    {
        무기,
        방어구,
        소모품,
        우유,
        None
    }


    public enum ConsumableEffect
    {
        체력회복,
        마나회복,
        공격력증가,
        방어력증가,
        None
    }

    /* -------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

    // [Item] - 부모 클래스 
    public class Item
    {
        public Player player => PlayerManager.instance.mainPlayer;                                                  // 로컬변수에 플레이어 싱글톤 저장
        public Inventory invenManager => InventoryManager.Instance.PlayerInventory;                                 // 로컬변수에 인벤토리 싱글톤 저장
        public Dictionary<string, Item> inventory => InventoryManager.Instance.PlayerInventory.inventory;            // 로컬변수에 인벤토리 리스트 싱글톤 저장

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
        public static Player player = PlayerManager.instance.mainPlayer;

        // 아이템 저장고
        // 무기
        public static Dictionary<string, Item> weaponStorage;
        public static Weapon longSword;
        public static Weapon shortSword;
        // 방어구
        public static Dictionary<string, Item> armorStorage;
        public static Armor shortarmor;
        public static Armor longarmor;

        // 소모품      
        public static Dictionary<string, Item> consumableStorage;
        public static Consumable smallHealingPotion;
        
        // 소모품 - 퀘스트 아이템
        public static Consumable fish;
        public static Consumable glassPiece;
        public static Consumable blessing9jo;
        public static Consumable catnip;

        // 소모품 & 보스방 열쇠 - 우유
        public static Milk milk;

        static ItemDataBase()
        {
            // 무기
            longSword = new Weapon(30, "롱소드", ItemType.무기, 1, "공격력이 10증가합니다.");
            shortSword = new Weapon(15, "요술지팡이", ItemType.무기, 1, "공격력이 15증가합니다.");
            weaponStorage = new Dictionary<string, Item>
            {
                [longSword.Name] = longSword,
                [shortSword.Name] = shortSword
            };
            smallHealingPotion = new Consumable(30, "힐링포션(소)", ItemType.소모품, ConsumableEffect.체력회복, 1, "체력을 30 회복합니다.");
            //방어구
            shortarmor = new Armor(30, "갑옷", ItemType.방어구, 1, "방어력이 10증가합니다.");
            longarmor = new Armor(15, "낡은갑옷", ItemType.방어구, 1, "방어력이 5증가합니다.");
            armorStorage = new Dictionary<string, Item>
            {
                [shortarmor.Name] = shortarmor,
                [longarmor.Name] = longarmor
            };

            smallHealingPotion = new Consumable(30, "힐링포션(소)", ItemType.소모품, ConsumableEffect.체력회복, 1, "체력을 30 회복합니다.");

            // 퀘스트용 아이템 추가
            fish = new Consumable(0, "생선", ItemType.소모품, ConsumableEffect.None, 1, "고양이한테 생선을 맡기면 ?");
            glassPiece = new Consumable(0, "유리조각", ItemType.소모품, ConsumableEffect.None, 1, "꽤 큰 유리조각이다.");
            blessing9jo = new Consumable(0, "⟡༺༒9조의 축복༒༻⟡", ItemType.소모품, ConsumableEffect.None, 1, "이제 미르는 누구도 무섭지 않아요 !");
            catnip = new Consumable(0, "캣닢", ItemType.소모품, ConsumableEffect.None, 1, "아가냥이가 세상에서 제일 좋아하는 풀이라고 한다."); 

            // 우유 아이템 추가
            milk = new Milk("우유", 1, ItemType.우유, $"미르가 좋아하는 우유이다! 하지만 락토프리가 아니기에 먹으면 왠지 기분은 좋겠지만 배가 아플 것 같다...");
          
            consumableStorage = new Dictionary<string, Item>
            {
                [smallHealingPotion.Name] = smallHealingPotion,
                [fish.Name] = fish,
                [glassPiece.Name] = glassPiece,
                [blessing9jo.Name] = blessing9jo,
                [catnip.Name] = catnip,
                [milk.Name] = milk
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

        public void EquipItem(Item item)
        {
            Weapon weapon = (Weapon)item;
            player.Atk += weapon.AttackPower;
            GameManager.atkItemCount++;
            ConsoleManager.PrintAnywhere("아이템을 장착했습니다", 2, 23);
        }

        public void ReleaseItem(Item item)
        {
            Weapon weapon = (Weapon)item;
            player.Atk -= weapon.AttackPower;
            GameManager.atkItemCount--;
            GameManager.wearAtkItemName = "";
            ConsoleManager.PrintAnywhere("아이템을 해제했습니다", 2, 23);
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

        public void EquipItem(Item item)
        {
            Armor armor = (Armor)item;
            player.Def += armor.DefensePower;
            GameManager.defItmeCount++;
            ConsoleManager.PrintAnywhere("아이템을 장착했습니다", 2, 23);
        }

        public void ReleaseItem(Item item)
        {
            Armor armor = (Armor)item;
            player.Def -= armor.DefensePower;
            GameManager.defItmeCount--;
            GameManager.wearDefItemName = "";
            ConsoleManager.PrintAnywhere("아이템을 해제했습니다", 2, 23);
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
                        Console.WriteLine($"{player.Name}이(가) {Name}을(를) 사용했습니다.");
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
                    // 먼저 consumableStorage에 있는지 확인
                    if (ItemDataBase.consumableStorage.ContainsKey(item))
                    {
                        Item itemName = ItemDataBase.consumableStorage[item];

                        bool yes = IsConsumable(itemName); // 타입이 소모품인지 체크
                        if (item == itemName.Name && yes)
                        {
                            onlyConsumables[item] = itemName; // 소모품만 저장
                        }
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


    public class Milk : Item
    {
        // [Fields                                                
        public bool HasMilk => invenManager.HasItem(this);                              // 우유 카운트가 인벤토리 내에 1개라도 있으면 true                               
        public bool IsAllMilkCollected => invenManager.HasItem(this) && Counts == 10;   // 우유가 10개 다 모였는지 체크
        public bool IsMilkUsed { get; set; } = false;


        // [Constructor]
        public Milk(string name, int count, ItemType type, string description) : base(name, type, count, description) { }

        // [Methods]
        public bool DrinkMilk()
        {
            if (HasMilk && !IsMilkUsed)
            {
                DrankMilk(); // 우유를 마셨을 때의 효과 적용

                var drinkMilk = new StringBuilder();
                    drinkMilk.AppendLine($"{player.Name}가 우유를 챱챱 마셨습니다!");
                    drinkMilk.AppendLine($"우유를 마신 {player.Name}는 매우 행복해졌습니다! ٩(^ᗜ^ )و ´- (공격력 증가!)");
                    drinkMilk.AppendLine($"하지만 락토프리 우유가 아니기에 알싸해지는 배는 어쩔 수 없군요... ( ˶°ㅁ°) !! (체력 감소!)");

                Console.WriteLine(drinkMilk.ToString());
            }
            else if (!HasMilk)
            {
                Console.WriteLine($"{player.Name}의 소지품에 우유가 없습니다 (｡•́︿•̀｡)");
                return false;
            }
            else if (IsMilkUsed)
            {
                Console.WriteLine($"{player.Name}는 이미 오늘 할당된 양의 우유를 마셨습니다. (•́ ᴖ •̀)");
                Console.WriteLine($"하루에 너무 많은 양의 우유를 먹으면 {player.Name}에게 해로워요!");
                return false;
            }

            return true;
        }
        public void DrankMilk()
        {
            int milkEffects = 5 * player.Level;     // 레벨에 따라 우유 효과 증가

            player.Hp -= milkEffects;               // 그냥 우유를 마셨기에 플레이어 체력 감소
            player.Atk += milkEffects;              // 하지만 우유를 마셔서 행복해진 플레이어의 공격력 증가

            inventory[this.Name].Counts -= 1;       // 사용된 우유 개수 감소

            IsMilkUsed = !IsMilkUsed;               // 우유를 마셨기에 true로 변경
        }
        public bool ForLactoseFreeMilk()
        {
            if (!IsAllMilkCollected)
            {
                Console.WriteLine($"{player.Name}은 아직 락토프리 우유를 얻기 위한 우유의 개수가 부족합니다! (부족한 우유 갯수 : {10 - this.Counts})");
                return false;
            }
            else
            {
                this.Counts = 0;             // 우유 개수 초기화
                inventory.Remove(this.Name); // 인벤토리에서 우유 삭제
                return true;
            }
        }
        public int GetMilkCounts(string itemName)
        {
            if (inventory.ContainsKey(itemName))
            {
                return inventory[itemName].Counts;
            }
            return 0;
        }
        public void CombineGlassPieces(string baseItemName)
        {
            int count = GetMilkCounts(baseItemName);
            if (count >= 5)
            {
                // 유리병 조합 성공
                invenManager.RemoveAll(inventory[baseItemName]); // 기존 유리조각 삭제

                Item GlassBottle = new Item("유리병", ItemType.소모품, 1, "유리조각 5개를 모아 완성된 유리병입니다.");
                invenManager.AddItem(GlassBottle, 1);
                Console.WriteLine($"[{baseItemName}] 5개를 모아 유리병을 완성했습니다!");
            }
            else
            {
                Console.WriteLine($"{baseItemName}이(가) 부족합니다. (현재 {count}/5)");
            }
        }
    }
}
