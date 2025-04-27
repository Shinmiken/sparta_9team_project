using System;
using System.Threading;

namespace sparta_9team_project
{

    public enum SkillType
    {
        Fart,           // 방구뀌기 (지속 데미지)
        Bite,           // 물기
        ArcaneBolt,     // 비전 화살
        IceSpike,       // 얼음 창
        MeteorStrike,   // 운석 강하
        PowerSlash,     // 강타
        ComboSlash,     // 연속베기
        EarthShatter    // 대지 분쇄
    }

    public struct SkillData
    {
        public SkillType Type { get; }
        public string Name { get; }
        public double DamageRatio { get; }
        public int ManaCost { get; }

        public SkillData(SkillType type, string name, double damageRatio, int manaCost)
        {
            Type = type;
            Name = name;
            DamageRatio = damageRatio;
            ManaCost = manaCost;
        }
    }


    public static class SkillRepository
    {
        public static readonly SkillData[] All = new[]
        {
            new SkillData(SkillType.Fart, "방구뀌기",       0.3,  10), 
            new SkillData(SkillType.Bite, "물기",           0.7,  12),
            new SkillData(SkillType.ArcaneBolt, "비전 화살", 1.2,  20),
            new SkillData(SkillType.IceSpike, "얼음 창",     1.5,  30),
            new SkillData(SkillType.MeteorStrike, "운석 강하",2.5, 50),
            new SkillData(SkillType.PowerSlash,  "강타",     1.1,  15),
            new SkillData(SkillType.ComboSlash,  "연속베기", 0.8,  25),
            new SkillData(SkillType.EarthShatter, "대지 분쇄",2.2, 45)
        };
    }

    public static class Skills
    {
        public static void UseSkill(Player player, Enemy target, SkillType type)
        {
            var info = Array.Find(SkillRepository.All, s => s.Type == type);
            if (player.Mp < info.ManaCost)
            {
                Console.WriteLine("마나가 부족합니다.");
                Thread.Sleep(800);
                return;
            }
            player.Mp -= info.ManaCost;

            // 방구뀌기: 3턴 지속 데미지
            if (type == SkillType.Fart)
            {
                Console.WriteLine($"{player.Name}가 {info.Name}을 시전하여 지속 피해를 시도합니다.");
                Thread.Sleep(500);
                for (int tick = 1; tick <= 3; tick++)
                {
                    int dmg = (int)Math.Ceiling(player.Atk * info.DamageRatio);
                    Console.WriteLine($"Tick {tick}: {target.Name}에게 {dmg} 피해");
                    player.DealDamage(target, dmg);
                    Thread.Sleep(600);
                }
            }
            // 연속베기 로직
            else if (type == SkillType.ComboSlash)
            {
                var rand = new Random();
                int hits = rand.Next(2, 5);
                Console.WriteLine($"{player.Name}가 {info.Name}으로 {hits}회 연속 공격합니다.");
                Thread.Sleep(500);
                for (int i = 0; i < hits; i++)
                {
                    int dmg = (int)Math.Ceiling(player.Atk * info.DamageRatio);
                    player.DealDamage(target, dmg);
                    Thread.Sleep(300);
                }
            }
            else
            {
                int dmg = (int)Math.Ceiling(player.Atk * info.DamageRatio);
                Console.WriteLine($"{player.Name}가 {info.Name} 사용: {dmg} 데미지");
                Thread.Sleep(700);
                player.DealDamage(target, dmg);
            }
        }

        public static void HandleSkill(Player player, Enemy[] enemies)
        {
            // 스킬 선택
            Console.WriteLine("🛡️ 스킬을 선택하세요:");
            for (int i = 0; i < SkillRepository.All.Length; i++)
                Console.WriteLine($"{i + 1}. {SkillRepository.All[i].Name} (MP:{SkillRepository.All[i].ManaCost})");
            Console.Write(">> ");
            if (!int.TryParse(Console.ReadLine(), out int sk) || sk < 1 || sk > SkillRepository.All.Length)
            {
                Console.WriteLine("잘못된 입력"); Thread.Sleep(800); HandleSkill(player, enemies); return;
            }
            var chosenType = SkillRepository.All[sk - 1].Type;

            // 대상 선택
            Console.WriteLine("🏷️ 스킬을 사용할 적을 고르세요:");
            for (int i = 0; i < enemies.Length; i++)
                if (enemies[i].Hp > 0)
                    Console.WriteLine($"{i + 1}. {enemies[i].Name} (HP:{enemies[i].Hp})");
            Console.Write(">> ");
            if (!int.TryParse(Console.ReadLine(), out int tg) || tg < 1 || tg > enemies.Length || enemies[tg - 1].Hp <= 0)
            {
                Console.WriteLine("잘못된 입력"); Thread.Sleep(800); HandleSkill(player, enemies); return;
            }
            var target = enemies[tg - 1];

            // 스킬 실행
            UseSkill(player, target, chosenType);
        }
    }
}
