using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;


namespace sparta_9team_project
{
    public class Dungeon
    {
        private static Enimies dungeonEnemies;
        private Enimies enimies;
        private static Enemy[] enemies = new Enemy[3];
        private static int[] locationx = { 3, 43, 84 };
        private static int currentDungeonType = 1;


        public static void Walking(int dungeonType)
        {
            Console.Clear();
            ConsoleManager.ConfigureConsoleSize();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Random x = new Random();
            int randomx = x.Next(1, 4);
            int hp = PlayerManager.instance.mainPlayer.Hp;
            int maxhp = PlayerManager.instance.mainPlayer.MaxHp;
            var p = PlayerManager.instance.mainPlayer;

            Hpbar(hp, maxhp, 49, 59);
            Console.SetCursorPosition(44, 46);
            ConsoleManager.PrintAnywhere($"Lv. {p.Level} {p.Name} ({p.Job})", 50, 80);
            ConsoleManager.PrintAsciiAt(Print.dogImage[1], 30, 5);

            Console.WriteLine();

            // 난이도 선택에 따른 멘트 변경


            for (int i = 0; i < randomx; i++)
            {
                if (dungeonType == 1)
                {
                    ConsoleManager.PrintCenteredSlow("🌸 미르는 꽃길을 산책하고 있어요...", 42, 2, 60);
                }
                else if (dungeonType == 2)
                {
                    ConsoleManager.PrintCenteredSlow("🌊 한강 바람이 시원하게 불어요...", 42, 2, 60);
                }
                else if (dungeonType == 3)
                {
                    ConsoleManager.PrintCenteredSlow("🌲 뒷산의 어두운 숲을 조심히 걷고 있어요...", 38, 2, 60);
                }
                else if (dungeonType == 4)
                {
                    ConsoleManager.PrintCenteredSlow("        여긴 어디일까.......?      ", 38, 2, 60);
                }

                ConsoleManager.PrintCenteredSlow("                                                         ", 33, 2, 60);
                Thread.Sleep(100);
            }
            // 산책 중 캣닢 드랍 시도
            DropManager.TryGiveCatnip();


            // 산책 끝나고 던전 진입
            EnterDungeon(dungeonType);
        }


        public static void Walk()
        {
            SoundManager.StopBGM();
            SoundManager.PlayBasicBGM();
            Console.Clear();
            ConsoleManager.ConfigureConsoleSize();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            ConsoleManager.PrintAnywhere("🚶 미르의 산책을 떠날 곳을 선택하세요 🚶", 42, 23);
            Console.WriteLine();
            ConsoleManager.PrintAnywhere("1. 집앞 공원 (쉬움)", 52, 25);
            ConsoleManager.PrintAnywhere("2. 한강 공원 (보통)", 52, 26);
            ConsoleManager.PrintAnywhere("3. 뒷산 (어려움)", 54, 27);
            ConsoleManager.PrintAnywhere("4. ??? (???)", 54, 28);
            Console.WriteLine();
            ConsoleManager.PrintAnywhere(">> 선택 (1~3): ", 56, 30);
            Console.SetCursorPosition(62, Console.CursorTop);

            string input = Console.ReadLine();
            int choice;
            bool isValid = int.TryParse(input, out choice);

            if (!isValid || choice < 1 || choice > 4)
            {
                ConsoleManager.PrintCentered("잘못된 입력입니다. 다시 선택하세요.", 40);
                Thread.Sleep(1000);
                Walk();
                return;
            }

            switch (choice)
            {
                case 1:
                    ConsoleManager.PrintAnywhere("🏞️ 집앞 공원으로 출발합니다...", 48, 31);
                    break;
                case 2:
                    ConsoleManager.PrintAnywhere("🏞️ 한강 공원으로 출발합니다...", 48, 31);
                    break;
                case 3:
                    ConsoleManager.PrintAnywhere("🏞️ 뒷산으로 출발합니다...", 52, 31);
                    break;
                case 4:
                    ConsoleManager.PrintAnywhere("🏞️ ???으로 출발합니다...", 52, 31);
                    Thread.Sleep(1000);
                    currentDungeonType = 4;
                    HiddenStage();
                    return;
            }

            Thread.Sleep(1000);
            currentDungeonType = choice;
            Walking(choice);
        }

        public static void Hpbar(int hp, int maxhp, int x, int y)
        {
            int maxBarCount = 5;
            string[] hpview = new string[maxBarCount];
            int presenthp = (int)Math.Ceiling((double)hp / maxhp * maxBarCount);
            presenthp = Math.Max(0, Math.Min(maxBarCount, presenthp));

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

            ConsoleManager.PrintAnywhere($"[{hpbar}] ({hp} / {maxhp})", x, y);

            // 색 복구
            Console.ForegroundColor = originalColor;
        }

        public static void Mpbar(int mp, int maxMp, int x, int y)
        {
            int maxBarCount = 5;
            string[] mpview = new string[maxBarCount];
            int filled = (int)Math.Ceiling((double)mp / maxMp * maxBarCount);
            filled = Math.Max(0, Math.Min(maxBarCount, filled));
            for (int i = 0; i < filled; i++) mpview[i] = "■";
            for (int i = filled; i < maxBarCount; i++) mpview[i] = "□";

            string mpbar = string.Join("", mpview);
            var orig = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            ConsoleManager.PrintAnywhere($"[{mpbar}] ({mp} / {maxMp})", x, y);
            Console.ForegroundColor = orig;
        }






        public static void EnterDungeon(int dungeonType)
        {
            Skills skills = SkillsManager.Instance.PlayerSkills;

            bool win = false;
            Console.Clear();
            if (dungeonType == 4)
            {
                bool hiddenWin = false;

                while (true)
                {
                    enemies = new Enemy[1] { new Enemy(Enemytype.finalboss) };
                    var boss = enemies[0];
                    var info = Enemyinfos.enemyinfos[(int)Enemytype.finalboss];

                    // 플레이어 턴
                    Console.Clear();
                    ConsoleManager.PrintAnywhere("🗡️ 플레이어의 턴입니다! 행동을 선택하세요.", 40, 2);
                    PrintPlayerInfo();

                    ConsoleManager.PrintAsciiAt(info.enepic, locationx[1], 3);
                    ConsoleManager.PrintAnywhere($"Lv. {boss.Level}, 이름: {boss.Name}", locationx[1] + 8, 23);
                    Hpbar(boss.Hp, info.mhp, locationx[1] + 8, 22);

                    ConsoleManager.PrintAnywhere("1. 공격 ", 49, 26);
                    ConsoleManager.PrintAnywhere("2. 스킬 ", 49, 27);
                    ConsoleManager.PrintAnywhere(">> 선택: ", 49, 28);
                    Console.SetCursorPosition(58, 28);

                    var choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        ConsoleManager.PrintAnywhere("               ", 49, 26);
                        ConsoleManager.PrintAnywhere("               ", 49, 27);
                        ConsoleManager.PrintAnywhere("               ", 49, 28);
                        PlayerManager.instance.mainPlayer.DealDamage(boss, PlayerManager.instance.mainPlayer.Atk);
                        Thread.Sleep(1000);
                    }
                    else if (choice == "2")
                    {
                        ConsoleManager.PrintAnywhere("               ", 49, 26);
                        ConsoleManager.PrintAnywhere("               ", 49, 27);
                        ConsoleManager.PrintAnywhere("               ", 49, 28);
                        skills.HandleSkill(PlayerManager.instance.mainPlayer, PlayerManager.instance.mainPlayer.Job, PlayerManager.instance.mainPlayer.skilltree,  enemies);
                        Thread.Sleep(1000);
                    }
                    
                    if (boss.Hp <= 0) 
                    {
                        ConsoleManager.PrintAnywhere("               ", 49, 26);
                        ConsoleManager.PrintAnywhere("               ", 49, 27);
                        ConsoleManager.PrintAnywhere("               ", 49, 28);
                        win = true;
                    }

                    // 보스 턴
                    Console.Clear();
                    ConsoleManager.PrintAsciiAt(info.enepic, locationx[1], 3);
                    ConsoleManager.PrintAnywhere($"Lv. {boss.Level}, 이름: {boss.Name}", locationx[1] + 8, 23);
                    Hpbar(boss.Hp, info.mhp, locationx[1] + 8, 22);
                    ConsoleManager.PrintAnywhere("👾 적의 턴입니다! 공격이 시작됩니다...", 40, 2);
                    var player = PlayerManager.instance.mainPlayer;
                    int dmg = Math.Max(0, boss.Atk - player.Def);
                    player.TakeDamage(dmg);

                    PrintPlayerInfo();
                    Thread.Sleep(1000);
                    if (player.Hp <= 0)
                    { 
                        win = false;
                        Result(hiddenWin);
                        return;
                         
                    }
                }
            }

                var quest = QuestManager.AllQuests.Find(q => q.TITLE == "9, 또 너야 ?");
            if (quest != null && quest.IS_COMPLETED && quest.IS_REWARD_CLAIMED)
            {
                Console.WriteLine("⟡༺༒9조의 축복༒༻⟡을 받았습니다!");
                PlayerManager.instance.mainPlayer.IsInvincible = true; // 엔딩 전 무적 부여
            }

            DiscoverEnemy(dungeonType);
            EncounterManager.SetupEnemies(enemies); // 전투 중인 몬스터 리스트 확인용 함수 추가했습니다 - 황연주

            

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
            // 히든 던전 전용 
            if (difficulty == 4)
            {
                SoundManager.StopBGM();
                SoundManager.PlayLastBGM();
                Console.Clear();
                ConsoleManager.PrintAnywhere("　흐흐흐　여기까지　잘도왔군．．．．　", locationx[1], 2);
                enemies = new Enemy[1] { new Enemy(Enemytype.finalboss) };
                var boss = enemies[0];
                var info = Enemyinfos.enemyinfos[(int)Enemytype.finalboss];
                ConsoleManager.PrintAsciiAt(info.enepic, locationx[1], 3);
                ConsoleManager.PrintAnywhere($"Lv. {boss.Level} {boss.Name}",locationx[1] + 8,25);
                Hpbar(boss.Hp, info.mhp, locationx[1] + 8, 24);
                ConsoleManager.PrintAnywhere(">> [Enter]를 눌러 전투 시작...", 49, 27);
                Console.ReadLine();
                EnterDungeon(4);
                return;
            }

            // 기존 난이도
            SoundManager.StopBGM();
            SoundManager.PlayBattleBGM();
            ConsoleManager.PrintAnywhere("👾앗! 미르의 적을 발견했다! 👾", 48, 2);
            var rand = new Random();

            for (int i = 0; i < 3; i++)
            {
                int typeIndex;
                if (difficulty == 1)
                {
                    typeIndex = rand.Next(0, 3);
                }
                else if (difficulty == 2)
                {
                    typeIndex = rand.Next(2, 5);
                }
                else
                {
                    typeIndex = rand.Next(5, 8);
                }

                enemies[i] = new Enemy((Enemytype)typeIndex);
                var info = Enemyinfos.enemyinfos[typeIndex];
                ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);
                ConsoleManager.PrintAnywhere(
                    $"Lv. {enemies[i].Level} {enemies[i].Name}",
                    locationx[i] + 8,
                    23
                );
                Hpbar(enemies[i].Hp, info.mhp, locationx[i] + 8, 22);
            }
            ConsoleManager.PrintAnywhere(">> [Enter]를 눌러 전투 시작...", 49, 27);
            Console.ReadLine();
        }







        public static void PlayerPhase()
        {
            Skills skills = SkillsManager.Instance.PlayerSkills;
            Console.Clear();
            ConsoleManager.PrintAnywhere("🗡️ 플레이어의 턴입니다! 행동을 선택하세요.", 40, 2);
            Console.WriteLine();
            PrintPlayerInfo();



            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy enemy = enemies[i];
                int infoIndex = Array.FindIndex(Enemyinfos.enemyinfos, info => info.nm == enemy.Name);
                Enemyinfo info = Enemyinfos.enemyinfos[infoIndex];

                if (enemy.Hp > 0)
                {
                    Hpbar(enemy.Hp, info.mhp, locationx[i] + 8, 22);
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
                skills.HandleSkill(PlayerManager.instance.mainPlayer, PlayerManager.instance.mainPlayer.Job, PlayerManager.instance.mainPlayer.skilltree, enemies);
                Thread.Sleep(500);
                PlayerPhase();
            }
        }


        // 적공격하기 
        public static void Attackenemy()
        {
            ConsoleManager.PrintAnywhere("      🗡️ 공격할 적을 선택하세요.                  ", 39, 2);
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
            for (int i = 0; i < 3; i++)
            {
                Enemy enemy = enemies[i];
                if (enemy.Hp <= 0) continue; // 죽은 적 건너뜀
                Console.Clear();
                ConsoleManager.PrintAnywhere("👾 적의 턴입니다! 공격이 시작됩니다...", 40, 2);
                int infoIndex = Array.FindIndex(Enemyinfos.enemyinfos, info => info.nm == enemy.Name);
                Enemyinfo info = Enemyinfos.enemyinfos[infoIndex];
                ConsoleManager.PrintAsciiAt(info.enepic, locationx[i], 6);
                ConsoleManager.PrintAnywhere($"HP: [{enemy.Hp}]", locationx[i] + 10, 22);
                ConsoleManager.PrintAnywhere($"[{i + 1}] 레벨: {enemy.Level}, 이름: [{enemy.Name}]", locationx[i], 23);
                int damage = Math.Max(0, enemy.Atk - PlayerManager.instance.mainPlayer.Def);
                PlayerManager.instance.mainPlayer.TakeDamage(damage);

                PrintPlayerInfo();

                Thread.Sleep(1000);
            }
        }


        public static void Result(bool win)
        {
            Console.Clear();
            if (win)
            {
                // 체력 5 이하로 3마리 남겼는지 체크
                int lowHpEnemyCount = EncounterManager.CountLowHpEnemies();
                if (lowHpEnemyCount >= 3)
                {
                    Console.WriteLine("3아가냥이와 친구가 되었어요 !");

                    var quest = QuestManager.AllQuests.Find(q => q.TITLE == "3아가냥이와 친구가 되었어요 !");

                    if (quest != null && quest.IS_ACCEPTED && !quest.IS_COMPLETED)
                    {
                        quest.CURRENT_COUNT++;  // 달성 수 +1
                        Console.WriteLine("📜 퀘스트 진행도 +1 증가!");

                        // 퀘스트 자동 완료 확인용
                        if (quest.CURRENT_COUNT >= quest.REQUIRED_COUNT)
                        {
                            quest.IS_REWARD_CLAIMED = false;  // 선물 지급 대기
                            Console.WriteLine("📜 퀘스트 완료!");
                            Console.WriteLine(">> [Enter]를 누르세요...");
                            Console.ReadLine();

                            Console.WriteLine("어? 인벤토리에 무언가가?");
                        }
                    }
                }
                ConsoleManager.PrintAnywhere("🎉 전투에서 승리했습니다! 🎉", 45, 2);
                ConsoleManager.PrintAnywhere("경험치와 보상을 획득했습니다.", 45, 4);
                ConsoleManager.PrintAsciiAt(Print.dogImage[1], 37, 5);
                // 경험치나 골드 증가 코드는 여기에 추가 가능
                ConsoleManager.PrintAnywhere(">> [Enter]를 눌러 마을로 돌아가기...", 42, 27);
                Console.ReadLine();
                GameManager.MainScreen();
            }
            else
            {

                ConsoleManager.PrintAnywhere("💀 전투에서 패배했습니다... 💀", 48, 7);
                ConsoleManager.PrintAsciiAt(Print.dogImage[11], 23, 8);
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

            SoundManager.StopBGM();
            SoundManager.PlayDieBGM();  

            int startX = 20;
            int endX = 70;
            int y = 20;

            ConsoleManager.PrintAnywhere("🚑🚑🚑 마을로 실려 가는 중...", 49, 2);
            for (int x = startX; x <= endX; x++)
            {
                Console.Clear();
                ConsoleManager.PrintAsciiAt(Print.dogImage[12], x, y);
                Thread.Sleep(50);
            }
            Thread.Sleep(500);
            PlayerManager.instance.mainPlayer.Hp = PlayerManager.instance.mainPlayer.MaxHp;
            GameManager.MainScreen();
        }



        public static void PrintPlayerInfo()
        {
            Player player = PlayerManager.instance.mainPlayer;

            int left = 0;

            Console.SetCursorPosition(0, 0);
            ConsoleManager.PrintAnywhere("=========================", 0, 0);
            ConsoleManager.PrintAnywhere($"[플레이어 정보]", 0, 1);
            ConsoleManager.PrintAnywhere($"Lv. {player.Level}  {player.Name} ({player.Job})", 0, 2);
            // 체력바
            Console.SetCursorPosition(0, 3);
            Console.Write("HP: ");
            Hpbar(player.Hp, player.MaxHp, 5, 3);
            Console.SetCursorPosition(0, 4);
            Console.Write("HP: ");
            Hpbar(player.Hp, player.MaxHp, 5, 4);
            ConsoleManager.PrintAnywhere("=========================", 0, 5);

            // 현재 던전 위치 표시
            string dungeonName = "";

            if (currentDungeonType == 1)
            {
                dungeonName = "집앞 공원";
            }
            else if (currentDungeonType == 2)
            {
                dungeonName = "한강 공원";
            }
            else if (currentDungeonType == 3)
            {
                dungeonName = "뒷산";
            }
            Console.WriteLine($"현재 위치: {dungeonName}");
        }

        public static void HiddenStage()
        {
            currentDungeonType = 4;
            DiscoverEnemy(4);
        }

    }
}
