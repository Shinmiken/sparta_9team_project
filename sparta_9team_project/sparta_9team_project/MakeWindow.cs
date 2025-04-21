using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace sparta_9team_project
{
    public class ConsoleManager
    {
        public static void ConfigureConsoleSize()
        {
            Console.SetWindowSize(150, 40);     // 실제 보이는 창
            Console.SetBufferSize(150, 100);    // 스크롤 가능 크기
        }

        public static void PrintCentered(string text, int y)
        {
            int x = (Console.WindowWidth - text.Length) / 2;
            Console.SetCursorPosition(x - 10, y);
            Console.WriteLine(text);
        }

        public static void PrintAsciiAt(string ascart, int x, int y)
        {
            string[] lines = ascart.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.WriteLine(lines[i]);
            }
        }
    }

}
