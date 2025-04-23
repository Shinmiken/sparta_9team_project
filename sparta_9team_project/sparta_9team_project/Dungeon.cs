using System;

namespace sparta_9team_project
{
    public class Dungeon
    {
        public static void Walk()
        {
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
                break;
            }
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
            int[] randomenermy = new int[3]; // 3개만 뽑을 거니까 3개로 설정
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
                // 각 몬스터별 자리 지정해서 ConsoleManager.PrintAnywhere로 그 밑에 위 정보 내보내야함
            }

            ////Enermy가 3마리 들어가려면.. 칸이 너무 부족합니다.. 대책 필요..
            ////아스키 아트 크기를 줄이기 or 콘솔 창 더 키우기
            //ConsoleManager.PrintAsciiAt(Print.dogImage[6], 0, 0);
            ////ConsoleManager.PrintAsciiAt(Print.dogImage[7], 0, 0);
            //ConsoleManager.PrintAsciiAt(Print.dogImage[8], 73, 5);

        }

        public void PlayerPhase()
        {
            Console.Clear();

        }
        public void EnemyPhase()
        {
            Console.Clear();

        }
        public void ShowBattleResult(bool win)
        {
            Console.Clear();

            if (win)
            {

            }
            else
            {

            }

        }
    }
}
