using System;
using System.Security.Cryptography.X509Certificates;


namespace sparta_9team_project
{
    public class Dungeon
    {
        private static Enimies dungeonEnemies;
        private Enimies enimies;
        private static Enemy[] enemies = new Enemy[3];
        private static int[] locationx = { 1, 40, 82 };



        public static void Walking(int dungeonType)
        {
            Console.Clear();
            ConsoleManager.ConfigureConsoleSize();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Random x = new Random();
            int randomx = x.Next(1, 4);
            int hp = PlayerManager.instance.mainPlayer.Hp;
            int maxhp = PlayerManager.instance.mainPlayer.MaxHp;

            ConsoleManager.PrintAsciiAt(Print.dogImage[1], 36, 30);
            Console.SetCursorPosition(44, 45);
            Hpbar(hp, maxhp);

            Console.WriteLine();

            // 난이도 선택에 따른 멘트 변경
            for (int i = 0; i < randomx; i++)
            {
                if (dungeonType == 1)
                {
                    ConsoleManager.PrintCenteredSlow("🌸 미르는 꽃길을 산책하고 있어요...", 55, 2, 60);
                }
                else if (dungeonType == 2)
                {
                    ConsoleManager.PrintCenteredSlow("🌊 한강 바람이 시원하게 불어요...", 55, 2, 60);
                }
                else if (dungeonType == 3)
                {
                    ConsoleManager.PrintCenteredSlow("🌲 뒷산의 어두운 숲을 조심히 걷고 있어요...", 55, 2, 60);
                }

                ConsoleManager.PrintCenteredSlow("                                                      ", 55, 2, 60);
                Thread.Sleep(500);
            }

            // 산책 끝나고 던전 진입
            EnterDungeon(dungeonType);
        }


        public static void Walk()
        {
            Console.Clear();
            ConsoleManager.ConfigureConsoleSize();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            ConsoleManager.PrintCentered("🚶 미르의 산책을 떠날 곳을 선택하세요 🚶", 40);
            Console.WriteLine();
            ConsoleManager.PrintCentered("1. 집앞 공원 (쉬움)", 40);
            ConsoleManager.PrintCentered("2. 한강 공원 (보통)", 40);
            ConsoleManager.PrintCentered("3. 뒷산 (어려움)", 40);
            Console.WriteLine();
            ConsoleManager.PrintCentered(">> 선택 (1~3): ", 40);
            Console.SetCursorPosition(62, Console.CursorTop);

            string input = Console.ReadLine();
            int choice;
            bool isValid = int.TryParse(input, out choice);

            if (!isValid || choice < 1 || choice > 3)
            {
                ConsoleManager.PrintCentered("잘못된 입력입니다. 다시 선택하세요.", 40);
                Thread.Sleep(1000);
                Walk(); // 다시 선택
                return;
            }

            switch (choice)
            {
                case 1:
                    ConsoleManager.PrintCentered("🏞️ 집앞 공원으로 출발합니다...", 40);
                    break;
                case 2:
                    ConsoleManager.PrintCentered("🏞️ 한강 공원으로 출발합니다...", 40);
                    break;
                case 3:
                    ConsoleManager.PrintCentered("🏞️ 뒷산으로 출발합니다...", 40);
                    break;
            }

            Thread.Sleep(1000);
            Walking(choice);
        }

        public static void Hpbar(int hp, int maxhp)
        {
            int maxBarCount = 5;
            string[] hpview = new string[maxBarCount];
            int presenthp = (int)Math.Ceiling((double)hp / maxhp * maxBarCount);
            for (int i = 0; i < presenthp; i++)
            {
                hpview[i] = "■";
            }

            for (int i = presenthp; i < maxBarCount; i++)
            {
                hpview[i] = "□";
            }

            string hpbar = string.Join("", hpview);
            var originalColor = Console.ForegroundColor;
            double hpRate = (double)hp / maxhp;

            // 단계별 색 설정
            if (hpRate < 0.3)
            {
                Console.ForegroundColor = ConsoleColor.Red; // 30% 미만 빨간색
            }
            else if (hpRate < 0.5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow; // 30~50% 노란색
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White; // 50% 이상 흰색
            }
            Console.Write($"[{hpbar}]");

            // 색 복구
            Console.ForegroundColor = originalColor;
        }



        public static void EnterDungeon(int dungeonType)
        {
            Console.Clear();
            DiscoverEnemy(dungeonType);

            bool win = false;

            while (true)
            {
                PlayerPhase();

                bool enemyAllDead = true;
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].Hp > 0)
                    {
                        enemyAllDead = false;
                        break;
                    }
                }
                if (enemyAllDead)
                {
                    win = true;
                    Result(win);
                    break;
                }

                EnemyPhase();
                if (PlayerManager.instance.mainPlayer.Hp <= 0)
                {
                    win = false;
                    Result(win);
                    break;
                }
            }
        }

        public static void DiscoverEnemy(int difficulty)
        {
            ConsoleManager.PrintAnywhere("👾앗! 미르의 적을 발견했다! 👾", 44, 2);

            Random rand = new Random();
            dungeonEnemies = new Enimies(3);

            //던전 난이도별 에너미 배치
            for (int i = 0; i < 3; i++)
            {
                int typeIndex = 0;

                if (difficulty == 1) // 집앞 공원
                {
                    typeIndex = rand.Next(0, 3); // 새끼고양이, 치와와, 고양이
                }
                else if (difficulty == 2) // 한강공원
                {
                    typeIndex = rand.Next(2, 5); // 고양이, 허스키, 오토바이
                }
                else if (difficulty == 3) // 뒷산
                {
                    typeIndex = rand.Next(3, 5); // 허스키, 오토바이
                }

                Enemyinfo info = Enemyinfos.enemyinfos[typeIndex];

                ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);
                Console.SetCursorPosition(locationx[i] + 5, 22);
                Hpbar(info.hpoint, info.mhp);

            }

            while (true)
            {
                ConsoleManager.PrintAnywhere(">> 0. 전투 시작: ", 49, 27);
                Console.SetCursorPosition(66, 27);
                string choice = Console.ReadLine();
                if (choice == "0")
                {
                    Thread.Sleep(1000);
                    break;
                }
                else
                {
                    ConsoleManager.PrintAnywhere("잘못된 입력입니다.", 49, 27);
                    Thread.Sleep(1000);
                }
            }
        }



        public static void PlayerPhase()
        {
            Console.Clear();
            ConsoleManager.PrintAnywhere("🗡️ 플레이어의 턴입니다! 행동을 선택하세요.", 40, 2);
            Console.WriteLine();



            for (int i = 0; i < 3; i++)
            {
                Enemy enemy = enemies[i];
                int infoIndex = Array.FindIndex(Enemyinfos.enemyinfos, info => info.nm == enemy.Name);
                Enemyinfo info = Enemyinfos.enemyinfos[infoIndex];

                if (enemy.Hp > 0)
                {

                    ConsoleManager.PrintAnywhere($"HP: [{enemy.Hp}]", locationx[i] + 10, 22);
                    ConsoleManager.PrintAnywhere($"[{i + 1}] 레벨: {enemy.Level}, 이름: [{enemy.Name}]", locationx[i], 23);
                }
                else if (enemy.Hp <= 0)
                {
                    var originalColor = Console.ForegroundColor;

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    ConsoleManager.PrintAnywhere($"[{i + 1}] 레벨: {enemy.Level}, 이름: [{enemy.Name}]", locationx[i], 23);
                    ConsoleManager.PrintAnywhere($"[쓰 러 짐]", locationx[i] + 10, 22);

                    Console.ForegroundColor = originalColor;

                }
                ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);
            }

            ConsoleManager.PrintAnywhere("1. 공격 ", 49, 26);
            ConsoleManager.PrintAnywhere("2. 스킬 ", 49, 27);
            ConsoleManager.PrintAnywhere(">> 선택: ", 49, 28);
            Console.SetCursorPosition(58, 28);
            string Playerchoice = Console.ReadLine();

            bool isNumber = int.TryParse(Playerchoice, out int choice);

            if (!isNumber || choice < 1 || choice > 3 || enemies[choice - 1].Hp <= 0)
            {
                ConsoleManager.PrintAnywhere("                                                 ", 36, 26);
                ConsoleManager.PrintAnywhere("                                                 ", 36, 27);
                ConsoleManager.PrintAnywhere("                                                 ", 36, 28);
                ConsoleManager.PrintAnywhere("잘못된 입력입니다. 다시 선택하세요.", 36, 26);
                ConsoleManager.PrintAnywhere("                                                 ", 36, 26);
                ConsoleManager.PrintAnywhere(">> 선택: ", 49, 27);
                Console.SetCursorPosition(58, 27);
                PlayerPhase();
            }
            else if (choice == 1)
            {
                ConsoleManager.PrintAnywhere("                                                 ", 36, 26);
                ConsoleManager.PrintAnywhere("                                                 ", 36, 27);
                ConsoleManager.PrintAnywhere("                                                 ", 36, 28);
                Attackenemy();
            }
            else if (choice == 2)
            {
                Skill();
            }
        }

        // 스킬 (임시)
        public static void Skill ()
        {
            ConsoleManager.PrintAnywhere("                                                 ", 36, 26);
            ConsoleManager.PrintAnywhere("                                                 ", 36, 27);
            ConsoleManager.PrintAnywhere("                                                 ", 36, 28);
            ConsoleManager.PrintAnywhere("미구현입니다.", 50, 26);
            ConsoleManager.PrintAnywhere(">> [Enter]를 눌러 돌아가기...", 42, 27);
            Console.ReadLine();
            PlayerPhase();
        }


        // 적공격하기 
        public static void Attackenemy()
        {
            ConsoleManager.PrintAnywhere("🗡️ 공격할 적을 선택하세요.", 40, 2);
            PrintPlayerInfo();
            int choice;
            ConsoleManager.PrintAnywhere(">> 선택: ", 49, 27);
            Console.SetCursorPosition(58, 27);

            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3 || enemies[choice - 1].Hp <= 0)
            {
                ConsoleManager.PrintAnywhere("잘못된 입력입니다. 다시 선택하세요.", 36, 26);
                ConsoleManager.PrintAnywhere("                                                 ", 36, 26);
                ConsoleManager.PrintAnywhere(">> 선택: ", 49, 27);
                Console.SetCursorPosition(58, 27);
            }

            int damage = PlayerManager.instance.mainPlayer.Atk;
            PlayerManager.instance.mainPlayer.DealDamage(enemies[choice - 1], damage);
          
            Thread.Sleep(1000);
        }


        public static void EnemyPhase()
        {
            Console.Clear();
            ConsoleManager.PrintAnywhere("👾 적의 턴입니다! 공격이 시작됩니다...", 40, 2);
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Enemy enemy = enemies[i];

                int infoIndex = Array.FindIndex(Enemyinfos.enemyinfos, info => info.nm == enemy.Name);
                Enemyinfo info = Enemyinfos.enemyinfos[infoIndex];
                if (enemy.Hp > 0)
                {
                    ConsoleManager.PrintAnywhere($"HP: [{enemy.Hp}]", locationx[i] + 10, 22);
                    ConsoleManager.PrintAnywhere($"[{i + 1}] 레벨: {enemy.Level}, 이름: [{enemy.Name}]", locationx[i], 23);
                    ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);

                    int damage = Math.Max(0, enemy.Atk - PlayerManager.instance.mainPlayer.Def);
                    PlayerManager.instance.mainPlayer.TakeDamage(damage);
                    PrintPlayerInfo();
                    Thread.Sleep(1000);
                }
            }
        }




        public static void Result(bool win)
        {
            Console.Clear();
            if (win)
            {
                ConsoleManager.PrintAnywhere("🎉 전투에서 승리했습니다! 🎉",40 , 2);
                ConsoleManager.PrintAnywhere("경험치와 보상을 획득했습니다.", 40, 4);
                ConsoleManager.PrintAsciiAt(Print.dogImage[1], 37, 5);
                // 경험치나 골드 증가 코드는 여기에 추가 가능
                ConsoleManager.PrintAnywhere(">> [Enter]를 눌러 마을로 돌아가기...",42,27);
                Console.ReadLine();
                GameManager.MainScreen();
                
            }
            else
            {
                ConsoleManager.PrintAnywhere("💀 전투에서 패배했습니다... 💀",45, 7);
                ConsoleManager.PrintAsciiAt(Print.dogImage[11], 35, 8);
                ConsoleManager.PrintAnywhere(">> [Enter]를 눌러 마을로 돌아가기...", 42, 50);
                Console.SetCursorPosition(49, 51);
                Console.ReadLine();
                Die();
            }


        }

        public static void Die()
        {
            Console.Clear();
            ConsoleManager.ConfigureConsoleSize();

            int startX = 70; 
            int endX = 10;  
            int y = 20;      

            for (int x = startX; x >= endX; x--)
            {
                Console.Clear();
                ConsoleManager.PrintAsciiAt(Print.dogImage[12], x, y); 
                Thread.Sleep(50); 
            }

            // 마을로 실려가는 연출
            for (int i = 0; i < 3; i++)
            {
                ConsoleManager.PrintCenteredSlow("🚑🚑🚑 마을로 실려 가는 중... ", 49, 2, 30);
                ConsoleManager.PrintCenteredSlow("                                ", 49, 2, 30);
                Thread.Sleep(500);
            }

            // 체력 복구 후 메인 화면 복귀
            PlayerManager.instance.mainPlayer.Hp = PlayerManager.instance.mainPlayer.MaxHp;
            GameManager.MainScreen();
        }


        public static void PrintPlayerInfo(int dungeonType)
        {
            Player player = PlayerManager.instance.mainPlayer;

            int left = 0;
            int bottom = Console.WindowHeight - 8; //밑에 2줄 추가

            Console.SetCursorPosition(left, bottom);
            Console.WriteLine("=========================");
            Console.WriteLine($"[플레이어 정보]");
            Console.WriteLine($"Lv. {player.Level}  {player.Name} ({player.Job})");

            // 체력바
            Console.Write("HP: ");
            Hpbar(player.Hp, player.MaxHp);
            Console.WriteLine($" {player.Hp} / {player.MaxHp}");

            Console.WriteLine("=========================");

            // 현재 던전 위치 표시
            string dungeonName = "";

            if (dungeonType == 1)
            {
                dungeonName = "집앞 공원";
            }
            else if (dungeonType == 2)
            {
                dungeonName = "한강 공원";
            }
            else if (dungeonType == 3)
            {
                dungeonName = "뒷산";
            }
            Console.WriteLine($"현재 위치: {dungeonName}");
        }


    }
}
