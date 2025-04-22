using System;

namespace sparta_9team_project
{
    public enum JobType
    {
        전사,
        마법사
    }

    public class  PlayerManager
    {
        public static PlayerManager instance = new PlayerManager();
        public Player mainPlayer { get; private set;}
        public void Init(Player player)
        {
            mainPlayer = player;
        }
    }

    public class Player : Character
    {
		// [Fields]
		public string Name { get; set; } = "미르";
        public int Level { get; set; } = 1;
        public int Hp { get; set; }
		public int MaxHp { get; set; } = 100;
		public int Atk { get; set; }
		public int Def { get; set; }
        public JobType Job { get; set; }
        public string Bones { get; set; }


        // [Methods]
        public void ShowStatus()
        {
            Console.Clear();

            Console.WriteLine($"[{Name}의 상태보기]");
            Console.WriteLine($"{Name, 5} ( {Job} )");
            Console.WriteLine("-----------------------");
            Console.WriteLine($"공격력: {Atk}");
            Console.WriteLine($"방어력: {Def}");
            Console.WriteLine($"체 력: {Hp} / {MaxHp}");
            Console.WriteLine($"뼈다귀: {Bones}\n");
            Console.WriteLine(">> [0] 돌아가기");

        }

        public void DealDamage(Enemy mob, int damage)
        {
            Enemy enemy = new Enemy();
            // 적에게 데미지를 주고 해당 적의 Hp를 줄인다.

        }

        public void TakeDamage(int damage)
        {
            Hp = +damage;
            if (Hp < 0) Hp = 0;
            Console.WriteLine($"{Name}는 {damage}만큼의 데미지를 입었습니다!");
            Console.WriteLine($"현재 {Name}의 체력: {Hp}");
        }
    }
}