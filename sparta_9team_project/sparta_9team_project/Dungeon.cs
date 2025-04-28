using System;

namespace sparta_9team_project
{
    public class Dungeon
    {
        private Player player;
        private List<Enemy> enemies;
        private Random rand = new Random();

        public Dungeon(Player player)
        {
            this.player = player;
        {
    public void EnterDungeon()
        {
            Console.Clear();
            ConsoleManager.PrintCentered("산책 중...", 2);

            // ASCII 아트 자리
            ConsoleManager.PrintAsciiAt("[도트텍스트: 미르 산책 이미지]", 65, 8);

            // 플레이어 정보
            Console.SetCursorPosition(5, 25);
            Console.WriteLine($"이름: {player.Name} | HP: {player.Hp} | Lv: {player.Level}");

            Console.ReadKey();
            DiscoverEnemies();
        }
    public void DiscoverEnemies()
        {
        Console.Clear();
        ConsoleManager.PrintCentered("어디선가 적을 발견했다!", 2);

        // 적 3명 생성
        enemies = new List<Enemy>();
        for (int i = 0; i < 3; i++)
            {
                enemies.Add(new Enemy($"적 {i + 1}", rand.Next(1, 5), rand.Next(10, 30), rand.Next(0, 5), rand.Next(30, 60)));
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = enemies[i];
                Console.SetCursorPosition(10 + (i * 45), 10);
                Console.WriteLine($"이름: {e.Name}");
                Console.SetCursorPosition(10 + (i * 45), 11);
                Console.WriteLine($"Lv: {e.Level} | HP: {e.Hp}");
                // 도트텍스트 자리
                ConsoleManager.PrintAsciiAt($"[도트텍스트: 적 {i + 1}]", 10 + (i * 45), 13);
            }

        Console.ReadKey();
        PlayerTurn();   
        }
        public void PlayerTurn()
        {
            Console.Clear();
            ConsoleManager.PrintCentered("미르의 차례", 2);

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = enemies[i];
                Console.SetCursorPosition(10 + (i * 45), 10);
                Console.WriteLine($"{i + 1}. {e.Name} | HP: {e.Hp} | Lv: {e.Level}");
            }

            ConsoleManager.PrintAsciiAt("[도트텍스트: 플레이어 전투 자세]", 60, 18);
            Console.SetCursorPosition(55, 30);
            Console.WriteLine($"이름: {player.Name} | HP: {player.Hp} | Lv: {player.Level}");

            ConsoleManager.PrintCentered("공격할 적을 선택하세요 (1~3):", 35);
            int choice = int.Parse(Console.ReadLine()) - 1;

            Enemy target = enemies[choice];
            int damage = Math.Max(0, player.Atk - target.Def);
            target.Hp -= damage;

            Console.Clear();
            ConsoleManager.PrintCentered($"[미르의 공격] {target.Name}에게 {damage}의 피해를 입혔습니다!", 3);
            ConsoleManager.PrintAsciiAt("[도트텍스트: 공격 장면]", 60, 10);

            Console.ReadKey();

            if (target.Hp <= 0)
            {
                enemies.RemoveAt(choice);
                ConsoleManager.PrintCentered($"{target.Name} 처치됨!", 20);
                Console.ReadKey();
            }

            if (enemies.Count == 0)
            {
                ShowBattleResult(true);
            }
            else
            {
                EnemyTurn();
            }
        }
        public void EnemyTurn()
        {
            Console.Clear();
            ConsoleManager.PrintCentered("적의 공격!", 2);

            foreach (var enemy in enemies)
            {
                int damage = Math.Max(0, enemy.Atk - player.Def);
                player.Hp -= damage;

                Console.WriteLine($"{enemy.Name}이(가) 미르에게 {damage} 피해!");

                ConsoleManager.PrintAsciiAt("[도트텍스트: 적 공격 중]", 60, 10);
            }

            Console.ReadKey();

            if (player.Hp <= 0)
            {
                ShowBattleResult(false);
            }
            else
            {
                PlayerTurn();
            }
        }
        public void ShowBattleResult(bool win)
        {
            Console.Clear();
            ConsoleManager.PrintCentered("전투 결과", 2);

            if (win)
            {
                ConsoleManager.PrintCentered("승리!", 5);
                ConsoleManager.PrintAsciiAt("[도트텍스트: 승리 장면]", 60, 10);
            }
            else
            {
                ConsoleManager.PrintCentered("패배...", 5);
                ConsoleManager.PrintAsciiAt("[도트텍스트: 패배 장면]", 60, 10);
            }

            Console.SetCursorPosition(50, 25);
            Console.WriteLine($"이름: {player.Name} | HP: {player.Hp} | Lv: {player.Level}");

            Console.ReadKey();
        }
    }
