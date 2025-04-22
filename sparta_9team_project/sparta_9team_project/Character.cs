using System;

namespace sparta_9team_project
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }
        
        public Character(int Level, string Name, int Atk, int Def, int Hp, int Gold)
        {
            level = Level;
            name = Name;
            atk = Atk;
            def = Def;
            hp = Hp;
            gold = Gold;
        }

        public void ShowStatus()
        {
            Console.WriteLine();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv. {Level:00}");
            Console.WriteLine($"{Name} ( {Job} )");
            Console.WriteLine($"공격력 : {Atk}");
            Console.WriteLine($"방어력 : {Def}");
            Console.WriteLine($"체 력 : {Hp}");
            Console.WriteLine($"Gold : {Gold} G");
            Console.WriteLine();
            Console.WriteLine("[0. 나가기]");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
        }
        public void ShowStatusWithExit()
        {
            bool isRunning = true;
            while (isRunning)
            {
                ShowStatus();
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.WriteLine("상태 보기를 종료합니다.");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }
    }
}
