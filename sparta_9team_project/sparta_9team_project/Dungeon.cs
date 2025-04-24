using System;

namespace sparta_9team_project
{
    public class Dungeon
    {
        private Enimies enimies;
        private static Enemy[] enemies = new Enemy[3];
        private static int[] locationx = { 1, 40, 82 };


        public static void Walk()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            ConsoleManager.ConfigureConsoleSize();

            Random x = new Random();
            int randomx = x.Next(1, 4);
            int hp = PlayerManager.instance.mainPlayer.Hp;
            Hpbar(hp);

            Console.WriteLine();
            for (int i = 0; i < randomx; i++)
            {
                ConsoleManager.PrintCenteredSlow("🌲 미르는 산책중.... 🌲", 55, 2, 60);
                ConsoleManager.PrintCenteredSlow("                       ", 55, 2, 60);
                Thread.Sleep(500);
            }

            //던전 진입 호출
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
            bool win = false;

            Console.Clear();
            DiscoverEnermy();
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
                    break;
                }
                EnemyPhase();
                if (PlayerManager.instance.mainPlayer.Hp <= 0)
                {
                    win = false;
                    break;
                }
            }
            Result(win);
        }


        public static void DiscoverEnermy()
        {
            ConsoleManager.PrintAnywhere("👾앗! 미르의 적을 발견했다! 👾",44, 2);

            Random rand = new Random();
            for (int i = 0; i < 3; i++)
            {
                int typeIndex = rand.Next(0, 3); // 0 ~ 2 (catling, chihuahua, cat)
                enemies[i] = new Enemy((Enemytype)typeIndex); // 여기가 빠졌을 가능성이 높음
            }

            for (int i = 0; i < 3; i++)
            {
                Enemy enemy = enemies[i];

                int infoIndex = Array.FindIndex(Enemyinfos.enemyinfos, info => info.nm == enemy.Name);
                Enemyinfo info = Enemyinfos.enemyinfos[infoIndex];

                ConsoleManager.PrintAnywhere($"HP: [{enemy.Hp}]", locationx[i] + 10, 22);
                ConsoleManager.PrintAnywhere($"레벨: {enemy.Level}, 이름: [{enemy.Name}]", locationx[i], 23);
                ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);
            }

            while (true)
            {
                
                ConsoleManager.PrintAnywhere(">> 0. 전투 시작: ", 49, 27);
                Console.SetCursorPosition(66, 27);
                string choise = Console.ReadLine();
                if (choise == "0")
                {
                    Thread.Sleep(1000);
                    break;
                }
                else
                {
                    ConsoleManager.PrintAnywhere("잘못된 입력입니다.", 44, 27);
                    Thread.Sleep(1000);
                }
            }
        }

        public static void PlayerPhase()
        {
            Console.Clear();
            ConsoleManager.PrintAnywhere("🗡️ 플레이어의 턴입니다! 공격할 적을 선택하세요.",40, 2);
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Enemy enemy = enemies[i];
                int infoIndex = Array.FindIndex(Enemyinfos.enemyinfos, info => info.nm == enemy.Name);
                Enemyinfo info = Enemyinfos.enemyinfos[infoIndex];

                if (enemy.Hp > 0)
                {

                    ConsoleManager.PrintAnywhere($"HP: [{enemy.Hp}]", locationx[i]+10 , 22);
                    ConsoleManager.PrintAnywhere($"[{i + 1}] 레벨: {enemy.Level}, 이름: [{enemy.Name}]", locationx[i], 23);
                }
                ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);
            }

            int choice;
            ConsoleManager.PrintAnywhere(">> 선택: ",49, 27);
            Console.SetCursorPosition(58, 27);
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3 || enemies[choice - 1].Hp <= 0)
            {
                ConsoleManager.PrintAnywhere("잘못된 입력입니다. 다시 선택하세요.", 36, 26);
                ConsoleManager.PrintAnywhere(">> 선택: ", 49, 27);
            }

            int damage = PlayerManager.instance.mainPlayer.Atk;
            enemies[choice - 1].GetDamage(damage);
            
            Thread.Sleep(1000);
        }



        public static void EnemyPhase()
        {
            Console.Clear();
            ConsoleManager.PrintAnywhere("👾 적의 턴입니다! 공격이 시작됩니다...",40, 2);
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Enemy enemy = enemies[i];

                int infoIndex = Array.FindIndex(Enemyinfos.enemyinfos, info => info.nm == enemy.Name);
                Enemyinfo info = Enemyinfos.enemyinfos[infoIndex];
                if (enemy.Hp > 0)
                {
                    ConsoleManager.PrintAnywhere($"HP: [{enemy.Hp}]", locationx[i] + 10, 26);
                    ConsoleManager.PrintAnywhere($"[{i + 1}] 레벨: {enemy.Level}, 이름: [{enemy.Name}]", locationx[i], 28);
                    
                }
                ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);
            }

            for (int i = 0; i < 3; i++)
            {
                Enemy e = enemies[i];
                if (e.Hp <= 0) continue;

                int damage = Math.Max(0, e.Atk - PlayerManager.instance.mainPlayer.Def);
                PlayerManager.instance.mainPlayer.TakeDamage(damage);
                Thread.Sleep(1000);
            }
        }




        public static void Result(bool win)
        {
            Console.Clear();
            if (win)
            {
                ConsoleManager.PrintCentered("🎉 전투에서 승리했습니다! 🎉", 2);
                Console.WriteLine("경험치와 보상을 획득했습니다.");
                //ConsoleManager.PrintAsciiAt(Print.dogImage[1, 73, 5);
                // 경험치나 골드 증가 코드는 여기에 추가 가능
            }
            else
            {
                ConsoleManager.PrintCentered("💀 전투에서 패배했습니다... 💀", 2);
                Console.WriteLine("체력이 0이 되어 전투에서 쓰러졌습니다.");
                //ConsoleManager.PrintAsciiAt(Print.dogImage[2, 73, 5);
            }

            Console.WriteLine("\n>> [Enter]를 눌러 계속...");
            Console.ReadLine();
        }

    }
}
