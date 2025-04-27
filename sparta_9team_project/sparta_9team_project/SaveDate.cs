using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sparta_9team_project
{
    internal class SaveDate
    {
        public static string save = "Save_Game_Data.json";

        public static void SaveGame(Player player)
        {
            string json = JsonConvert.SerializeObject(player, Formatting.Indented);
            File.WriteAllText(save, json);
            Console.SetCursorPosition(52, 25);
            Console.WriteLine("게임이 저장되었습니다.");
            Thread.Sleep(1000);
        }

        public static Player LoadGame()
        {
            if(!File.Exists(save))
            {
                return null;
            }

            string json = File.ReadAllText(save);
            Player player = JsonConvert.DeserializeObject<Player>(json);
            Console.WriteLine("게임을 불러옵니다.");
            return player;
        }
    }
}
