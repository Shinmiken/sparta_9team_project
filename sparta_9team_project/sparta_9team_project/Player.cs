using System;
using System.Text;

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
   	    public bool IsInvincible { get; set; } = false; // 최종 보스 무적 처리 관련 함수
		// [Fields]

		public int MaxHp { get; set; } = 100;
	    public JobType Job { get; set; }
        public string Bones { get; set; }
        public int Exp { get; set; } = 0;       // 경험치
        public int MaxExp { get; set; } = 100;  // 최대 경험치
        public int Mp { get; set; } = 100;      // 마나
        public int MaxMp { get; set; } = 100;   // 최대 마나
        public int Loss { get; set; } = 0;
        public int ImageType { get; set; }
        public SkillData[] skilltree { get; set; }

	    private List<string> usedItems = new List<string>();    // 사용한 아이템 이름 저장용 리스트
 
        InventoryManager inventory = InventoryManager.Instance; // 인벤토리 매니저의 플레이어 인벤토리

        // [Constructor]
        public Player (string name, int level, int hp, int maxHp, int mp, int MaxMp, int atk, int def, JobType job, string bones, int imageType) : base(name, level, atk, def, hp, maxHp) // 부모 클래스인 Character의 생성자 호출
        {
            MaxHp = maxHp;
            Job = job;
            Bones = bones;
            ImageType = imageType;
        }


        // [Methods]
        public void DealDamage(Enemy mob, int damage)
        {
            int totalDamage = Randomize(0.1f, damage);  // ±10% 오차
            mob.GetDamage(totalDamage);                      // 적의 HP에 데미지 적용

            ConsoleManager.PrintAnywhere($"{Name}는 {mob.Name}에게 {totalDamage}의 댕미지를 입혔습니다!",40,25);
            ConsoleManager.PrintAnywhere($"{mob.Name}의 HP가 {mob.Hp} 남았습니다.",46,27);
        }
        public void TakeDamage(int damage)
        {
	        if (IsInvincible)
    	    {
           	 Console.WriteLine("미르는 갑자기 힘이 넘쳐남을 느꼈다!");
            	 return;
    	    }	
	 
            Hp -= damage;
            if (Hp < 0) { Hp = 0; }

            ConsoleManager.PrintAnywhere($"{Name}는 {damage}만큼의 댕미지를 입었습니다!",40,25);
            ConsoleManager.PrintAnywhere($"현재 {Name}의 체력: {Hp}",46,27);
        }
	    public void UseItem(string itemName)
	{
   		usedItems.Add(itemName);
   		Console.WriteLine($"{itemName}을(를) 사용했습니다.");
	}
	    public bool HasUsedItem(string itemName)
	{
  		return usedItems.Contains(itemName);
	}
        public void GainExp(int exp, JobType job)
        {
            Exp += exp;
            if (Exp >= MaxExp)
            {
                LevelUp(job);
                Exp = Exp - MaxExp; // 남은 경험치
            }
        }   
        public void LevelUp(JobType jobType)
        {
            // 레벨업 시 선택지 (공격력, 방어력, 체력 증가 중 택 1)
            if (jobType == JobType.전사)
            {
                var LevelUpMessage01 = new StringBuilder();
                LevelUpMessage01.AppendLine($"나이스! {Name}가 한 걸음 더 성장했습니다! {Level - 1} >> {Level}");
                LevelUpMessage01.AppendLine($"{Name}의 성장을 선택해주세요! ૮ ˶ᵔ ᵕ ᵔ˶ ა");
                LevelUpMessage01.AppendLine($"-----------------------------------------------------------------");
                LevelUpMessage01.AppendLine($"1. 댕댕파워 증가 ");
                LevelUpMessage01.AppendLine($"2. 댕댕방어 증가 :: {Name}는 {Job}이기에 해당 수치가 더 강해집니다!");
                LevelUpMessage01.AppendLine($"3. 댕댕체력 증가 :: {Name}는 {Job}이기에 해당 수치가 더 강해집니다!");
                LevelUpMessage01.AppendLine($"4. 댕댕기력 증가");
                LevelUpMessage01.AppendLine($"-----------------------------------------------------------------");
                LevelUpMessage01.Append($">>");

                Console.WriteLine(LevelUpMessage01.ToString());
            }
            else if (jobType == JobType.마법사)
            {
                var LevelUpMessage02 = new StringBuilder();
                LevelUpMessage02.AppendLine($"나이스! {Name}가 한 걸음 더 성장했습니다! {Level - 1} >> {Level}");
                LevelUpMessage02.AppendLine($"{Name}의 성장을 선택해주세요! ૮ ˶ᵔ ᵕ ᵔ˶ ა");
                LevelUpMessage02.AppendLine($"-----------------------------------------------------------------");
                LevelUpMessage02.AppendLine($"1. 댕댕파워 증가 :: {Name}는 {Job}이기에 해당 수치가 더 강해집니다!");
                LevelUpMessage02.AppendLine($"2. 댕댕방어 증가");
                LevelUpMessage02.AppendLine($"3. 댕댕체력 증가");
                LevelUpMessage02.AppendLine($"4. 댕댕기력 증가 :: {Name}는 {Job}이기에 해당 수치가 더 강해집니다!");
                LevelUpMessage02.AppendLine($"-----------------------------------------------------------------");
                LevelUpMessage02.Append($">>");

                Console.WriteLine(LevelUpMessage02.ToString());
            }            

            bool isDecided = false;
            while (!isDecided)
            {
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        if (jobType == JobType.전사)
                        {
                            Atk += Randomize(3, 6);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕파워가 증가했습니다!", 40, 25);
                            isDecided = true;
                        }
                        else if (jobType == JobType.마법사)
                        {
                            Atk += Randomize(5, 8);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕파워가 증가했습니다! (슈퍼파워업!!)", 40, 25);
                        }
                        break;
                    case "2":
                        if (jobType == JobType.전사)
                        {
                            Def += Randomize(5, 8);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕방어가 증가했습니다! (슈퍼파워업!!)", 40, 25);
                            isDecided = true;
                        }
                        else if (jobType == JobType.마법사)
                        {
                            Def += Randomize(3, 6);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕방어가 증가했습니다!", 40, 25);
                            isDecided = true;
                        }
                        break;
                    case "3":
                        if (jobType == JobType.전사)
                        {
                            MaxHp += Randomize(15, 20);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕체력이 증가했습니다! (슈퍼파워업!!)", 40, 25);
                            isDecided = true;
                        }
                        else if (jobType == JobType.마법사)
                        {
                            MaxHp += Randomize(10, 15);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕체력이 증가했습니다!", 40, 25);
                            isDecided = true;
                        }
                        break;
                    case "4":
                        if (jobType == JobType.전사)
                        {
                            MaxMp += Randomize(10, 15);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕기력이 증가했습니다!", 40, 25);
                            isDecided = true;
                        }
                        else if (jobType == JobType.마법사)
                        {
                            MaxMp += Randomize(15, 20);
                            ConsoleManager.PrintAnywhere($"{Name}의 댕댕기력이 증가했습니다! (슈퍼파워업!!)", 40, 25);
                            isDecided = true;
                        }
                        break;
                    default:
                        ConsoleManager.PrintAnywhere("잘못된 입력입니다. (1 ~ 3에서 선택해주세요!)", 40, 25);
                        break;
                }
            }

            HpMpMax();                      // 레벨업 시 체력과 마나를 최대치로 회복
            MaxExp = Exp * (Level * Level); // 레벨업 시 최대 경험치 증가
        }    
        public void HpMpMax()
        {
            // 체력과 마나를 최대치로 회복
            Hp = MaxHp;
            Mp = MaxMp;
            ConsoleManager.PrintAnywhere($"{Name}의 체력과 기력이 최대치로 회복되었습니다!", 40, 25);
        }
        public SkillData[] SkillTree(JobType type)
        {
            SkillRepository skillRepository = SkillRepository.Instance;
            if (type == JobType.전사)
            {
                skilltree = skillRepository.WarriorSkills;
            }
            else if (type == JobType.마법사)
            {
                skilltree = skillRepository.MagicSkills;
            }

            return skilltree;
        }

        // [Methods] - Overloading
        public int Randomize(int min, int max)                   // General Randomize
        {
            Random random = new Random();

            int randomValue = random.Next(min, max + 1);
            return randomValue;
        }
        public int Randomize(float percent, int damage)          // Randomize 데미지
        {
            Random random = new Random();
            int totalPercent = (int)Math.Ceiling(damage * (float)percent);
            int minValue = damage - totalPercent;
            int maxValue = damage + totalPercent;
            int randomValue = random.Next(minValue, maxValue + 1);

            return randomValue;

        }
    }
}
