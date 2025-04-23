using System;

namespace sparta_9team_project
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }

        private Random rng = new Random(); // 랜덤 데미지용 필드

        public Character(string Name, int Level, int Atk, int Def, int Hp, int Gold)
        {
            this.Name = Name;
            this.Level = Level;
            this.Atk = Atk;
            this.Def = Def;
            this.Hp = Hp;
            this.Gold = Gold;
        }

        // 데미지를 준다
        public void DealDamage(Monster enemy)
        {
            int variance = (int)Math.Ceiling(Atk * 0.1); // ±10% 오차
            int min = Atk - variance;
            int max = Atk + variance;
            int damage = rng.Next(min, max + 1);

            Console.WriteLine($"{Name}이(가) {enemy.Name}에게 {damage}의 데미지를 입혔습니다!");
            enemy.Hp -= damage;

            if (enemy.Hp < 0) enemy.Hp = 0;
        }

        // 데미지를 받는다
        public void GetDamage(int incomingDamage)
        {
            int damageTaken = Math.Max(incomingDamage - Def, 0); // 방어력 적용
            Hp -= damageTaken;

            Console.WriteLine($"{Name}이(가) {damageTaken}의 피해를 입었습니다. 남은 체력: {Hp}");

            if (Hp < 0) Hp = 0;
        }
    }

    public class Monster
    {
        public int Hp { get; set; }
        public string Name { get; set; }

        public Monster(string name, int hp)
        {
            Name = name;
            Hp = hp;
        }
    }
}
