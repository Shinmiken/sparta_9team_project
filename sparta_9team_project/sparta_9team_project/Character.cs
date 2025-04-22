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

        public Character(string Name, int Level, int Atk, int Def, int Hp, int Gold)
        {
            this.Name = Name;
            this.Level = Level;
            this.Atk = Atk;
            this.Def = Def;
            this.Hp = Hp;
            this.Gold = Gold;
        }

        public void DealDamage(Monster mob, int damage)
        {
            
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
