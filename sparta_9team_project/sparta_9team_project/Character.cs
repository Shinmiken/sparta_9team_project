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

        public Character(int Level, string Name, int Atk, int Def, int Hp, int Gold)
        {
            this.Level = Level;
            this.Name = Name;
            this.Atk = Atk;
            this.Def = Def;
            this.Hp = Hp;
            this.Gold = Gold;
        }

        public void DealDamage(Enemy mob, int damage)
        {
        }
    }

    public class Enemy
    {
        public int Hp { get; set; }
        public string Name { get; set; }

        public Enemy(string name, int hp)
        {
            Name = name;
            Hp = hp;
        }
    }
}
