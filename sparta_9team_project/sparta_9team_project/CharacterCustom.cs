using System;

namespace sparta_9team_project
{
    public enum JobType { 전사, 마법사 }

    public static class CharacterCustom
    {
        public static string SelectedName { get; private set; } = "미르";
        public static JobType SelectedJob { get; private set; } = JobType.전사;

        public static void SetupCharacter()
        {
            // 1. 이름 입력 받기
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.Write("원하시는 이름을 설정해주세요.\n>> ");
            string inputName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(inputName))
                SelectedName = inputName;

            // 2. 직업 선택
            while (true)
            {
                Console.Clear();
                Console.WriteLine("직업을 선택해주세요:");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 마법사");
                Console.Write(">> ");

                string input = Console.ReadLine();
                if (input == "1")
                {
                    SelectedJob = JobType.전사;
                    break;
                }
                else if (input == "2")
                {
                    SelectedJob = JobType.마법사;
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
