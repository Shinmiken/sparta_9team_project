using System;
using System.Collections.Generic;

namespace sparta_9team_project
{
    public class Quest // 퀘스트 목록, 상세 출력을 위한 골격, 보상 == 선물
    {
        public string TITLE { get; set; }               // 퀘스트 제목
        public string DESCRIPTION { get; set; }         // 사용자에게 보여지는 설명
        public string CONDITION { get; set; }           // 내부 추적용, UI에 출력되지 않음
        public int REQUIRED_COUNT { get; set; }         // 달성 조건 수
        public int CURRENT_COUNT { get; set; }          // 현재 달성 수치 + 퀘스트 수락 / 완료 조건 추적
        public string REWARD { get; set; }              // 사용자에게 보여지는 선물

        public bool IS_ACCEPTED { get; set; }           // 퀘스트 수락 여부
        public bool IS_REWARD_CLAIMED { get; set; }     // 선물 수령 여부
        public bool IS_COMPLETED => CURRENT_COUNT >= REQUIRED_COUNT;

        public Quest(string title, string description, string condition, int requiredCount, string reward)
        {
            this.TITLE = title;
            this.DESCRIPTION = description;
            this.CONDITION = condition;
            this.REQUIRED_COUNT = requiredCount;
            this.CURRENT_COUNT = 0;
            this.REWARD = reward;
            this.IS_ACCEPTED = false;
            this.IS_REWARD_CLAIMED = false;
        }

        public void ShowQuestDetail()
        {
            Console.Clear();
            Console.WriteLine("Quest!!\n");
            Console.WriteLine($"{TITLE}\n");
            Console.WriteLine($"{DESCRIPTION}\n");

            Console.Write("\n(아무 키나 입력하면 돌아갑니다.)");
            Console.ReadKey();
        }

        public void ShowRewardMenu()
        {
            Console.Clear();

            string displayReward = REWARD;
            if (TITLE == "9, 또 너야 ?" && IS_COMPLETED && IS_REWARD_CLAIMED)
                displayReward = "⟡༺༒9조의 축복༒༻⟡"; // 던전 재입장 시 문구 변경

            Console.WriteLine("- 선물 -\n" + displayReward + "\n");

            if (IS_COMPLETED && !IS_REWARD_CLAIMED)
            {
                Console.WriteLine("1. 선물 받기");
                Console.WriteLine("2. 돌아가기");

                Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    RewardManager.GiveReward(this);
                    IS_REWARD_CLAIMED = true;
                    Console.WriteLine("🎁 선물을 받았습니다!");
                }
                else
                {
                    Console.WriteLine("돌아갑니다.");
                }
            }
            else if (!IS_ACCEPTED)
            {
                Console.WriteLine("1. 수락");
                Console.WriteLine("2. 거절");
            }
            else
            {
                Console.WriteLine("1. 돌아가기");
            }

            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
        }

        public void ResetQuest()
        {
            IS_ACCEPTED = false;
            IS_REWARD_CLAIMED = false;
            CURRENT_COUNT = 0;
        }
    }

    public static class QuestManager // 퀘스트 목록, 상세 출력
    {
        public static List<Quest> AllQuests = new List<Quest>();

        public static void InitQuests()
        {
            AllQuests.Add(new Quest(
                "3 아가냥이와 친구가 되었어요 !",
                "전투 중 아가냥이를 만나서 우정을 쌓아보세요.",
                "아가냥이 달래기 성공 (≡・x・≡)",
                1,
                "고양이한테 생선을 맡기면 ?"
            ));

            AllQuests.Add(new Quest(
                "우주 최고 용맹 강아지 ~☆",
                "최고의 용맹함에 도전해 보세요 !",
                "가장 강력한 적들과 맞서는 미르는 최고 강아지 ~☆",
                1,
                "유리조각 x ?"
            ));

            AllQuests.Add(new Quest(
                "9, 또 너야 ?",
                "이번 생에는 구조될 줄 알았어 ´△｀",
                "9조 요청",
                1,
                "???"
            ));
        }

        public static void ShowQuestList()
        {
            Console.Clear();
            Console.WriteLine("[Quest List]\n");

            for (int i = 0; i < AllQuests.Count; i++)
                Console.WriteLine($"{i + 1}. {AllQuests[i].TITLE}");

            Console.Write("\n원하시는 퀘스트를 선택해주세요.\n>> ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= AllQuests.Count)
                AllQuests[choice - 1].ShowQuestDetail();
            else
                Console.WriteLine("잘못된 입력입니다.");
        }
    }

    public static class RewardManager // 선물 지급 처리
    {
        public static void GiveReward(Quest quest)
        {
            if (quest.TITLE == "3 아가냥이와 친구가 되었어요 !")
            {
                InventoryManager.Instance.PlayerInventory.AddItem(ItemDataBase.fish, 1);
                Console.WriteLine("고양이 회피용 생선 1개를 획득했습니다냥 !");
            }
            else if (quest.TITLE == "우주 최고 용맹 강아지 ~☆")
            {
                int amount = new Random().Next(1, 4); // 1~3 랜덤
                InventoryManager.Instance.PlayerInventory.AddItem(ItemDataBase.glassPiece, amount);
                Console.WriteLine($"유리조각 {amount}개를 획득했습니다 !");
            }
            else if (quest.TITLE == "9, 또 너야 ?")
            {
                InventoryUI.AddItem("⟡༺༒9조의 축복༒༻⟡");
                Console.WriteLine("⟡༺༒9조의 축복༒༻⟡을 받았습니다! 이번 전투는 좀 쉬울지도..?");
            }
        }
    }

    public static class InventoryUI // 인벤토리 임시 구현
    {
        private static List<string> items = new List<string>();

        public static void AddItem(string item)
        {
            items.Add(item);
            Console.WriteLine($"[인벤토리 추가] {item}");
        }

        public static void ShowInventory()
        {
            Console.WriteLine("\n[인벤토리 목록]");
            foreach (var item in items)
            {
                Console.WriteLine($"- {item}");
            }
        }
    }
}
