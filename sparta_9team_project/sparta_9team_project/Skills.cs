using System;
using System.Collections.Generic;

namespace sparta_9team_project
{
    // 스킬 타입 열거형
    public enum SkillType
    {
        Fart,           // 방구뀌기 (공통 기초 스킬)
        Bite,           // 물기 (공통 기초 스킬)
        ArcaneBolt,     // 비전 화살 (마법사 기초 스킬)
        IceSpike,       // 얼음 창 (마법사 중급 스킬)
        MeteorStrike,   // 운석 강하 (마법사 필살기)
        PowerSlash,     // 강타 (전사 기초 스킬)
        ComboSlash,     // 연속베기 (전사 중급 스킬)
        EarthShatter    // 대지 분쇄 (전사 필살기)
    }

    // 스킬 기본 정보 구조체
    public struct SkillInfo
    {
        public SkillType Type;      // 스킬 종류
        public string Name;         // 스킬 이름
        public double DamageRatio;  // 공격력 대비 데미지 비율 (1.0 = 100%)
        public int ManaCost;        // 마나 비용

        public SkillInfo(SkillType type, string name, double damageRatio, int manaCost)
        {
            Type = type;
            Name = name;
            DamageRatio = damageRatio;
            ManaCost = manaCost;
        }
    }

    // 모든 스킬 정보를 담은 배열
    public static class SkillInfos
    {
        public static SkillInfo[] skillInfos = new SkillInfo[]
        {
            new SkillInfo(SkillType.Fart,           "방구뀌기",      0.5,  10),
            new SkillInfo(SkillType.Bite,           "물기",          0.7,  12),
            new SkillInfo(SkillType.ArcaneBolt,     "비전 화살",      1.2,  20),
            new SkillInfo(SkillType.IceSpike,       "얼음 창",        1.5,  30),
            new SkillInfo(SkillType.MeteorStrike,   "운석 강하",      2.5,  50),
            new SkillInfo(SkillType.PowerSlash,     "강타",          1.1,  15),
            new SkillInfo(SkillType.ComboSlash,     "연속베기",      0.8,  25),
            new SkillInfo(SkillType.EarthShatter,   "대지 분쇄",      2.2,  45)
        };
    }

    // 스킬 사용 및 처리 로직
    public static class Skills
    {
        // 단일 스킬 실행
        public static void UseSkill(Player player, Enemy target, SkillType type)
        {
            var info = SkillInfos.skillInfos[(int)type];
            if (player.Mp < info.ManaCost)
            {
                Console.WriteLine("마나가 부족합니다.");
                return;
            }
            player.Mp -= info.ManaCost;

            if (type == SkillType.ComboSlash)
            {
                var rand = new Random();
                int hits = rand.Next(2, 5); // 2~4회 연속 공격
                Console.WriteLine($"{player.Name}가 {info.Name}으로 {hits}회 연속 공격을 시도합니다!");
                for (int i = 0; i < hits; i++)
                {
                    int damage = (int)Math.Ceiling(player.Atk * info.DamageRatio);
                    player.DealDamage(target, damage);
                }
            }
            else
            {
                int damage = (int)Math.Ceiling(player.Atk * info.DamageRatio);
                Console.WriteLine($"{player.Name}가 {info.Name}을(를) 사용해 {damage}의 피해를 입혔습니다.");
                player.DealDamage(target, damage);
            }
        }

        public static void HandleSkill(Player player, Enemy[] enemies)
        {
            // 1) 스킬 선택
            Console.WriteLine("🛡️ 사용할 스킬을 선택하세요:");
            for (int i = 0; i < SkillInfos.skillInfos.Length; i++)
            {
                var info = SkillInfos.skillInfos[i];
                Console.WriteLine($"{i + 1}. {info.Name} (MP: {info.ManaCost})");
            }
            if (!int.TryParse(Console.ReadLine(), out int skillChoice)
                || skillChoice < 1
                || skillChoice > SkillInfos.skillInfos.Length)
            {
                Console.WriteLine("잘못된 입력입니다. 다시 시도합니다.");
                HandleSkill(player, enemies);
                return;
            }
            var selectedSkill = SkillInfos.skillInfos[skillChoice - 1].Type;

            // 2) 대상 선택
            Console.WriteLine("🏷️ 스킬을 사용할 적 번호를 선택하세요:");
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].Hp > 0)
                    Console.WriteLine($"[{i + 1}] {enemies[i].Name} (HP: {enemies[i].Hp})");
            }
            if (!int.TryParse(Console.ReadLine(), out int targetIdx)
                || targetIdx < 1
                || targetIdx > enemies.Length
                || enemies[targetIdx - 1].Hp <= 0)
            {
                Console.WriteLine("잘못된 입력입니다. 다시 시도합니다.");
                HandleSkill(player, enemies);
                return;
            }
            var target = enemies[targetIdx - 1];

            // 3) 스킬 실행
            UseSkill(player, target, selectedSkill);
        }
    }
}
