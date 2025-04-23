using System;

namespace sparta_9team_project
{
    public class Dungeon
    {
        private Enimies enimies;
        public static void Walk()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            ConsoleManager.ConfigureConsoleSize();
            Random x = new Random();
            int ramdomx = x.Next(1, 4);
            int hp = PlayerManager.instance.mainPlayer.Hp;
            Hpbar(hp);

            Console.WriteLine();
            for (int i = 0; i < ramdomx; i++)
            {

                ConsoleManager.PrintCenteredSlow("🌲 미르는 산책중.... 🌲", 55, 2, 60);
                ConsoleManager.PrintCenteredSlow("                       ", 55, 2, 60);
                Thread.Sleep(500);
                
            }
            EnterDungeon();
        }
        
        // 현재체력 >> hp바로 표시
        public static void Hpbar(int hp)
        {
            int level = PlayerManager.instance.mainPlayer.Level;
            string name = PlayerManager.instance.mainPlayer.Name;
            int maxBarCount = 5;
            string[] hpview = new string[maxBarCount];

            int presenthp = hp / 20;

            //현재 hp ■
            for (int i = 0; i < presenthp; i++)
            {
                hpview[i] = "■";
            }

            // 나머지는 □
            for (int i = presenthp; i < maxBarCount; i++)
            {
                hpview[i] = "□";
            }

            string hpbar = string.Join("", hpview);
            ConsoleManager.PrintAsciiAt(Print.dogImage[1], 36, 30);
            Console.WriteLine();
            ConsoleManager.PrintCentered($"hp : [{hpbar}]", 40);
            ConsoleManager.PrintCentered($"1. lv{level} [{name}]", 40);
        }

        public static void EnterDungeon()
        {
            Console.Clear();
            DiscoverEnermy();



        }

        public static void DiscoverEnermy()
        {

            ConsoleManager.PrintCentered("👾 적을 발견했다! 👾", 2);

            Random rand = new Random();
            int[] randomenermy = new int[3];
            int count = 0;

            while (count < 3)
            {
                int randomenermytype = rand.Next(0, 4);
                bool isDuplicate = false;  // 중복 체크
                for (int j = 0; j < count; j++)
                {
                    if (randomenermy[j] == randomenermytype)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                if (!isDuplicate)
                {
                    randomenermy[count] = randomenermytype;
                    count++;
                }
            }
            // 출력 (이름, 레벨, HP)
            for (int i = 0; i < 3; i++)
            {
                Enemyinfo info = Enemyinfos.enemyinfos[randomenermy[i]];
                Console.WriteLine($"이름: {info.nm}, 레벨: {info.level}, HP: {info.hpoint}");
                
            }


            //ConsoleManager.PrintAsciiAt(Print.dogImage[6], 0, 0);
            ////ConsoleManager.PrintAsciiAt(Print.dogImage[7], 0, 0);
            //ConsoleManager.PrintAsciiAt(Print.dogImage[8], 73, 5);

        }

        public void PlayerPhase()
        {
            Console.Clear();
            ConsoleManager.PrintCentered("🗡️ 플레이어의 턴입니다! 공격할 적을 선택하세요.", 2);
            Console.WriteLine();

            for (int i = 0; i < 3; i++)  // 3명 고정
            {
                Enemy enemy = enimies.GetEnemy(i);
                if (enemy != null)
                    Console.WriteLine($"[{i + 1}] 이름: {enemy.Name}, HP: {enemy.Hp}");
            }

            Console.Write(">> 선택: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
                Console.Write(">> 선택: ");
            }

            Enemy target = enimies.GetEnemy(choice - 1);
            int damage = PlayerManager.instance.mainPlayer.Atk;
            PlayerManager.instance.mainPlayer.DealDamage(target, damage);
            Console.WriteLine($"{target.Name}에게 {damage}의 피해를 입혔습니다!");

            Thread.Sleep(1000);
        }

        public void EnemyPhase()
        {
            Console.Clear();
            ConsoleManager.PrintCentered("👾 적의 턴입니다! 공격이 시작됩니다...", 2);
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Enemy e = enimies.GetEnemy(i);
                if (e == null || e.Hp <= 0) continue;

                int damage = Math.Max(0, e.Atk - PlayerManager.instance.mainPlayer.Def);
                PlayerManager.instance.mainPlayer.TakeDamage(damage);
                Console.WriteLine($"{e.Name}이(가) {damage}의 피해를 입혔습니다!");
                Thread.Sleep(1000);
            }
        }


        public void Result(bool win)
        {
            Console.Clear();
            if (win)
            {
                ConsoleManager.PrintCentered("🎉 전투에서 승리했습니다! 🎉", 2);
                Console.WriteLine("경험치와 보상을 획득했습니다.");
                // 경험치나 골드 증가 코드는 여기에 추가 가능
            }
            else
            {
                ConsoleManager.PrintCentered("💀 전투에서 패배했습니다... 💀", 2);
                Console.WriteLine("체력이 0이 되어 전투에서 쓰러졌습니다.");
            }

            Console.WriteLine("\n>> [Enter]를 눌러 계속...");
            Console.ReadLine();
        }

    }
}
