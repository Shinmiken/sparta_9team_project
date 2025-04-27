using System;
using System.Collections.Generic;

namespace sparta_9team_project
{
    public enum SkillType
    {
        Fart,           // 방구뀌기 (공통 기초 스킬)
        Bite,           // 물기 (공통 기초 스킬)
        ArcaneBolt,     // 비전 화살 (마법사 기초 스킬)
        IceSpike,       // 얼음 창 (마법사 중급 스킬)
        MeteorStrike,   // 운석 강하 (마법사 필살기)
        PowerSlash,     // 강타 (전사 기초 스킬)
        ComboSlash,     // 연속베기 (전사 중급 스킬, 2~4회 공격)
        EarthShatter    // 대지 분쇄 (전사 필살기)
    }

    // 스킬 정보 구조체
    public struct SkillInfo
    {
        public string Name;        // 스킬 이름
        public double Multiplier;  // 플레이어 Atk에 곱해질 배수
        public int ManaCost;       // 마나 소모량
        public SkillType Type;     // 스킬 타입

        public SkillInfo(string name, double multiplier, int manaCost, SkillType type)
        {
            Name = name;
            Multiplier = multiplier;
            ManaCost = manaCost;
            Type = type;
        }
    }

    public static class SkillInfos
    {
        public static SkillInfo[] skillInfos = new SkillInfo[]
        {
            // 공통 스킬
            new SkillInfo("방구뀌기",    0.5, 10, SkillType.Fart),
            new SkillInfo("물기",        0.8, 12, SkillType.Bite),
            // 마법사 스킬
            new SkillInfo("비전 화살",   1.5, 20, SkillType.ArcaneBolt),
            new SkillInfo("얼음 창",     2.5, 30, SkillType.IceSpike),
            new SkillInfo("운석 강하",   5.0, 60, SkillType.MeteorStrike),
            // 전사 스킬
            new SkillInfo("강타",        1.2, 15, SkillType.PowerSlash),
            new SkillInfo("연속베기",     1.0, 25, SkillType.ComboSlash),
            new SkillInfo("대지 분쇄",   4.0, 50, SkillType.EarthShatter)
        };
    }

    // 스킬 실행기
    public static class SkillExecutor
    {
        private static Random rng = new Random();

        public static void UseSkill(Player player, Enemy target, SkillType skillType)
        {
            SkillInfo info = SkillInfos.skillInfos[(int)skillType];

            if (player.Mp < info.ManaCost)
            {
                Console.WriteLine("마나가 부족합니다.");
                Console.ReadLine();
                return;
            }
            player.Mp -= info.ManaCost;

            // 연속베기만 따로 로직
            if (skillType == SkillType.ComboSlash)
            {
                int hits = rng.Next(2, 5); // 2~4회 공격
                int damagePerHit = (int)Math.Ceiling(player.Atk * info.Multiplier);
                int totalDamage = 0;
                for (int i = 0; i < hits; i++)
                {
                    player.DealDamage(target, damagePerHit);
                    totalDamage += damagePerHit;
                }
                Console.WriteLine($"{player.Name}가 {info.Name}으로 {hits}회 공격해 총 {totalDamage}의 피해를 입혔습니다!");
            }
            else
            {
                int damage = (int)Math.Ceiling(player.Atk * info.Multiplier);
                player.DealDamage(target, damage);
                Console.WriteLine($"{player.Name}가 {info.Name}을(를) 사용해 {damage}의 피해를 입혔습니다!");
            }
            Console.WriteLine("[Enter]를 눌러 계속...");
            Console.ReadLine();
        }
    }
}
