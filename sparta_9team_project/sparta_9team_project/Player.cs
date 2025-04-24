using System;

namespace sparta_9team_project
{
    public enum JobType
    {
        전사,
        마법사
    }

    // [PlayerManager] 클래스 - 싱글톤 패턴으로 Player 객체를 관리
    public class PlayerManager
    {
        public static PlayerManager instance = new PlayerManager();
        public Player mainPlayer { get; private set; }
        public void Init(Player player)
        {
            mainPlayer = player;
        }
    }

    // [Player] 클래스 - 플레이어 캐릭터
    public class Player : Character
    {
		// [Fields]

		public int MaxHp { get; set; } = 100;
	    public JobType Job { get; set; }
        public string Bones { get; set; }

        // [Constructor]
        public Player (string name, int level, int hp, int maxHp, int atk, int def, JobType job, string bones) : base(name, level, atk, def, hp, maxHp) // 부모 클래스인 Character의 생성자 호출
        {
            MaxHp = maxHp;
            Job = job;
            Bones = bones;
        }


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
            Random random = new Random();

            int tenPercent = (int)Math.Ceiling(damage * 0.1);   // ±10% 오차
            int randomDmgMin =  damage - tenPercent;            // 최소 데미지
            int randomDmgMax =  damage + tenPercent;            // 최대 데미지
            damage = random.Next(randomDmgMin, randomDmgMax + 1);  // 랜덤 데미지

            mob.GetDamage(damage); // 적의 HP에 데미지 적용

            ConsoleManager.PrintAnywhere($"{Name}는 {mob.Name}에게 {damage}의 댕미지를 입혔습니다!",40,25);
            ConsoleManager.PrintAnywhere($"{mob.Name}의 HP가 {mob.Hp} 남았습니다.",46,27);
        }

        public void TakeDamage(int damage)
        {
            Hp -=damage;
            if (Hp < 0) { Hp = 0; }

            ConsoleManager.PrintAnywhere($"{Name}는 {damage}만큼의 댕미지를 입었습니다!",40,25);
            ConsoleManager.PrintAnywhere($"현재 {Name}의 체력: {Hp}",46,27);
        }
    }
}