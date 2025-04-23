using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sparta_9team_project
{
    class Inventory
    {
        Player player = PlayerManager.instance.mainPlayer;

        private List<Item> items = new List<Item>();
        public void AddItem(Item item)
        {
            if (items.Contains(item))
            {
                //내용 들어갈것
            }
            items.Add(item);
            Console.WriteLine($"{player.Name}의 소지품에 {item.Name}이(가) 인벤토리에 추가되었습니다.");
        }
    }
}
