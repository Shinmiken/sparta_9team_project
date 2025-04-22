using System;

namespace sparta_9team_project
{
    public class  PlayerManager
    {
        public static PlayerManager instance = new PlayerManager();
        public Player mainPlayer { get; private set;}
        public void Init(Player player)
        {
            mainPlayer = player;
        }
    }
    public class Player
    {
		// [Fields]
		public string Name { get; set; } = "미르";
        public int Level { get; set; } = 1;
        public int Hp { get; set; }
		public int MaxHp { get; set; } = 100;
		public int Attak { get; set; }
		public int Defense { get; set; }
        public string Job { get; set; }
        public string Bones { get; set; }


    }
}