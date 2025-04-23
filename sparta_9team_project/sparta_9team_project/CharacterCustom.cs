using System;

namespace sparta_9team_project
{
    public static class CharacterCustom
    {
        public static string SelectedName { get; private set; } = "미르";

        public static void SetupCharacter()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.Write("원하시는 이름을 설정해주세요.\n>> ");
            string inputName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(inputName))
                SelectedName = inputName;
        }
    }
}
