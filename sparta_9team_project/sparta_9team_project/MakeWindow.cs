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

        }// 글자 가운데로 천천히 나오게....
        public static void PrintCenteredSlow(string text, int x, int y, int delay)
        {
            Console.SetCursorPosition(x, y);
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
        public static void PrintAnywhere(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(text);
        }

        // 글자 색깔 바꿔서 원하는 곳에 출력하기
        public static void ColorPrintAnyWhere(ConsoleColor color, string text, int x, int y)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.WriteLine(text);
        }

        // 이미지 색깔 바꿔서 원하는 곳에 출력하기
        public static void ColorPrintAsciiAt(ConsoleColor color, string ascart, int x, int y)
        {
            Console.ForegroundColor = color;
            string[] lines = ascart.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.WriteLine(lines[i]);
            }

        }
    }
}
