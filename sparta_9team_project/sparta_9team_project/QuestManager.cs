using System;
using System.Collections.Generic;

namespace sparta_9team_project
{
    public class Quest //퀘스트 목록, 상세 출력을 위한 골격, 보상 == 선물
    {
        public string TITLE { get; set; }             // 퀘스트 제목
        public string DESCRIPTION { get; set; }       // 사용자에게 보여지는 설명
        public string CONDITION { get; set; }         // 내부 추적용, UI에 출력되지 않음
        public int REQUIRED_COUNT { get; set; }       // 달성 조건 수
        public int CURRENT_COUNT { get; set; }        // 현재 달성 수치 + 퀘스트 수락 / 완료 조건 추적
        public string REWARD { get; set; }            // 사용자에게 보여지는 선물

        public bool IS_ACCEPTED { get; set; }         // 출력용 퀘스트 선물(보상) (랜덤인 경우 ???로 출력) + 퀘스트 수락 / 완료 조건 추적
        public bool IS_REWARD_CLAIMED { get; set; }   // 퀘스트 수락 여부
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

        public void ShowQuestDetail() //“???” → “9조의 축복” 출력 시점 분기
        {
            Console.Clear();
            Console.WriteLine("Quest!!\n");
            Console.WriteLine($"{TITLE}\n");
            Console.WriteLine($"{DESCRIPTION}\n");

            // ‘9조의 축복’은 던전 재입장 시에만 보이게 설정
          
            // 던전 입장 시 특별 보상 출력 (Dungeon.cs에 추가해야할 듯 합니당)
            /*var quest = QuestManager.AllQuests.Find(q => q.TITLE == "9, 또 너야 ?");
            if (quest != null && quest.IS_COMPLETED && !quest.IS_REWARD_CLAIMED)
            {
                Console.WriteLine("선물: ⟡༺༒9조의 축복༒༻⟡을 받았습니다! 다음 전투에서 무적입니다!");
                Inventory.AddItem("⟡༺༒9조의 축복༒༻⟡");
                quest.IS_REWARD_CLAIMED = true;
            }*/
            
            string displayReward = REWARD;
            if (TITLE == "9, 또 너야 ?" && IS_COMPLETED && IS_REWARD_CLAIMED)
                displayReward = "⟡༺༒9조의 축복༒༻⟡"; // 던전 재입성 즉, 보상 수령 시 문구 출력

            Console.WriteLine("- 선물 -\n" + REWARD + "\n");

            if (IS_COMPLETED && !IS_REWARD_CLAIMED)
            {
                Console.WriteLine("1. 선물 받기");
                Console.WriteLine("2. 돌아가기");
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

            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
        }
    }

    public static class QuestManager //퀘스트 목록, 상세 출력
    {
        public static List<Quest> AllQuests = new List<Quest>();

        public static void InitQuests()
        {
            AllQuests.Add(new Quest(
                "3 아가냥이와 친구가 되었어요 !",
                "전투 중 아가냥이를 만나서 우정을 쌓아보세요.",
                "아가냥이 달래기 성공 (≡・x・≡)", // 내부 조건
                1,
                "고양이한테 생선을 맡기면 ?"
            ));
            // TODO: 한 전투 페이즈 내 Enemytype == catling 3마리 등장 시 유효
            // 조건1: 세 마리 모두 체력 5 이하로 3마리 남기기
            // 조건2: 캣닢 사용 시
            // 둘 중 하나 충족 시 CURRENT_COUNT++
            // - 캣닢은 DropManager에서 5% 확률로 드랍하도록
            // 선물은 EncounterManager에서 고양이 Encounter 시 전투 스킵으로 구현

            AllQuests.Add(new Quest(
                "우주 최고 용맹 강아지 ~☆",
                "최고의 용맹함에 도전해 보세요 !",
                "가장 강력한 적들과 맞서는 미르는 최고 강아지 ~☆",
                1,
                "유리조각 x ?"
            ));
            // TODO: 한 전투 페이즈에 husky OR motorcycle 3마리 등장 시
            // 유리조각 1~3개 (RewardManager에서 랜덤 지급) → 조합 시스템으로 유리병 (유리 조각 5개) 만들기
            // 유리병 + 허스키 체력 1 상태 → 우유 획득

            AllQuests.Add(new Quest(
                "9, 또 너야 ?",
                "이번 생에는 구조될 줄 알았어 ´△｀",
                "9조 요청",
                1,
                "???"
            ));
            // TODO: 우유 9병만 모으고 게임 종료 or 최종 보스 도전 실패 시 조건 충족
            // - 최종 보스 패배 시 우유 1병 파괴되도록
            // - 선물: ⟡༺༒9조의 축복༒༻⟡ → 던전 재입장 시 무적
            // - 다음 보스전 무적 처리 (선물: 9조의 축복)로 엔딩 클리어 가능 (반전)
            // - 퀘스트 작명 제안받습니다. 밈을 써도 좋을 것 같아요 ! (아스키 짤은 대충 손현주 거지짤..?)
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

    public static class RewardManager //선물 지급 처리 멘트
    {
        public static void GiveReward(Quest quest)
        {
            if (quest.TITLE == "3 아가냥이와 친구가 되었어요 !")
            {
                Inventory.AddItem("생선 x 1");
                Console.WriteLine("고양이 회피용 생선 1개를 획득했습니다냥 !");
            }
            else if (quest.TITLE == "우주 최고 용맹 강아지 ~☆")
            {
                int amount = new Random().Next(1, 4); // 1~3 랜덤
                Inventory.AddItem($"유리조각 x {amount}");
                Console.WriteLine($"유리조각 {amount}개를 획득했습니다 !");
            }
            else if (quest.TITLE == "9, 또 너야 ?")
            {
                Inventory.AddItem("⟡༺༒9조의 축복༒༻⟡");
                Console.WriteLine("⟡༺༒9조의 축복༒༻⟡을 받았습니다! 이번 전투는 좀 쉬울지도..?");
            }
        }
    }

    public static class DropManager //산책하기 중 캣닢 드랍 확률 5%로 설정
    {
        private static Random rng = new Random();

        public static bool TryDropCatnip()
        {
            int chance = rng.Next(0, 100);
            return chance < 5;
        }
    }

    public static class Inventory
    {
        private static List<string> items = new List<string>();

        public static void AddItem(string item)
        {
            items.Add(item);
        }
    }
}

public static class QuestManager
{
    public static void TriggerQuest(string questName)
    {
        // TODO: 나중에 조건 확인하고 선물 지급하는 로직 추가 필요 !!
        Console.WriteLine($"[퀘스트 트리거됨] : {questName}");
    }
}

// - 기능 체크리스트
// - player.UsedItem(string) 메서드를                  {특정 아이템 사용 여부 체크 목적}	                Player.cs 클래스에 추가 필요
// - enemies 리스트 (현재 전투 중 몬스터 목록)을	           {EncounterManager / Battle.cs에서 활용 목적}	  EncounterManager.cs에 있도록 연결하기
// - '던전 재입장 시 9조 퀘스트 완료 체크'를	                 “9조의 축복” 표시 여부	                        Dungeon.Enter() 또는 Walk.Start() 시작부에서 IS_COMPLETED 확인 후 처리 (?)
// - 선물 수령 이후 IS_REWARD_CLAIMED = true; 처리	필요    보상 중복 방지	                                보상 수령 시 RewardManager.GiveReward() 호출 후 Quest 객체 수정 필요 . .
