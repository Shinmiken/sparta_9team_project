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
            Console.Clear();
            DiscoverEnermy();




        }

        public static void DiscoverEnermy()
        {
            ConsoleManager.PrintCentered("👾 적을 발견했다! 👾", 2);

            Random rand = new Random();
            Enemy[] enemies = new Enemy[3];

            for (int i = 0; i < 3; i++)
            {
                int typeIndex = rand.Next(0, 3); // catling(0), chihuahua(1), cat(2)
                enemies[i] = new Enemy((Enemytype)typeIndex);
            }

            for (int i = 0; i < 3; i++)
            {
                Enemy e = enemies[i];
                string name = e.Name;

                Enemytype type = (Enemytype)Enum.Parse(typeof(Enemytype), name switch
                {
                    "새끼고양이" => "catling",
                    "치와와" => "chihuahua",
                    "고양이" => "cat",
                    _ => "catling"  // 기본값 fallback
                });

                Enemyinfo info = Enemyinfos.enemyinfos[(int)type];
                int[] locationx = [0, 40, 83]; // 위치 값 저장
                
                ConsoleManager.PrintAnywhere($"[{i + 1}] 이름: {info.nm}, 레벨: {info.level}, HP: {info.hpoint}", locationx[i],25);
                ConsoleManager.PrintAsciiAt(Print.dogImage[6], 0, 11);
                ConsoleManager.PrintAsciiAt(Print.dogImage[7], 40, 9);
                ConsoleManager.PrintAsciiAt(Print.dogImage[8], 83, 11);
            }
        }





        public void PlayerPhase()
        {
            Console.Clear();
            ConsoleManager.PrintCentered("🗡️ 플레이어의 턴입니다! 공격할 적을 선택하세요.", 2);
            Console.WriteLine();

            for (int i = 1; i <= 3; i++)
            {
                Enemy enemy = new Enemy(Enemytype.cat);
                if (enimies.GetEnemyInfo(i, ref enemy) && enemy.Hp > 0)
                {
                    Console.WriteLine($"[{i}] 이름: {enemy.Name}, HP: {enemy.Hp}");
                }
            }

            int choice;
            Console.Write(">> 선택: ");
            Enemy tempEnemy = new Enemy(Enemytype.cat);  
            while (!int.TryParse(Console.ReadLine(), out choice) || !enimies.GetEnemyInfo(choice, ref tempEnemy))
            {
                Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
                Console.Write(">> 선택: ");
            }

            Enemy selected = new Enemy(Enemytype.cat);
            if (enimies.GetEnemyInfo(choice, ref selected))
            {
                int damage = PlayerManager.instance.mainPlayer.Atk;
                enimies.EnemygetDamage(choice, damage);
                Console.WriteLine($"{selected.Name}에게 {damage}의 피해를 입혔습니다!");
            }

            Thread.Sleep(1000);
        }

        public void EnemyPhase()
        {
            Console.Clear();
            ConsoleManager.PrintCentered("👾 적의 턴입니다! 공격이 시작됩니다...", 2);
            Console.WriteLine();

            for (int i = 1; i <= 3; i++)
            {
                Enemy enemy = new Enemy(Enemytype.cat);
                if (enimies.GetEnemyInfo(i, ref enemy) && enemy.Hp > 0)
                {
                    int damage = Math.Max(0, enemy.Atk - PlayerManager.instance.mainPlayer.Def);
                    PlayerManager.instance.mainPlayer.TakeDamage(damage);
                    Console.WriteLine($"{enemy.Name}이(가) {damage}의 피해를 입혔습니다!");
                    Thread.Sleep(1000);
                }
            }
        }


        public void Result(bool win)
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
