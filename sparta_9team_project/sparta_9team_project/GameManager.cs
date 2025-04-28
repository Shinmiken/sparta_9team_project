using System;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using System.Media;
using System.ComponentModel.Design;
using System.IO.Compression;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
#nullable disable

namespace sparta_9team_project
{
    class GameManager
    {
        public static string wearAtkItemName; // 공격력 아이템 장착 비교
        public static string wearDefItemName; // 방어력 아이템 장착 비교
        public static int atkItemCount = 0; // 공격력 아이템 장착 여부
        public static int defItmeCount = 0; // 방어력 아이템 장착 여부
        public static int type; // 상태보기에서 직업에 따른 이미지 표시를 위한 변수
        static JobType selectjob; // 직업 선택 enum 변수
        static int questcnt = 0;
        #nullable disable

        static void Main(string[] args)
        {
            SoundManager.StopBGM();
            SoundManager.PlayStartBGM();
            Console.OutputEncoding = Encoding.UTF8;
            ConsoleManager.ConfigureConsoleSize();
            Dungeon dungeon = new Dungeon();

            Player player = StartGame();
            type = player.ImageType;
            PlayerManager.instance.Init(player);


            while (true)
            {
                Console.Clear();
                MainScreen();

                Console.SetCursorPosition(61, 28);
                string choice = Console.ReadLine();

                if (choice == "1") // 상태 보기
                {
                    MakeAnimation();
                    Thread.Sleep(500);
                    ShowStatus(player);
                }
                else if (choice == "2") // 던전 입장
                {
                    Dungeon.Walk();
                }
                else if (choice == "3") // 인벤토리
                {
                    ShowInventory();
                }
                else if (choice == "4") // 스킬보기
                {
                    ShowSkill();
                }
                else if (choice == "5") // 퀘스트 보기
                {
                    ShowQuest();
                }
                else if (choice == "6") // 게임 저장
                {
                    SaveDate.SaveGame(player);
                }
                else if (choice == "7" /*&& 우유가 10개이면*/)
                {
                    //최종 스테이지
                    Dungeon.HiddenStage();
                }
                else if (choice == "0") // 게임 종료
                {
                    ShowGameEnd();
                }
            }
        }

        //첫 시작화면
        public static Player StartGame()
        {
            while (true)
            {
                Console.Clear();
                ConsoleManager.ColorPrintAsciiAt(ConsoleColor.Red, Print.dogImage[3], 75, 6);
                ConsoleManager.ColorPrintAsciiAt(ConsoleColor.Yellow, Print.dogImage[5], 0, 7);
                ConsoleManager.PrintAnywhere("『 미  르  의  모  험 』", 50, 2);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Blue, "✦･ﾟ:* 미지의 세계로 떠나는 여정 *:･ﾟ✦", 43, 3);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Cyan, "⟡༺༒ 1. 새로운 여정 시작하기 ༒༻⟡", 46, 5);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Red, "⟡༺༒ 2. 저장된 여정 이어하기 ༒༻⟡", 46, 7);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.White, "⟡༺༒ 0. 종료하기 ༒༻⟡", 51, 9);

                ConsoleManager.PrintAnywhere(">>", 58, 11);
                ConsoleManager.PrintAnywhere("<<", 63, 11);
                Console.SetCursorPosition(61, 11);
                string Choice = Console.ReadLine();
                if (Choice == "1")
                {
                    ChoiceJob(); // 직업 선택
                    return new Player("미르", 1, 100, 100, 100, 100, 10, 5, selectjob, "0", type);
                }
                else if (Choice == "2")
                {
                    Player load = SaveDate.LoadGame();
                    if (load != null)
                    {
                        return load;
                    }
                    else
                    {
                        ConsoleManager.PrintAnywhere("저장된 게임이 없습니다.", 50, 13);
                        Thread.Sleep(1000);
                    }
                }
                else if (Choice == "0")
                {
                    ShowGameEnd();
                }
                else
                {
                    Console.SetCursorPosition(53, 11);
                    Console.WriteLine("잘못된 입력입니다");
                    Console.SetCursorPosition(53, 12);
                    Console.WriteLine("다시 입력해주세요");
                    Thread.Sleep(1000);
                }
            }
        }

        //직업 선택 화면
        public static void ChoiceJob()
        {
            while (true)
            {
                Console.Clear();
                ConsoleManager.ColorPrintAsciiAt(ConsoleColor.Red, Print.dogImage[3], 75, 6);
                ConsoleManager.ColorPrintAsciiAt(ConsoleColor.Yellow, Print.dogImage[5], 0, 7);
                ConsoleManager.PrintAnywhere("✦･ﾟ:* 락토프리 우유를 너무나도 좋아하는 댕댕이 미르는 *:･ﾟ✦", 33, 1);
                ConsoleManager.PrintAnywhere("✦･ﾟ:* 주인들이 주는 우유의 양이 턱없이 부족했던 탓에 *:･ﾟ✦", 34, 2);
                ConsoleManager.PrintAnywhere("✦･ﾟ:* 우유를 직접 찾으러 나서기로 하는데... *:･ﾟ✦", 38, 3);
                ConsoleManager.PrintAnywhere("미르의 우유찾기 모험을 도와줄 직업을 선택해주세요!", 38, 5);
                ConsoleManager.PrintAnywhere("⟡༺༒ 1. 마법사 ༒༻⟡", 53, 7);
                ConsoleManager.PrintAnywhere("⟡༺༒ 2. 워리어 ༒༻⟡", 53, 9);
                ConsoleManager.PrintAnywhere(">>", 58, 11);
                ConsoleManager.PrintAnywhere("<<", 65, 11);
                Console.SetCursorPosition(62, 11);
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    selectjob = JobType.마법사;
                    type = 1;
                    break;
                }
                else if (choice == "2")
                {
                    selectjob = JobType.전사;
                    type = 2;
                    break;
                }
                else
                {
                    Console.SetCursorPosition(54, 13);
                    Console.WriteLine("잘못된 입력입니다");
                    Console.SetCursorPosition(54, 14);
                    Console.WriteLine("다시 입력해주세요");
                    Thread.Sleep(1000);
                }
            }
        }

        //게임 메인화면
        public static void MainScreen()
        {
            PlayerManager.instance.mainPlayer.SkillTree(selectjob); // 스킬트리 초기화
            SoundManager.StopBGM();
            SoundManager.PlayStartBGM();
            int x = 20;
            Console.Clear();
            ConsoleManager.ColorPrintAsciiAt(ConsoleColor.White, Print.dogImage[0], 60, 5);
            ConsoleManager.PrintAnywhere("✦ ────── 『MENU 』────── ✦", 16, 6);
            ConsoleManager.PrintAnywhere("[1] ✧ 『상태보기 』", 20, 8);
            ConsoleManager.PrintAnywhere("[2] ✧ 『산책하기 』", 20, 10);
            ConsoleManager.PrintAnywhere("[3] ✧ 『인벤토리 』", 20, 12);
            ConsoleManager.PrintAnywhere("[4] ✧ 『스킬보기 』", 20, 14);
            ConsoleManager.PrintAnywhere("[5] ✧ 『퀘스트창 』", 20, 16);

            if (ItemDataBase.milk.IsAllMilkCollected)
            {
                ConsoleManager.PrintAnywhere("[6] ✧ 『게임저장 』", 20, 18);
                ConsoleManager.PrintAnywhere("[6] ✧ 『게임저장 』", 20, 20);
                ConsoleManager.PrintAnywhere("[0] ✧ 『종료하기 』", 20, 22);
                ConsoleManager.PrintAnywhere("✦ ────────────────────── ✦", 16, 24);
            }
            else
            {
                ConsoleManager.PrintAnywhere("[6] ✧ 『게임저장 』", 20, 18);
                ConsoleManager.PrintAnywhere("[0] ✧ 『종료하기 』", 20, 20);
                ConsoleManager.PrintAnywhere("✦ ────────────────────── ✦", 16, 22);
            }
            ConsoleManager.PrintAnywhere(">>", 56, 31);
            ConsoleManager.PrintAnywhere("<<", 65, 28);
        }

        //미르 모습 변경 함수
        static void MakeAnimation()
        {
            int y = 23;
            string[] image1Lines = Print.dogImage[0].Split('\n');
            string[] image2Lines = Print.dogImage[type + 2].Split('\n');

            //int width = Math.Max(image1Lines.Max(list => list.Length), image2Lines.Max(list => list.Length));

            // 밑에서부터 두 번째 이미지로 한 줄씩 덮어쓰기
            for (int i = 21; i >= 0; i--)
            {
                Console.SetCursorPosition(61, y);
                Console.Write(image2Lines[i].PadRight(50)); // 공백으로 채워서 덮어쓰기
                y--;
                Thread.Sleep(100); // 덮는 속도 조절
            }
        }

        //미르의 상태보기
        public static void ShowStatus(Player player)
        {
            int x = 20;
            while (true)
            {
                Console.Clear();
                ConsoleManager.PrintAnywhere("미르의 상태보기", x, 6);
                ConsoleManager.PrintAnywhere($"Lvl. {player.Level}", x, 9);
                ConsoleManager.PrintAnywhere($"Exp : {player.Exp}", x, 10); // 경험치 변수 생기면 변경 필요
                ConsoleManager.PrintAnywhere($"미르           직업 : {player.Job}", x, 11);
                ConsoleManager.PrintAnywhere("=================================", x, 12);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Red, $"공격력 : {player.Atk}", x, 13);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Blue, $"방어력 : {player.Def}", x, 14);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Green, $"체력 : {player.Hp} / {player.MaxHp}", x + 2, 15);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Yellow, $"뼈다귀 : {player.Bones}", x, 16);
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.Red, $"패배 : {player.Bones}", x + 2, 17); // 패배 변수 생기면 변경 필요
                ConsoleManager.ColorPrintAnyWhere(ConsoleColor.White, ">> [0]돌아가기", x, 18);
                ConsoleManager.PrintAsciiAt(Print.dogImage[type + 2], 61, 2);
                ConsoleManager.PrintAnywhere(">>", x, 19);
                Console.SetCursorPosition(24, 19);
                string choice = Console.ReadLine();
                if (choice == "0")
                {
                    break;
                }
                else
                {
                    Console.SetCursorPosition(40, 23);
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.SetCursorPosition(40, 24);
                    Console.WriteLine("다시 입력해주세요.");
                    Thread.Sleep(1000);
                }
            }
        }

        //인벤토리 보여주기
        public static void ShowInventory()
        {
            bool isOver = true;
            while (isOver)
            {
                Console.Clear();
                int width = 150;
                int height = 29;
                Item selectedItem = null;

                // 상단과 하단 테두리 출력
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x == 0 && y == 0)
                        {
                            ConsoleManager.PrintAnywhere("╔", x, y);
                        }
                        else if (x == width - 1 && y == 0)
                        {
                            ConsoleManager.PrintAnywhere("╗", x, y);
                        }
                        else if (x == 0 && y == 28)
                        {
                            ConsoleManager.PrintAnywhere("╚", x, y);
                        }
                        else if (x == width - 1 && y == 28)
                        {
                            ConsoleManager.PrintAnywhere("╝", x, y);
                        }
                        else if (x == 0 && y == 18)
                        {
                            ConsoleManager.PrintAnywhere("╠", x, y);
                        }
                        else if (x == width - 1 && y == 18)
                        {
                            ConsoleManager.PrintAnywhere("╣", x, y);
                        }
                        else if (y == 0 || y == height - 1)
                        {
                            ConsoleManager.PrintAnywhere("═", x, y);
                        }
                        else if (x == 0 || x == width - 1)
                        {
                            ConsoleManager.PrintAnywhere("║", x, y);
                        }

                    }
                }
                for (int x = 1; x < 119; x++) // 중앙 가로 줄 출력
                {
                    string line = (x == 119 / 2) ? "╦" : "═";
                    if (x == 119 / 2)
                    {
                        ConsoleManager.PrintAnywhere("╦", x, 0);
                    }
                    ConsoleManager.PrintAnywhere(line, x, 18);
                }
                for (int y = 1; y < 29; y++) // 중앙 세로 줄 출력
                {
                    string line = (y == 28) ? "╩" : "║";
                    if (y == 18)
                    {
                        line = "╬";
                    }
                    ConsoleManager.PrintAnywhere(line, 59, y);
                }
                //찐 인벤 표시
                var inventory = InventoryManager.Instance.PlayerInventory.inventory;

                // 인벤토리가 비었는지 체크
                if (inventory.Count == 0)
                {
                    ConsoleManager.PrintAnywhere("인벤토리가 비어 있습니다.", 2, 1);
                }
                else
                {
                    Consumable tempConsumable = (Consumable)ItemDataBase.smallHealingPotion;
                    tempConsumable.ShowOnlyConsumables();
                    int i = 1;
                    foreach (var (itemName, itemObject) in inventory) // 여기! count가 아니라 itemObject
                    {
                        if (ItemDataBase.weaponStorage.ContainsKey(itemName) || ItemDataBase.armorStorage.ContainsKey(itemName))
                        {
                            Console.SetCursorPosition(60, i);

                            string displayName = itemObject.Name;

                            if (itemObject.Type == ItemType.무기 && itemObject.Name == wearAtkItemName)
                            {
                                displayName = "[E] " + displayName;
                            }
                            else if (itemObject.Type == ItemType.방어구 && itemObject.Name == wearDefItemName)
                            {
                                displayName = "[E] " + displayName;
                            }

                            Console.WriteLine($"{displayName} - {itemObject.Description}");
                            i++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                // 아이템 표시

                ConsoleManager.PrintAnywhere("아이템 이름을 입력해주세요 : ", 60, 19);
                ConsoleManager.PrintAnywhere("나가기는 나가기를 입력해주세요", 60, 20);
                Console.SetCursorPosition(89, 19);
                string chooseitem = Console.ReadLine();

                // 1. 아이템을 찾는다


                if (ItemDataBase.consumableStorage.ContainsKey(chooseitem))
                {
                    selectedItem = ItemDataBase.consumableStorage[chooseitem];
                }
                else if (ItemDataBase.weaponStorage.ContainsKey(chooseitem))
                {
                    selectedItem = ItemDataBase.weaponStorage[chooseitem];
                }
                else if (ItemDataBase.armorStorage.ContainsKey(chooseitem))
                {
                    selectedItem = ItemDataBase.armorStorage[chooseitem];
                }

                if (chooseitem == "나가기")
                {
                    isOver = false;
                }
                else if (selectedItem == null)
                {
                    ConsoleManager.PrintAnywhere("아이템이 없습니다!", 89, 19);
                    Thread.Sleep(1000);
                }
                else
                {
                    // 여기부터 selectedItem 안전하게 사용
                    if (selectedItem.Type == ItemType.소모품)
                    {
                        Consumable potion = (Consumable)selectedItem;
                        while (true)
                        {
                            ConsoleManager.PrintAnywhere($"{potion.Name}", 2, 19);
                            ConsoleManager.PrintAnywhere("[1] 사용하기", 2, 20);
                            ConsoleManager.PrintAnywhere("[2] 취소하기", 2, 21);
                            ConsoleManager.PrintAnywhere(">>", 2, 22);
                            ConsoleManager.PrintAnywhere(" ", 5, 22);
                            Console.SetCursorPosition(5, 22);
                            string choice = Console.ReadLine();
                            if (choice == "1")
                            {
                                Console.SetCursorPosition(2, 23);
                                potion.UseItem(potion);
                                InventoryManager.Instance.PlayerInventory.RemoveAll(selectedItem);
                                Thread.Sleep(1000);
                                break;
                            }
                            else if (choice == "2")
                            {
                                break;
                            }
                        }
                    }
                    else if (selectedItem.Type == ItemType.무기)
                    {
                        Weapon weapon = (Weapon)selectedItem;
                        while (true)
                        {
                            ConsoleManager.PrintAnywhere($"{weapon.Name}", 2, 19);
                            ConsoleManager.PrintAnywhere("[1] 장착/해제하기", 2, 20);
                            ConsoleManager.PrintAnywhere("[2] 취소하기", 2, 21);
                            ConsoleManager.PrintAnywhere(">>", 2, 22);
                            ConsoleManager.PrintAnywhere(" ", 5, 22);
                            Console.SetCursorPosition(5, 22);
                            string choice = Console.ReadLine();

                            if (choice == "1")
                            {
                                if (wearAtkItemName == weapon.Name)
                                {
                                    weapon.ReleaseItem(weapon);
                                    Thread.Sleep(1000);
                                    break;
                                }
                                else if (atkItemCount != 1)
                                {
                                    Console.SetCursorPosition(2, 23);
                                    weapon.EquipItem(weapon);
                                    wearAtkItemName = weapon.Name;
                                    Thread.Sleep(1000);
                                    break;
                                }
                                else
                                {
                                    ConsoleManager.PrintAnywhere("이미 장착 중 입니다.", 2, 23);
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            else if (choice == "2")
                            {
                                break;
                            }

                        }
                    }
                    else
                    {
                        Armor armor = (Armor)selectedItem;
                        while (true)
                        {
                            ConsoleManager.PrintAnywhere($"{armor.Name}", 2, 19);
                            ConsoleManager.PrintAnywhere("[1] 장착/해제하기", 2, 20);
                            ConsoleManager.PrintAnywhere("[2] 취소하기", 2, 21);
                            ConsoleManager.PrintAnywhere(">>", 2, 22);
                            ConsoleManager.PrintAnywhere(" ", 5, 22);
                            Console.SetCursorPosition(5, 22);
                            string choice = Console.ReadLine();
                            if (choice == "1")
                            {
                                if (wearDefItemName == armor.Name)
                                {
                                    armor.ReleaseItem(armor);
                                    Thread.Sleep(1000);
                                    break;
                                }
                                else if (defItmeCount != 1)
                                {
                                    if (choice == "1")
                                    {
                                        Console.SetCursorPosition(2, 23);
                                        armor.EquipItem(armor);
                                        wearDefItemName = armor.Name;
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                                else
                                {
                                    ConsoleManager.PrintAnywhere("이미 장착 중 입니다.", 2, 23);
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            else if (choice == "2")
                            {
                                break;
                            }

                        }
                    }
                }
            }
        }

        // 스킬 보여주기
        public static void ShowSkill()
        {
            Console.Clear();

            int boxWidth = 30;  // 스킬 하나당 박스 가로 길이
            int boxHeight = 5;  // 스킬 하나당 박스 세로 길이
            int startX = 4;     // 시작 x 위치
            int startY = 4;     // 시작 y 위치
            int boxPerRow = 3;  // 한 줄에 몇 개?
            int width = 150;
            int height = 29;

            // 상단과 하단 테두리 출력
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        ConsoleManager.PrintAnywhere("╔", x, y);
                    }
                    else if (x == width - 1 && y == 0)
                    {
                        ConsoleManager.PrintAnywhere("╗", x, y);
                    }
                    else if (x == 0 && y == 28)
                    {
                        ConsoleManager.PrintAnywhere("╚", x, y);
                    }
                    else if (x == width - 1 && y == 28)
                    {
                        ConsoleManager.PrintAnywhere("╝", x, y);
                    }
                    else if (y == 0 || y == height - 1)
                    {
                        ConsoleManager.PrintAnywhere("═", x, y);
                    }
                    else if (x == 0 || x == width - 1)
                    {
                        ConsoleManager.PrintAnywhere("║", x, y);
                    }

                }
            }
            ConsoleManager.PrintAnywhere("스킬 목록", 4, 2);

            for (int i = 0; i < PlayerManager.instance.mainPlayer.skilltree.Length; i++)
            {
                SkillData data = PlayerManager.instance.mainPlayer.skilltree[i];

                int currentX = startX + (i % boxPerRow) * (boxWidth + 2); // +2는 박스 사이 여백
                int currentY = startY + (i / boxPerRow) * (boxHeight + 1); // +1은 줄 간격

                // 박스 상단 테두리
                ConsoleManager.PrintAnywhere("╔" + new string('═', boxWidth - 2) + "╗", currentX, currentY);

                // 내용
                ConsoleManager.PrintAnywhere("║" + $" {i + 1}. {data.Name}".PadRight(boxWidth - 2), currentX, currentY + 1);
                ConsoleManager.PrintAnywhere("║" + $" 데미지: {data.DamageRatio}".PadRight(boxWidth - 2), currentX, currentY + 2);
                ConsoleManager.PrintAnywhere("║" + $" MP 소모: {data.ManaCost}".PadRight(boxWidth - 2), currentX, currentY + 3);

                // 박스 하단 테두리
                ConsoleManager.PrintAnywhere("╚" + new string('═', boxWidth - 2) + "╝", currentX, currentY + 4);
            }
            ConsoleManager.PrintAnywhere("아무키나 입력하세요", 50, 25);
            Console.SetCursorPosition(60, 26); Console.ReadKey();
        }


        //퀘스트 보여주기
        public static void ShowQuest()
        {
            if (questcnt == 0)
            {
                QuestManager.InitQuests();
                questcnt++;
            }
            QuestManager.ShowQuestList();

        }
        //게임 종료 화면
        static void ShowGameEnd()
        {
            Console.Clear();
            Console.SetCursorPosition(50, 10);
            Console.WriteLine("게임을 종료합니다.");
            Console.SetCursorPosition(53, 15);
            Console.WriteLine("감사합니다.");
            Environment.Exit(0);
        }
    }

    //이미지
    public class Print
    {
        public static string[] dogImage = new string[18]
        { @"
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⢖⢖⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⢲⢴⢲⡂⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⢐⠡⠚⢥⡄⠠⡤⣤⢤⣤⠀⢤⠛⡐⡐⡔⢸⡣⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡿⠠⡱⡸⡨⢂⠪⢇⠑⠄⢅⠂⠟⢌⠐⡕⢜⢌⢜⢝⣠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣄⠻⡈⡒⠔⡐⡐⠌⠄⢅⢑⠄⢅⠑⢄⢑⠨⢐⢐⢐⠕⡳⣠⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡨⢳⢁⢂⠢⡁⡢⠨⠨⡈⡂⠢⡈⡂⡑⡐⢄⢑⢐⢐⠐⠌⠌⡎⣶⡲⠶⠖⢆⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡸⠎⡂⡂⡂⡂⡂⠢⠡⢑⢐⠨⢂⢂⠢⢂⢊⢐⢐⢐⢁⢊⠌⡂⡑⠌⢶⠀⠌⡈⠌⠊⢦⡀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢱⢆⢂⢂⢂⢂⠊⣟⣿⠇⡂⠌⠔⡠⠡⣿⣻⢐⢐⢐⠔⡐⡐⡐⠌⡮⡏⣎⢂⠄⠂⡁⠰⡇⡀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢠⡜⡃⡂⠅⢂⠐⠀⠂⠛⠚⠃⢰⣬⣴⡄⠂⠛⠙⠄⠄⠄⢐⠐⡐⡨⢐⢐⠹⣴⡑⡆⡅⠄⢅⠪⡯⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠐⢫⡴⡤⢑⠀⠄⠡⠈⡐⢨⡔⠘⢚⡗⠃⡤⡅⡈⠄⠂⡈⠠⠐⡐⡐⡐⠤⡅⣷⣣⡇⡎⡎⡢⡹⡽⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢀⡸⡃⠅⠢⠡⡈⠄⢁⠀⡂⠸⠫⠛⠞⠫⠊⡀⠄⠐⡀⢂⢈⢂⢂⢂⠊⢌⢺⢸⠰⡝⣇⣇⢳⡣⠁⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⡎⠎⡊⢀⠂⢐⠀⢂⠀⢂⠁⡈⠄⢁⠐⠀⠄⡁⠂⡂⡂⡂⡂⡢⢡⢳⢱⢑⠕⡱⡸⣞⠚⠈⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⢇⡡⠀⠄⠂⠄⠂⠄⢈⠠⠀⠄⠂⠠⠈⠄⠁⠄⠂⡐⢀⠂⡂⠢⠡⡱⡑⡕⢅⢪⢪⢱⢖⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⢎⣌⢐⢈⢀⠂⡈⠠⠀⠌⠠⠈⠄⠁⠄⠡⢀⠡⢀⠄⠢⠨⠨⡢⡱⣑⢕⠕⡕⡱⡱⡯⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⣗⢐⢐⢐⠀⡂⠄⠡⢀⠡⠐⢈⠠⠁⢂⢐⠨⢐⠨⠨⠨⡈⡎⣽⡪⡢⡣⡣⡣⡗⠉⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⣴⠠⡑⡆⡔⡄⠂⡀⠂⠄⡐⢌⢐⢐⠨⢐⠨⠨⡌⡎⣮⡚⡎⡪⡸⡰⡹⡭⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠫⣣⢊⢎⢢⢣⣪⣢⠣⡣⢪⡲⡘⡔⢈⠢⠨⠪⡸⡮⠓⣗⡇⡇⢇⢳⠇⠁⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡗⣇⢇⢕⢕⢕⡯⠞⠼⠲⡯⡣⡊⠄⢅⢱⡱⠎⠉⠀⣟⡮⣪⢎⡞⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢟⢮⡺⣜⢜⠴⠇⠉⠀⠀⠀⢟⢆⢇⣣⡱⡸⡅⠀⠀⠀⠉⢺⠺⠽⠆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠊⠉⠙⠁⠀⠀⠀⠀⠀⠀⠉⠙⠈⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
            //0
            @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡤⡻⠻⡣⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⢫⠻⢻⢡⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⢵⡣⡣⡩⢱⡲⡀⠴⡶⡲⡶⣲⢶⢖⢍⡊⡮⣒⢽⡕⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⡸⡹⠪⡫⡣⡑⢌⠢⡑⢌⠪⡨⢚⠇⢕⢐⠝⡝⠢⡳⣧⢤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡹⡇⢎⢊⠢⡊⢌⠢⡑⡌⠆⠕⢌⠢⡡⡑⢔⠡⡊⢜⠨⡩⣿⠀⢀⢰⢶⢲⢶⡀⡀⡀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣤⡚⠝⡌⠜⢌⢼⢴⡵⡡⢊⠌⠕⡵⣕⣆⠪⡐⢅⠪⡐⡑⡸⠱⣤⡙⠅⠄⠂⠄⡙⢹⡇⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⢟⠎⡪⠨⢊⠢⢺⢯⠟⠔⠡⠑⠅⠿⡻⡮⠨⠢⢑⠌⡢⢑⢌⣪⠻⢔⢑⢌⠠⢁⠐⡈⢌⡷⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠋⡶⡑⢌⠨⢀⠐⡈⢃⠂⡿⡽⡯⠐⡀⠍⢃⠂⢂⠡⢈⠢⡑⡘⢜⡮⣮⠦⣑⢑⠔⡐⢐⡐⡙⣴⠄⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢀⡨⢗⠕⡡⠐⡀⢂⠐⠸⢳⣠⣌⣾⣨⣄⣢⡹⠕⢐⠀⡂⢐⢐⠨⠨⡂⣕⠽⡣⡢⢣⣑⢕⢇⠢⡂⣟⠆⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠈⠹⡦⣃⢊⠐⡀⢂⢈⠐⡈⢺⣺⡺⡕⣟⡞⡐⢈⠀⡂⢐⠐⠠⢑⢑⢌⠏⣗⡮⡮⣢⢣⠣⡪⡪⣮⢾⠅⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢀⣄⠗⢇⠌⡂⢐⠠⠐⢀⠂⡙⠮⢗⠏⠨⠀⡂⢐⠠⠢⡑⠅⢕⢘⠗⡑⠔⠅⡏⡗⣕⡕⣕⢯⢷⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠈⡈⡾⡢⠡⠐⠠⠐⢈⠠⠐⠀⠌⠠⠈⠄⠡⠐⠠⢈⠐⡈⠨⢐⠔⡡⢊⠜⡨⠢⡱⡙⣞⣞⢎⠉⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠈⠫⣪⢔⠈⠄⡁⠌⠠⠐⢈⠈⠄⠡⠈⠄⠡⢈⠐⡀⠢⡐⢅⠕⡨⢂⠕⢌⠪⡪⡘⡌⡾⣆⡄⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠿⡢⡃⢆⢐⠀⠅⡈⠄⠂⠡⠈⠄⠡⠈⠄⡂⠢⡑⢌⠢⡑⢌⡢⣊⠪⡊⡆⡣⡱⢽⢯⡗⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡺⣏⠢⡑⢌⠄⡐⠀⠅⠨⠀⠅⠨⢠⢑⢌⠪⡨⡂⡣⡑⢕⡿⡰⡑⡕⢜⢌⢎⢽⡋⠊⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢸⣺⢨⢢⠱⡐⣕⢈⠄⡑⡌⡪⡢⡱⡰⡑⢕⠌⡆⣗⣗⢝⢔⢕⢜⢜⢔⢕⠽⡃⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⢊⡮⡆⢇⢝⢚⣦⢧⡧⣮⣖⡽⣇⢇⢪⠢⡃⣧⠎⠫⣻⡴⡕⡜⡔⡕⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⢀⡿⣪⡣⡕⣕⠽⠅⠈⠀⠀⢽⣳⣳⢱⢱⢱⢽⡂⠈⠈⣯⢗⣇⢇⡳⡻⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⢽⢮⢧⣳⢽⣺⠀⠀⠀⠀⠀⠑⢳⣵⣳⢽⣪⠋⠂⠀⠀⣯⢷⡵⣽⣝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠁⠁⠁⠀⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠁⠈⠀⠀⠀⠀⠀⠁⠈ ",
            //1
           @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣤⣤⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢐⣖⢍⠩⡱⣆⢀⢀⠀⢀⢀⢀⢀⢤⠺⡃⡢⠢⢹⡕⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣐⢷⢐⢕⠌⢜⢹⢷⢴⢜⠫⢫⢻⡐⢅⠪⡰⡡⡑⡸⡯⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠨⣗⢅⢪⢪⢊⠆⡢⡑⢔⠰⢡⢑⠌⡌⡢⢱⢱⢑⠌⡜⣽⡴⣔⠀⠀⢠⣤⣤⣤⡤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢠⣨⢷⢐⠕⢌⠢⡑⠔⢌⠢⡑⢅⠢⡑⠔⢌⠢⡑⢅⠕⡘⠗⢜⢓⣤⠛⠣⢐⠠⠠⡈⡛⠫⢛⣤⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢸⡺⡨⢂⢅⠅⠕⢌⢊⠢⡑⢌⠢⡑⢌⢊⠢⡑⢌⠢⡑⢌⢊⠢⡑⢌⠻⣜⢔⠨⢐⠠⢂⢑⢐⠠⣻⠄⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢀⡰⡎⢕⠨⡂⠆⢕⠩⡂⡅⠕⢌⠢⣑⢌⠢⡡⡑⢌⠢⡑⢌⠢⡡⡑⢌⠢⡑⡙⡮⣣⡡⢂⠂⡂⠔⡈⡚⢦⡂⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢈⢱⢎⠔⡡⢊⠜⡰⡵⡞⡫⡘⢔⢑⠜⡻⡼⡔⢌⠢⡑⢌⠢⡑⠔⢌⠢⡱⢸⢰⠨⡙⡮⣢⢣⠠⡁⡂⠔⣽⡂⠀⠀⠀
⠀⠀⠀⠀⠀⢠⡎⡇⢕⠨⡂⠕⢌⠢⣊⡔⠔⢌⠢⠢⡑⢌⡆⡊⡢⡑⢌⠢⡑⢌⠪⡨⢊⠔⡡⠣⡑⢌⠜⡙⣮⡒⢔⠨⡊⣺⡂⠀⠀⠀
⠀⠀⠀⠀⠀⢠⡸⠣⠢⡑⠌⡊⢞⢾⣳⢑⠅⠅⠅⡑⠌⡊⢯⢷⡳⡪⠂⠕⠨⠢⡑⢌⠢⡑⢌⢪⢊⠢⡑⢌⠝⡮⣣⡣⣗⠏⠂⠀⠀⠀
⠀⠀⠀⠀⠀⢸⣎⡪⡁⡂⠅⠌⡐⠅⡂⢖⣮⢶⣇⠂⠅⡐⠡⢑⠰⢈⠌⠌⡨⢐⢈⠢⡑⢌⢌⢪⢪⠨⡨⢂⠕⢌⢷⡳⠍⠁⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠈⢸⢇⢆⢂⠅⠌⠄⠅⡂⡙⢹⡝⠊⠌⡐⠄⡑⢐⠨⢀⠂⢅⠐⠄⡂⢕⠨⡂⣎⡮⡪⡨⠢⡑⢌⢊⣯⢀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢘⣇⢆⢌⣢⣁⢂⢢⡸⢊⢓⠳⣕⠄⠅⢂⠡⡨⡐⡨⣐⢌⠪⡨⢂⠕⡸⢣⢣⢣⢣⠣⡊⡆⡗⡜⣽⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢀⢰⢜⠍⡋⡋⡣⢋⢆⢆⡐⡐⡐⡐⡀⡊⡨⡰⡜⢝⠹⡙⢝⢙⠕⢌⠢⡑⢌⢆⣯⡻⣽⣫⢮⢪⡪⣎⢿⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢸⡣⣱⡪⡸⣲⠨⡢⡱⣝⡮⣖⡨⣂⡪⣐⢼⡇⣕⡕⢬⢆⢕⢐⢅⢕⢨⢨⢢⡳⣳⢽⣺⣪⢯⡮⣞⠺⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠈⠹⡞⡧⡯⢷⠽⡼⢮⠊⠙⠑⠙⠑⠙⠉⠉⠳⡵⡯⣞⢷⢵⢧⠷⡵⡵⡧⡗⠙⠉⠋⠑⠉⠃⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀",

           //2
             @"          ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣰⢞⡽⣝⣞⡵⣶⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣰⢞⣵⡫⣞⣞⡼⡾⣕⣟⣶⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣸⣳⡫⣞⢮⣳⢵⣻⣻⣗⡷⡽⣾⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣀⠀⠀⠀⠀⢀⣜⢷⢵⢝⡮⣳⢽⡽⣞⣿⠈⠝⣯⣿⣺⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣷⢱⢑⢗⣄⠀⢐⣷⢟⣟⣟⣟⣟⣟⣯⢿⠱⡑⣽⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⢸⢸⢴⣵⢿⣻⣮⣟⣞⣮⣾⣮⣾⡎⡎⡎⡎⣿⣤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣤⣤⢟⣟⢯⢯⡺⣝⢞⡼⡮⣳⢝⡮⣺⢼⣺⣻⢧⣧⣿⣾⣴⣤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠑⢋⣿⢫⢛⢏⢟⢫⡛⡏⡟⢏⢟⢻⢹⢳⣗⡷⣯⣗⣷⣳⣗⣟⡿⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡻⡄⢕⢑⢌⢆⠕⢌⢢⢑⠱⡨⡢⡱⡐⡢⡑⢝⠪⡛⡎⣿⠉⡀⡦⡋⡋⡋⠫⡦⣆⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡻⢔⠅⡕⢁⠫⣿⡮⡊⡢⡑⢵⣽⡏⢃⢘⢔⢡⢑⢅⢕⢱⠹⡦⣿⠀⢆⢂⠢⡁⢂⢘⠵⡆⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⣽⢜⠰⡑⠨⠐⠨⠯⢙⣶⢷⡎⠌⠳⡇⢂⠂⠌⠢⡑⢔⠌⢆⢇⢷⢿⡘⡌⡎⡪⡨⡂⠄⠩⢳⡆⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⣈⠿⢁⢂⠡⠨⠈⠌⣌⣩⣯⣡⡁⡂⢂⠂⠌⠄⠅⠌⠢⡑⡕⡜⡌⡿⣾⣜⢜⢜⢰⠨⡊⢌⢪⡇⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠺⣌⣄⠂⠄⠅⠌⠨⢉⠡⠉⠌⡁⡂⠂⠌⠠⢁⠂⢅⠣⡱⢸⢨⢪⢪⢱⢻⣾⢸⢸⠸⡨⢢⡹⠃⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣽⠪⡈⠄⠅⠌⡐⠠⠂⠡⠈⠄⢂⠁⠅⡁⠢⢊⢢⠱⡨⡂⡇⡕⡜⡜⢜⢜⣷⢱⢱⢱⡹⠣⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣯⠣⡐⡈⠄⠡⠠⠡⠨⠈⠌⡈⠄⠌⡐⠠⡁⣊⠢⡱⢰⢑⢕⢱⠱⡸⠸⡸⡸⣷⡕⠇⠁⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⢨⢐⠈⠌⡐⠈⠄⠡⠂⡂⠌⡐⠠⠡⡨⠢⡑⡜⢜⢜⢸⢘⢌⢌⢪⠨⣪⣗⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⢐⢅⢣⠠⡁⠅⡁⠢⠐⡐⡰⠡⡃⡪⡘⢔⢱⢱⠱⣕⢆⢕⠰⡡⡱⡸⣗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⣬⠢⡑⢕⡼⡰⡠⡱⡱⢸⢸⡧⡑⡌⡌⢆⢿⡪⡕⣿⢢⢂⠇⡆⢇⡳⡯⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠨⡊⡢⡹⣟⡯⡮⣮⢮⢾⡇⡒⢌⢌⢪⢽⡮⡋⠛⣻⣼⣼⢸⢨⣗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣤⢳⢱⢌⢆⡳⡗⠀⠀⠀⢰⡜⢕⢜⡌⡎⣮⠝⠀⠀⣴⡹⡲⡘⣜⢬⢗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠿⡵⠷⠧⠷⠗⠀⠀⠀⠀⠸⠯⠾⠮⠾⢮⠊⠁⠀⠀⠑⡳⢗⠷⡳⠉   ",


             //3
            @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀

⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⢰⡲⣶⡂⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⡸⡚⡣⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⣄⠳⡛⡺⢤⢠⡟⡦⡄⠀⠀⢀⡬⣗⣵⢵⡃⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⣔⢇⢎⢌⢚⢥⢤⣄⡤⣄⣤⣄⣤⢓⠪⡨⡢⡊⡽⡄⠙⡽⣜⢧⣔⢗⡽⡎⠛⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢘⡧⢱⢱⠱⡑⢌⢊⠢⡩⠨⡂⡪⢚⠆⡣⡳⡱⡑⡜⣾⢠⣨⠺⣕⣵⢻⢧⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⡌⡇⢕⠡⡑⢌⠪⡐⡑⢌⠪⡐⢌⠢⡑⠔⢌⠢⡊⡜⣾⢹⢢⢫⡻⣺⣳⢵⡣⡄⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⢜⠣⡊⡢⡑⢌⠢⡑⢌⢌⠢⡑⢌⠢⡑⢌⢊⠢⡑⢔⠡⢣⢻⣜⢕⡝⠊⠉⠑⢽⣹⡖⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡸⢇⠕⢌⢂⠪⠮⣎⣌⢆⠢⡑⡌⣆⠧⢗⠅⡅⠕⢌⠢⡑⠅⢇⢯⢯⡳⠶⠖⢖⣀⣁⠁⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠰⢎⡕⡐⢅⠕⠨⠊⣷⣳⠑⠔⡑⠌⢌⢺⡿⣵⠑⠌⡊⡢⡑⢌⢊⠢⣣⣗⡡⠂⠌⠄⠍⠸⢆⣀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢀⡰⡪⢊⠌⡂⠌⠄⡁⡫⠏⢜⣶⣲⡎⠠⠘⠯⢓⠁⡂⡐⠐⢌⠢⡡⡑⡍⡞⣝⣠⢁⢂⠅⠡⢘⢇⣀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠐⡇⣎⠢⠡⠐⠠⠁⠄⢂⠨⣈⢼⢕⡁⡅⠌⡐⢐⠐⡀⢂⢁⠢⡑⠔⢌⠮⣞⣗⢕⢌⢢⠡⡃⢔⢱⡇⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢀⢸⢝⢑⠅⡅⠡⠁⠌⠄⢊⠉⠍⠩⠉⠍⡐⢐⢀⠂⡂⡂⡢⢑⠌⢎⠦⣳⡱⣝⢾⢸⡸⡔⢌⢒⢜⡧⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠐⢗⣜⡔⠕⢌⠊⠌⡐⠈⠄⠨⠈⠄⠡⢁⢐⢀⠂⡂⠢⢂⢊⠢⡡⡡⢩⣪⣫⢷⡳⣕⢕⢕⢱⢸⣇⠁⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠈⢱⡇⡣⠡⢈⠐⠠⠁⠅⠡⠈⠌⡈⠄⡐⢀⠂⠄⠅⡂⢅⠕⡰⢨⢢⢫⢺⢸⢸⡱⣷⢱⣓⠧⠁⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠰⢇⡪⡨⢂⠨⠈⠄⠅⠌⠨⠐⡀⡂⠔⠐⡈⠄⢅⠪⡐⡑⢜⠜⡜⢜⢘⠸⡸⡸⡱⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⢇⡎⠔⠄⠅⢌⠐⡈⠄⠡⠠⠐⠠⢁⠢⡑⠅⢕⠨⡨⠢⡑⢌⠢⡡⡑⢌⠮⡺⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⢧⡫⡪⣊⣂⢢⠐⡈⠄⢕⢈⢌⢢⢑⠌⡊⡢⡑⣌⡌⡢⡣⡑⠔⢌⢢⢫⢺⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⡗⡝⢮⢇⣇⠢⡑⡅⡢⣪⢪⠢⡑⢌⢂⢎⡾⣕⣽⢪⢌⠪⣘⢜⣜⡧⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡕⡇⡏⣎⢾⡳⢗⣜⣜⢮⢾⢾⢐⢌⠢⡡⣳⠻⠉⠈⠷⣱⢕⢕⡕⢮⢯⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣎⢞⢜⢎⢮⡚⠃⠈⠉⠈⠉⣡⡻⡰⡰⡡⣳⠝⠂⠀⠀⠀⣯⢫⢪⡪⡳⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠘⠝⠝⠝⠃⠀⠀⠀⠀⠀⠀⠳⣕⣽⣸⣜⠓⠁⠀⠀⠀⠀⠈⠫⠓⠏⠀     ",

            //4
             @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⡶⣖⢦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⡿⡼⣜⡧⡄⠀⠀⢀⣤⢻⢦⢠⡎⢗⠻⢤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣤⢛⠮⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠛⠳⣽⡹⣥⢬⡳⣱⡫⢁⡝⡎⢔⢥⢑⢜⢣⣤⣠⢤⣠⢤⣠⡤⡛⢔⢔⢕⢳⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢸⡺⣳⣹⡜⢇⡄⢼⡪⡊⢎⢎⢎⠢⡹⠢⡑⢅⠣⡑⢅⠕⡘⢜⢜⢜⢌⣗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢀⣜⢺⡼⣞⢗⢯⢪⡚⣟⡎⡎⠢⡑⢔⢑⠌⡪⠨⡂⠕⢌⠢⡑⢌⠢⡢⡑⢌⠧⡤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠰⣶⡱⠓⠉⠉⠻⣸⡱⣕⢇⠇⡪⠨⢌⠢⠢⡑⢌⠪⡐⢅⠥⡑⢌⠢⡑⠔⢌⠢⡑⢏⡤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠈⢈⣁⡰⠶⠖⢮⡳⢯⢇⠇⡪⢐⢑⠅⢕⢑⠼⡲⣑⡌⡢⡑⢌⢢⣱⠮⠇⡅⠕⢌⠢⢯⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⢀⡰⠎⡊⡁⡂⠌⣐⡹⣧⡑⢌⠢⡡⡑⢌⠢⢑⢸⣞⣯⠱⠐⠌⠢⠑⢵⢷⡣⠊⠜⡐⢅⢅⡣⠖⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⢀⣐⢇⢂⢐⢀⢂⢂⣔⢯⣃⢣⠡⡑⢔⠨⠐⢐⢀⠊⠟⠞⢈⠨⣮⢾⡼⠘⢟⠂⡁⡂⠡⠑⢌⢙⢦⡀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⢐⡗⢔⢐⠔⡡⡡⡱⡹⣞⢮⠢⡑⢌⢂⢂⢁⢂⠐⡈⡈⠌⢠⣈⣊⢯⣘⡈⠄⢂⠐⠠⢁⠡⡑⣌⣒⠏⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⢐⡯⡆⠕⢌⢎⢎⢞⣞⣝⣜⡜⡜⡐⢅⠢⡂⡐⢐⠀⡂⠌⠨⠑⡉⠍⢊⠑⡈⠄⠌⡐⡐⡰⠨⡱⡳⡀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⢙⡮⡪⡸⡸⡪⣳⢺⢮⣳⡑⡑⢌⠢⡑⢌⠢⢂⠂⡂⠌⠄⠡⠐⡈⠄⢂⠐⠠⢁⠢⢂⠪⢪⣢⡪⠗⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠹⢺⢜⡺⣎⢗⢕⢕⡳⡱⡡⢅⠕⢌⠢⠐⡐⠐⡀⢂⠡⠨⠐⠠⠈⠄⠌⡈⠄⠂⡂⢕⢑⣗⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢙⡮⡳⡱⡱⢑⢑⠕⢝⠔⡅⠕⢌⠢⢂⠡⠐⡀⡂⠌⠠⠁⠅⠨⢀⠂⠌⡐⡐⢅⡪⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢐⡯⡎⡎⢔⠡⠢⡑⢅⠪⡐⢅⠕⢌⠢⡑⡐⡀⡂⠌⡈⠌⡈⡐⢐⠨⠀⢆⢪⡢⠟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢐⡯⡪⡪⡂⢕⢑⢜⢔⡑⣌⡢⢡⠡⡑⢌⢢⢂⢂⢂⢂⢐⠠⢐⡐⣅⢇⢇⡮⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢙⣞⢜⢜⡐⢅⢎⢞⣎⣷⡣⢅⠕⢌⠢⡱⡱⣑⢌⠢⡡⡊⡮⡞⡝⡜⣜⡗⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢘⡮⡪⡣⣣⣣⡳⠋⠈⠺⢾⣑⢌⠢⡡⢳⣻⣎⣎⣇⡮⢞⡿⡸⡪⣪⢪⣏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠈⢯⢎⢇⢇⢗⡇⠀⠀⠀⠛⢮⡢⢱⢨⢺⢥⠈⠁⠉⠁⠀⠻⣪⢎⢮⢪⢺⢮⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠑⠛⠝⠃⠀⠀⠀⠀⠀⠙⣎⣧⣳⣕⡝⠁⠀⠀⠀⠀⠀⠈⠛⠺⠙⠇⠁⠀⠀",

             //5
            @"
  ⠀⠀⠀⢀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⡀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⢸⡑⡝⡜⡤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡄⡖⡝⡌⡽⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠸⣂⢇⢯⢦⡹⡴⠤⠤⠤⠤⠤⢤⢤⢣⡣⡳⡕⡅⡯⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢗⡸⡸⡽⡘⡌⡪⡑⢍⢪⢑⢕⠱⠵⣏⢞⢜⢜⡅⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠈⢺⢺⠨⡢⠱⡌⡮⡘⢔⢑⢔⠱⡑⢵⢣⢱⠑⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢸⡘⣗⣷⢵⢕⡏⣮⢾⣺⣏⢣⢑⢕⢇⠁⠀⠀⣤⢤⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠈⣪⠪⡋⢎⣟⢎⢢⢋⢟⢑⢌⢆⢮⠃⠀⠀⠀⢸⢜⢵⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠘⢜⢌⢎⠎⢕⢣⠣⢢⢱⢰⠕⠯⠦⡢⢆⣄⢞⢢⠳⠁⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⡞⡜⡜⡔⠕⢅⢣⢑⡑⢍⢪⠨⡢⢓⢗⠕⠁⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⡪⡘⡌⡌⢎⢊⠆⡕⡸⡨⡢⡣⡊⢆⢕⠇⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡏⡖⣕⣌⢎⣆⠣⡊⣾⢼⢮⡳⢕⣑⠌⡧⡀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⠯⡸⡸⠀⠁⢑⡕⢅⡏⢯⢮⠇⠈⢼⢘⢜⠄⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠗⠝⠎⠀⠀⠀⠳⠕⠗⠕⠽⠺⠀⠨⠯⠺⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
  ",
            //6
                @"⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠹⡦⡀⠀⠀⠀⢀⡤⠏⢳⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢰⡇⣱⣩⡻⣝⣟⢽⡻⣅⡧⢸⡂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⣼⢶⠳⠣⢟⡞⡮⡳⠝⠮⣞⣞⡧⠀⢀⠤⠤⠤⣀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢀⣴⡻⡽⢹⡦⣆⠧⢧⡦⡞⢱⠑⢧⡻⣯⢀⢠⡰⡴⣄⠉⢢⡄⠀⠀
⠀⠀⠀⠀⠀⠀⢀⣽⠍⠀⠀⡙⣥⣬⡄⠩⠉⠀⢀⠀⠹⣽⠲⢷⠛⢿⣟⡷⡀⢧⠀⠀
⠀⠀⠀⠀⠀⠀⠀⣹⣳⣐⢀⡇⡬⡯⣄⣂⢠⠈⡀⡴⡽⡯⡷⣦⣡⣿⢯⡯⡣⡺⠀⠀
⠀⠀⠀⠀⠀⢠⡾⡽⣝⣗⣆⠹⠥⠤⠤⢎⠫⣰⢪⢯⢯⡺⣝⢮⢟⣟⣯⣮⠚⠀⠀⠀
⠀⠀⠀⠀⠀⣸⡽⣺⠂⠀⠘⠊⠌⠊⠌⠀⠘⠀⢀⠀⢳⣫⣞⢽⢽⣯⣺⡺⣇⠀⠀⠀
⠀⠀⠀⠀⢐⢯⡫⣾⡄⠅⠀⠐⠀⡁⡀⠄⠁⠀⢄⢢⡯⣺⡺⣝⢵⣻⡼⣪⣿⠀⠀⠀
⠀⠀⠀⠀⣰⣟⡮⣗⣿⣬⣌⡢⡑⡐⠔⡠⡡⣑⢴⢻⣽⡺⡮⣳⢽⣺⢹⡺⣺⡆⠀⠀
⠀⠀⠀⠀⣺⢺⣪⢓⠸⡇⠙⢦⣢⢢⢑⢰⢰⠸⣸⣽⢏⢝⣞⢵⢫⡺⣤⡙⡺⢧⠀⠀
⠀⠀⠀⠀⠪⡈⠊⠂⡬⠃⠀⠈⠈⠺⠼⡬⡮⡷⢿⠺⠣⡀⠳⠝⢀⠇⢹⢌⠂⢹⠀⠀
⠀⠀⠀⡄⠎⠂⠐⢰⠁⠀⠀⠀⠀⡤⢳⢫⢪⢎⡇⠀⢀⠃⡀⠄⢸ ⡞⡌⢠⠈⡆⠀
⠀⠀⢸⢬⢆⡧⣨⢼     ⠢⡑⢝⠹⡑⡓⡫ ⡑⣧⢇⣖⢬ ⠇⠋⠋⠋⠙⠀⠀
",//7
            @"
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡀⡀⣄⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠋⠛⠍⠉⠛⣆⠀⠀⣰⣲⡄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣈⡷⣺⣟⢾⢩⢹⢂⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⣴⣺⣲⢴⣄⣀⣀⢀⢔⣎⢧⢧⢳⣻⡊⣯⡿⡰⡑⡽⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢶⢶⣻⢾⡽⣽⣺⢽⣏⡗⣗⢵⡫⣞⡵⣗⣷⢩⣿⣽⢍⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢠⣺⣽⢾⡻⣼⡯⣺⢭⡳⣻⣽⢯⣻⣳⢿⡽⡏⡦⣳⡱⡿⡤⡤⡤⣄⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢀⡾⡝⣵⣻⢳⣟⣗⢷⢽⢽⣞⡯⣷⣻⣳⡏⠀⣸⡎⢯⢿⣽⣫⣟⢿⣔⡀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⣺⢕⣮⡵⣧⣄⣻⣺⣻⣟⢯⢿⣽⣳⢿⡸⣁⣪⣷⢟⢮⡓⣶⡒⠝⢕⢯⢿⣆⠀⠀⠀
⠀⠀⠀⠀⠀⣺⢝⣦⢪⢩⢚⠾⣗⣿⣺⣽⣳⢷⢯⡳⡽⠘⢽⢮⢳⢱⡙⣾⡺⣖⠀⢫⣗⡿⡄⠀⠀
⠀⠀⠀⠂⡀⢸⡷⣕⢭⠁⡵⣟⣾⢮⢝⡞⡞⡟⡯⡞⠉⢸⢽⢸⠂⣟⡷⣯⢯⡻⠀⢸⢮⣟⡇⠀⠀
⠀⠀⠀⠀⠐⢀⠹⠯⣗⣿⣺⣟⠞⠀⠀⠉⠉⠉⠉⠀⠀⠘⢽⢞⡲⡈⠛⠚⠉⠀⢀⣺⢽⡽⠁⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠐⠐⠈⠂⠢⢂⠄⠄⠄⡀⠀⠀⠘⣿⡽⣖⡢⡤⢔⣪⡵⣯⡿⠁⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠈⠀⠁⠑⠈⠔⠙⡫⢷⣯⢿⠽⠛⠁⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
",

            //8
            @"
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⢀⣀⡀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⠀⠀⠀
⠀⠀⠀⢰⡺⢵⠃⠀⠀⠀⠀⠀⠀⢠⡛⠧⣆⠀⠀⠀⠀⠀⠀⠀⢀⡼⡛⡇⠀⠀
⠀⠀⢠⡿⣪⠏⠀⠀⠀⠀⠀⠀⠀⠸⡌⡊⡎⣷⡤⣖⢶⠶⢦⡾⡟⡰⢡⠇⠀⠀
⠀⠀⢸⡪⣗⠀⠀⢀⣠⢤⢜⢗⢟⢗⣇⡪⡪⡪⣎⢮⢪⢎⢧⢳⢹⢌⣺⠀⠀⠀
⠀⠀⠸⣞⢮⣇⡶⡝⡼⡸⡕⣝⢜⢮⣺⡣⡫⡏⡱⢧⣓⡯⡪⢾⡎⡯⣧⠀⠀⠀
⠀⠀⠀⠸⣧⢇⢇⡏⡞⡜⣎⢞⢜⢟⣧⢗⢭⢻⢤⢟⢗⣝⣧⡥⣞⢵⣹⠛⠀⠀
⠀⠀⠀⢠⡺⡕⡇⡗⣝⣜⢎⡎⡧⡫⡫⣯⣺⡸⣼⣮⣴⢿⣆⡎⡮⠾⡉⠁⠀⠀
⠀⠀⠀⣾⢕⢇⡏⡞⣼⢗⢕⢵⣱⢝⢜⡜⣟⣮⣻⡳⢕⠯⣟⡕⠋⠀⠀⠀⠀⠀
⠀⠀⢠⡻⡜⣕⢵⣹⢿⡮⣷⣵⡫⡎⡧⡳⣱⣳⣳⣽⣽⣝⢯⡇⠀⠀⠀⠀⠀⠀
⠀⠀⣞⢜⢮⡪⠞⢩⣗⢯⡾⢛⢮⢺⢸⡺⠗⠓⠉⢹⣗⢽⢵⢇⡀⠀⠀⠀⠀⠀
⠀⠀⣯⢪⡯⠀⠀⠀⣯⡳⣇⠌⣿⢱⢣⡇⠀⠀⠀⠀⠘⣽⢕⢟⢵⠀⠀⠀⠀⠀
⠀⠀⢗⢠⠙⢲⠀⠀⠘⠺⠼⠷⠀⢯⠎⠚⠦⡀⠀⠀⠀⠈⠫⡎⠃⢑⢢⠀⠀⠀
⠀⠀⠈⠊⠑⠁⠀⠀⠀⠀⠀⠀⠀⠈⠖⠖⠧⠁⠀⠀⠀⠀⠀⠁⠑⠼⠈⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
",
            //9
            @"⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢐⢕⢘⠔⣄⠀⣀⣀⣀⣀⠀⠀⡠⠔⡑⡅⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠠⡃⡆⡕⢐⠩⡐⡰⢐⠰⡉⡃⡢⡑⢅⠢⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠵⡨⢂⠡⡡⢂⠪⡐⡁⡂⡂⡑⡸⡐⢅⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⡪⡊⡦⡨⠐⢄⠑⠔⣐⡀⡂⡂⡂⢅⢝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢀⢨⠪⠨⢗⠇⡑⠄⠅⣹⢵⣻⠠⠢⡂⢆⡺⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠠⠰⢭⠊⡂⠀⢪⠃⠠⠐⠩⡑⢌⢂⠪⡰⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠠⠤⢌⠑⠤⣁⢁⠁⡁⠄⡡⡨⡢⢕⠕⣇⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢨⢃⠂⢄⢙⢄⠞⢄⠑⠄⠃⠅⡊⢌⠢⡑⢔⢘⠕⡄⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠑⠸⡰⡑⡸⠨⢀⠐⠈⡨⢈⢂⠆⡑⠨⡐⢅⢑⢘⠥⡀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠈⢎⡔⡩⢂⠠⢁⢂⠢⠡⡂⡅⢕⠌⢢⠨⢂⠱⣅⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠈⠘⢔⢕⠐⠅⢅⢢⢃⠢⡨⢂⠌⡢⢑⢜⡄⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡠⠞⡐⡡⢑⡸⣘⢆⡑⢄⢃⢂⢪⢘⡖⠜⡆⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⡀⡂⡐⠠⠕⢳⢁⢂⠌⡢⡱⡱⢱⢑⢌⢪⠃⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠊⠈⠁⠀⠀⠑⠁⠉⠊⠈⠑⠓⠖⠍⠀⠀⠀⠀⠀⠀
",
            //10
            @"⠀⠀⠀


⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⡤⠤⢤⢀⢰⠄⠀⠠⡢⠔⠤⡢⡄⠀⢠⢄⢤⡠⠀⣆⠀⠀⠀⠀⠠⡠⠀⠠⡄⠀⠀⠤⡰⡠⡄⠀⣲⠀⠐⠴⡰⠔⠄⡄⡮⠀⠀⠀⡰⡄⠀⠀⠀⢠⡀⠀⠀⠀⡔⡄⠠⡤⡠⡤⠀⣢⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⡗⠀⢸⠂⢨⡣⠀⠰⡔⠆⠖⠜⠌⠀⠀⠀⢰⡑⠀⣇⢆⠀⠀⠀⡨⡮⡀⡸⣪⠀⠀⢄⡄⣜⠅⡄⡮⠀⢀⢰⠝⡤⡘⢙⡆⠀⠢⠓⡉⢘⠱⢂⠀⢸⡂⠀⠀⠀⡺⡀⢘⡆⠀⠀⠀⡎⡆⡄⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣏⢄⡸⡁⢸⡂⠀⠘⠎⠖⠲⠒⠆⠀⢀⢔⠕⠀⠀⡧⠀⠀⠀⠈⠊⠈⠑⠁⠈⠉⠀⡣⣅⣀⣀⡀⢽⠀⠘⠈⣄⠈⠀⣔⠁⠘⢉⡃⡃⡃⣋⡊⠂⢸⣂⢄⢄⠄⡺⡀⢐⣇⣀⢄⠄⡺⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠈⠀⠁⠀⢸⡂⠰⠸⠰⠲⠸⠰⠒⠆⠁⠁⠀⠀⠀⡗⠀⠀⠀⠢⠣⠲⠱⠸⠰⠱⠀⠈⠀  ⠀⠀⣳⠀⠀⣔⠕⡥⡜⠎⣆⠀⠸⣌⢌⢌⢜⠆⠀⠀⠀⠁⠁⠁⢵⠁⠀⠀⠀⠁⠁⣝⠀⠀⠀⠀⠀⠀⠀⠀⠀
 ", //11
            @"⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡰⠨⣓⢕⢆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣫⡪⣲⡱⣹⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠠⠤⠳⠵⠽⠬⠷⠵⠽⠥⠤⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⢀⠂⠀⡠⢄⠤⡠⢠⠠⡠⠠⡠⠠⠠⡠⠠⢄⠤⢤⣀⠀⢄⠀⡠⠄⡄⢠⠀⡠⠀⢄⠀⡵⢄⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠨⠀⢰⢑⠔⡅⢪⠂⢕⠠⠣⡈⢆⠑⡄⡣⡊⡲⡐⢼⠀⢂⠠⡇⢪⢈⠢⡑⢌⠨⡂⢇⠱⡈⢆⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠕⠀⣸⠐⡕⢘⠄⡣⠊⠜⡠⢑⢄⠣⡪⡐⢕⢌⠪⣸⠀⢂⠠⡇⢢⠡⡑⠔⡠⠣⡪⡘⢆⠱⡌⢆⠀⠀⠀⠀⠀⠀
⠀⠀⠀⡃⠀⣸⠨⡈⡢⢊⠔⠡⡑⢄⠣⡌⡪⠰⡡⡃⡪⢊⢼⠀⡁⠠⡇⠢⢂⠜⡐⢕⠱⣨⠮⣼⡄⢳⡈⢆⠀⠀⠀⠀⠀
⠀⠀⠀⡅⠀⠘⠦⠜⠤⠥⠪⠢⠎⠦⠵⠬⠜⠬⠦⠕⠼⠴⠕⠀⠌⠀⠣⢜⠤⡣⢕⠥⡣⣜⢳⡪⣟⡄⠱⠍⢆⠀⠀⠀⠀
⠀⠀⠀⠆⠈⠠⠀⡐⠀⠂⡐⠀⡦⢤⡄⢀⠐⠀⠄⠂⠠⠀⠄⢀⠃⠈⣤⢤⢤⠀⡀⠠⠀⠈⠃⠋⠊⠄⠠⠀⠄⢑⡀⠀⠀
⠀⠀⠀⡍⣍⢍⢍⢪⢩⡩⣊⢍⢞⠴⣩⢊⢍⢍⢍⢍⢍⢍⢍⢕⡍⡣⡩⣑⢍⢕⡩⡩⡩⢍⢍⠝⣌⢫⠩⡩⡣⡱⡅⠀⠀
⠀⠀⠀⡲⠰⡡⡣⡑⢆⠵⡱⢕⢕⡣⡣⡳⡱⡃⢎⠢⡣⠪⡢⡱⡌⢎⠔⡥⢊⢆⠎⢦⢑⢕⢌⢎⢢⡃⡳⢸⠅⡍⡺⠀⠀
⠀⠀⠀⡊⠑⠈⠂⠙⠐⠉⠊⠑⣕⢕⡍⠊⠊⠑⠑⠑⠑⠑⠘⠐⡑⠑⠑⠘⠈⠂⠙⠐⠑⠈⠂⠑⠁⡃⠑⠙⣌⢢⡹⠀⠀
⠀⠀⠀⡅⠈⠄⠡⠀⡡⣠⣡⣠⡃⠓⢈⠀⠂⡐⠀⠂⡐⠀⢂⠐⠄⠂⠠⠁⢨⡤⣳⡺⣖⣥⣌⠠⠁⠂⡈⢀⠈⡀⢕⠀⠀
⠀⠀⢰⡹⣙⢷⠀⣞⢞⢵⢕⡵⣙⢷⣆⠈⠠⠀⠌⠠⠀⠌⠀⠄⡡⠈⡀⣱⢯⢺⢕⡝⡼⡜⢮⢷⡌⠀⢾⡩⣏⣝⢝⡆⠀
⠀⠀⠸⣜⢵⣹⢸⡝⣎⢷⣙⢞⢮⡲⣝⡆⡑⡐⠢⡂⡑⠌⠢⡂⠢⢂⢜⡷⡱⣣⢟⣜⡳⣝⢕⢯⢾⠀⡷⡱⣎⢮⣣⠃⠀
⠀⠀⠀⠉⠊⠁⠹⣎⢮⡣⣏⢟⢜⢧⡪⣟⠚⠚⠓⠛⠚⠙⠓⠙⠙⠊⠊⢹⡪⢾⡸⢧⡫⣺⢕⢧⡻⡇⠙⠘⠈⠃⠁⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠳⣕⢝⢮⢳⢝⢮⡺⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⡜⡳⡳⣝⢵⡱⠏⠋⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠔⠀⠉⠓⠙⠳⠙⠁⠔⠠⠂⠔⠠⠠⠀⠄⠠⠠⠠⠀⠢⠐⠀⠉⠛⠚⠊⠃⠡⠊⠐⠀⠂⠀⠀⠀⠀⠀⠀

", //12
            @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣶⣿⣫⣻⢎⢿⢜⢯⡾⣫⣍⡻⣮⡻⢦⢛⡟⠷⣶⣰⣟⣷⡀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⣀⣼⡿⡫⣚⢮⡺⣵⡹⣫⢷⣟⣼⣿⢽⣪⢓⡕⣕⢝⡎⢖⢬⠫⣿⡇⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢀⣠⣿⡫⣞⠵⡩⡺⡑⣷⢝⣽⢯⣿⢷⣫⢞⡵⣯⣮⣢⢍⣛⣗⣕⠳⣜⣷⡆⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣴⡿⣝⣝⡜⡪⣕⢝⡬⣫⢿⣽⣟⡷⣝⡵⡓⡮⣻⣧⣻⣯⢞⠿⠮⣗⣻⢎⣿⡀⠀⠀⠀
⠀⠀⠀⠀⠀⣸⣿⡺⣳⢜⡕⣕⢎⢖⣝⢞⣿⣿⣾⢯⣟⣮⡻⣜⢧⣯⢿⢴⣍⣻⣿⣛⣿⡷⣽⡇⠀⠀⠀
⠀⠀⠀⠀⣰⣿⡳⣝⢣⠳⡕⣎⢾⡽⣞⣿⣺⣿⣯⣿⡿⣜⣽⣳⣟⣷⣿⠶⣧⣪⣙⢿⣛⣿⡞⠀⠀⠀⠀
⠀⠀⠀⣨⣿⣞⢽⡪⡱⡫⡪⡺⣷⣿⣿⣵⢿⡾⣿⣿⣻⣟⣮⡷⣯⣿⡿⣇⣼⣏⣯⠋⣽⡿⠂⠀⠀⠀⠀
⠀⠀⢠⣾⣷⡽⣳⢕⡯⡪⡺⢜⢿⣿⡾⣷⣻⣽⣞⣯⣿⣯⣿⣻⡷⡿⣽⣫⣯⣻⣿⣿⣿⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠼⣷⢿⣹⡧⣻⢜⡜⣕⢕⠿⣿⣿⣽⡾⣷⣻⣽⣟⡿⣿⣯⣻⣉⠸⠶⠃⣸⣷⢽⣇⡀⠀⠀⠀⠀
⠀⠀⠀⠠⣿⣿⣷⣻⢵⡻⣎⢎⣳⡱⣙⢟⣾⣿⣯⣿⢾⣟⣿⣿⡿⣷⣝⣺⣪⣫⡿⣣⢟⡮⣷⡄⠀⠀⠀
⠀⠀⠀⣽⣿⣿⣟⣷⣿⣹⢞⢮⣳⡕⣝⢦⢯⣿⣾⣿⣿⣿⣿⣻⣿⣧⠀⢻⣯⢿⣽⣏⢷⡽⣪⢿⡀⠀⠀
⠀⠀⢘⣿⣷⣿⣿⣻⣷⣯⣯⣳⣮⣟⣮⣿⣟⢷⡍⢻⣿⣾⣿⣻⣿⣿⠀⠈⣻⣻⠛⢻⣷⣝⣽⣽⡇⠀⠀
⠀⠀⣿⣿⣿⣳⣿⢿⣏⠙⣾⣷⣹⡟⣿⣜⠻⣎⡇⢠⣾⣿⣿⣯⣿⣧⣤⠀⠈⠂⠀⢠⣟⣼⣫⣿⠃⠀⠀
⠀⠀⠿⣷⣻⣽⣺⣿⣹⡇⠀⠈⢳⠇⠀⠻⠀⠋⠀⠸⢿⣿⣾⣷⣟⣿⣻⣷⡆⠀⠠⢟⣽⣯⠟⠃⠀⠀⠀
", //13
            @"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⢸⣷⣀⠐⣿⣦⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⣤⡀⢻⣿⣦⡽⣿⢷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠀⠀⠀⠀⠀⠀
⠀⠀⠹⣿⣶⣽⣾⣿⣻⣿⣯⣗⣄⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⡆⢀⣼⣟⠀⠀⠀⠀⠀⠀
⠀⠀⢠⣈⢻⣷⢿⣽⣿⣽⣷⣻⣞⣗⣞⣗⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⢨⣿⣥⣿⣟⠁⠀⠀⠀⠀⠀⠀
⠀⠀⠈⠻⣿⣾⡿⣿⣾⣯⣿⢿⡾⣞⣷⣳⣳⠀⠀⠀⠀⠀⠀⢀⡀⣼⣿⣟⣿⣽⢷⠃⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⢸⣷⣿⣿⢷⣿⣻⣿⢿⣻⣽⡿⣞⡄⠀⠀⠀⠀⣀⢸⣿⢿⡯⡯⡷⣝⠇⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠉⢳⣿⣿⢿⡿⣿⣿⢿⣽⣿⡯⣧⠀⠀⠀⣐⣿⣿⣟⡯⡯⡯⣯⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠈⢹⣾⣿⡿⣟⣿⣿⢯⣿⣟⣗⣧⡀⣤⡿⣿⢷⣳⢯⢯⠏⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠈⠹⣿⣻⣿⣿⣽⣿⣽⡿⣿⣺⡽⡽⣽⣫⠛⢁⠉⡉⠒⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⢿⣿⣾⢿⣻⣷⡿⣿⣿⣿⣟⢷⢵⠡⠀⡂⠰⡲⣤⠱⣄⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⠻⣿⣿⢿⣿⣿⣻⣞⣗⡯⣗⣧⠱⡨⡢⠞⠧⠧⡢⠇⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣈⣭⢿⡾⡯⣞⣿⣽⣿⣽⡿⠉⠀⠀⠀⠀⠁⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠰⣐⡐⡐⡐⡔⣝⣝⣿⣻⣿⢯⢷⣽⣿⣾⣯⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⡝⡜⡌⢎⢓⢝⠯⣟⢿⣟⣯⢏⡻⡻⣺⢷⢄⢔⠲⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠬⠦⢇⠵⠑⠃⠈⠹⠋⡵⡵⢝⡞⢻⢜⠚⠻⠝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀

⠀⠀⠀⠀⠀⠀⠀
", //14
            @"⠀
⠀⠀⠀⠀⠀⠀⠀⣠⡀⡀⠀⠠⣤⡴⣷⣳⡳⡧⣿⣽⣢⣽⣟⡄⢀⠀⠀⠀⠀⡤⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠘⣯⣗⡶⣶⣾⣟⣾⣳⣟⣞⡮⡷⣧⡫⡮⣧⢽⡀⠀⡤⣞⡯⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢿⢽⣟⣶⡵⣝⢷⣻⡾⡽⣾⣻⡺⣪⢗⡽⣻⢷⣾⣯⡳⡯⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢀⣶⢿⢽⢿⣿⣳⢯⣗⢯⡺⣜⠮⣞⢮⣫⢺⠵⣝⢵⣷⣿⠉⠀⠀⠀⠀⠀⠀⠀⠀
⢀⣼⡂⠀⠀⠀⣸⡿⣽⢽⣯⣯⣻⣽⢗⣯⣳⣕⢗⣝⢮⣣⢳⣹⢝⢮⢳⢕⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀
⢠⣿⢝⣶⠀⠀⣴⢯⡯⡯⡷⣻⢾⣽⣺⢳⢽⣻⣾⡻⣞⣜⢞⣽⢾⣯⡺⣝⣽⡏⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠸⢯⣷⢿⣂⢰⣿⢽⡽⣕⢯⢯⣟⡷⣝⣵⠟⢳⡿⣽⣝⣾⣟⡟⣟⣽⢾⣻⣺⢷⡀⠀⣲⠀⠀
⠀⠉⠚⠻⣾⢿⣽⡽⡯⣳⢽⣽⡷⣯⢷⡻⠀⢹⣯⢗⣯⢗⣗⢟⡵⡗⡟⡟⣷⣫⣯⡠⡞⢔
⠀⠀⣀⣾⢯⣿⢯⣿⢵⢯⢷⣿⡿⣽⣗⢈⠨⢹⡿⢝⣗⣽⢽⢝⣮⣫⣺⣪⡫⡿⠍⡂⡜⠇⠀⠀
⠀⠀⠀⢼⡷⣯⣻⣟⣷⡯⡯⣟⢾⣻⣽⣟⡦⡢⣂⣾⡧⣿⣗⡯⣟⣟⣞⢾⣻⡺⣿⣜⠞⠀⠀⠀
⠀⠀⢸⣿⣻⣞⣾⣟⣿⣽⢽⢽⢽⣯⣿⣟⣿⣻⡯⣿⢿⡽⣻⣿⣷⢿⣻⣿⢳⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢀⣿⣽⣟⣾⡷⠟⠋⠿⣽⣪⣫⢾⣷⣿⠿⠯⢿⡽⣟⢄⣾⣻⢾⢿⢏⣪⣼⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢽⣟⣾⣟⠀⠀⠀⠀⠀⢻⣳⣕⢗⣿⣻⡀⠀⠘⢿⣽⣫⡷⡿⣟⣟⣯⣿⢷⣻⡤⠀⠀⠀⠀⠀⠀⠀⠀
⢰⣿⡽⣯⣿⡆⠀⠀⠀⠀⠈⣿⣪⡷⣕⣿⣿⣦⠄⠀⠙⠳⡿⡯⡷⡛⢿⣟⣯⣯⢿⡆⠀⠀⠀⠀⠀⠀⠀
⠘⠓⠛⠛⠛⠃⠀⠀⠀⠀⠘⣿⡾⣽⣾⡾⡏⠉⠁⠀⠀⠀⠀⠀⠀⠀⠈⠹⣿⢾⣻⣿⣷⠀⠀⠀⠀⠀⠀
",
 //15
            @"⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⢤⢤⢤⢤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⡫⣎⢮⡣⡳⡕⡮⣪⢳⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡼⡪⡞⣜⢦⡫⣚⢮⠺⣜⢕⡳⡂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡯⣚⠮⠪⠊⠊⠣⡫⠳⠕⠹⢜⡅⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡰⠙⡆⠀⢖⢆⡀⠐⡄⣠⠂⠀⡨⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡇⡳⠸⡄⠀⠉⠋⡞⠕⢱⠊⢗⢩⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⣘⠀⢸⢣⢎⣩⣄⣄⢼⣉⡡⡴⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⢼⡀⣞⢥⢎⡤⡊⣒⢑⢪⡺⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡠⡖⣝⠵⣅⠋⢮⢦⢅⡓⣙⢘⡑⡧⡃⠀⠀⠀⠀⠀⣠⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⣠⡫⣣⢝⢮⡪⡳⡱⢧⡀⠣⢫⡪⣣⢫⠺⡜⣝⢲⢄⠀⠀⣪⠎⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⡴⣣⢝⣜⢕⢧⡹⣪⢫⢎⣝⢵⡒⡖⢦⡲⣚⢮⡪⣣⡫⡆⡜⢡⠃⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠰⣕⠵⡣⡳⡱⡃⢮⡣⡳⣕⠵⣕⢝⢎⢗⢵⡱⡇⠝⠘⠎⣔⡡⢃⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⡗⡵⡹⡕⢧⢚⠵⢨⡺⡱⣕⡫⣪⡣⣫⠳⡕⠑⠀⠀⡉⠓⢄⡸⣣⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢐⡝⡼⡱⣝⢕⡇⢹⡐⣝⢮⢪⢞⢜⣜⠕⠋⠀⠀⠀⢤⡈⢧⢜⢈⢧⡃⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢨⢎⢯⡪⣠⢅⡋⣣⡨⡤⣖⢲⠢⣞⢰⠀⠀⠀⢀⠴⡢⢈⢖⢦⢫⢎⣝⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⣣⢏⢶⠃⣰⢪⡳⡱⡝⡼⣌⢷⡁⢷⢌⡠⠤⡖⠏⣐⢵⡹⡱⣇⢸⢕⢵⠅⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⣪⢣⢧⢜⡕⣧⡹⣪⢺⢪⢎⠎⣹⣢⡑⢏⡼⣍⢯⢪⢧⡹⡪⠆⢸⢕⡳⣝⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠐⠙⠊⠑⠙⠐⠑⠃⠃⠁⠊⠊⠃⠊⠊⠓⠑⠊⠓⠑⠃⠙⠊⠃⠘⠑⠑⠙⠀⠀⠀⠀⠀⠀

",
 //16
            @"⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⢮⡻⣝⢦⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣞⢵⡫⣞⢵⡫⣞⢧⡲⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⡴⣫⢞⡵⣝⢮⠓⢵⡫⣞⢵⡻⡤⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⣀⢶⢲⡲⡲⣏⢾⡱⣏⢞⢮⡳⣫⡐⢫⡞⡵⣝⢞⢮⣓⠒⠒⢖⢖⢖⢖⢖⢖⢖⢖⢖⢖⢖⢖⠒⠒⠒⢖⢖⣆⠀⠀⠀⠀
⠀⠀⠀⠀⢼⠃⣠⡼⣫⢞⡵⣫⢮⣫⡳⣝⢮⡫⣆⠊⠻⣜⣝⢮⣝⣥⡄⣟⠠⣤⣤⠀⣤⣤⠀⡇⢠⣤⡄⢠⣦⣤⡤⢸⢮⠀⠀⠀⠀
⠀⠀⠀⠀⣺⣀⣉⣈⣁⣉⣈⠷⠱⠧⡻⣜⢧⡻⣜⢽⣄⡀⠈⣑⣈⣈⢊⣹⣀⣃⣘⣀⣉⣈⣀⡇⢸⢎⡇⢈⡂⢁⣑⢸⡳⠀⠀⠀⠀
⠀⠀⠀⠀⢼⠀⣍⣌⣍⡌⠙⠒⢲⣄⢥⣌⢤⡡⡬⡤⣌⡤⣠⠏⠋⠁⠠⡃⢨⣌⠨⣌⣌⣅⠉⡆⠘⠙⠃⠘⠃⠘⠃⢸⣝⠀⠀⠀⠀
⠀⠀⠀⠀⣺⠀⣯⠈⠈⢸⡝⣧⠀⡇⢰⡤⣦⢴⠄⢴⢤⣦⠀⡇⢸⡇⠈⡇⢸⢧⠈⠈⢸⢮⠀⡏⢉⣉⠩⡋⣉⣉⣉⠱⣝⠀⠀⠀⠀
⠀⠀⠀⠀⢼⠤⡤⡥⡤⡤⡤⡤⣤⠯⡤⣤⢤⣤⢤⡤⣤⢤⠤⡧⡤⡤⢬⠆⢤⣤⠀⣤⣤⣤⠀⡇⢸⡕⢨⠂⠙⠘⠑⢨⢯⠀⠀⠀⠀
⠀⠀⠀⠀⣺⠀⡶⡴⡆⢰⢖⡄⢸⠀⡶⢴⠀⡇⢰⢦⡲⣦⠀⡇⢰⠆⢐⠇⠚⠑⡀⠛⠂⠓⢀⡇⠘⠊⢰⠅⣤⡴⣖⠮⠓⠀⠀⠀⠀
⠀⠀⠀⠀⢼⠀⣉⠈⣁⣈⣁⡁⢸⠀⢹⡳⠀⡇⢈⣈⢈⣈⠀⡇⢈⡁⠰⠉⣉⡉⢉⡉⢹⡉⣁⣉⠉⣉⣁⣟⡎⠺⠈⠁⠀⠀⠀⠀⠀
⠀⠀⠀⠀⣺⢀⡛⣀⣋⢃⣙⣁⣸⠠⠤⠤⠤⡇⠘⠑⠠⠟⠀⡇⠸⠭⢘⠀⠯⠃⠺⠇⢸⡂⡙⡊⢁⡽⣪⠎⠃⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠘⠫⡎⢁⣉⣉⡈⣈⣌⠀⡶⡶⠀⣇⣠⣀⣄⣄⣠⠗⠒⠂⠚⠐⠒⠒⠒⠂⢺⠄⣯⡃⢸⢎⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠠⡇⠸⠏⠽⠂⠽⠮⠀⣙⣘⠀⡇⢠⣤⢤⣤⠀⡇⢸⣻⠀⣟⡇⢸⣫⢇⢸⠂⣀⡁⠸⣝⢄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⢀⡇⢰⢶⡲⡂⢶⡲⠀⠉⠈⠀⡇⠈⠈⠁⠁⠀⡇⠘⠚⠀⠛⠂⢸⢵⡃⢸⡁⡽⣲⠂⠈⢹⡖⣤⡀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣇⣈⣉⣉⣀⣉⣉⣀⣋⣋⣀⡇⠘⠛⠈⠛⠁⣟⡝⠯⠻⡴⣄⣀⠈⠃⢸⣄⣉⣉⣁⣀⣀⣘⣱⠝⢽⡄⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠈⡇⢨⡌⢨⡬⡌⢱⠈⣤⢥⠁⣥⢤⣤⡄⢀⡷⡕⠇⠀⠀⠈⠓⠫⠳⣦⢼⡀⠁⣥⢥⠉⣬⢤⡥⠄⢳⡅⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠐⡇⢠⡤⢠⣤⡄⢸⠀⠀⢤⣤⣤⣤⣤⡤⣼⢝⠂⠀⠀⠀⠀⠀⠀⠀⠀⢻⢼⠀⠓⠙⠀⣤⢤⠀⢠⡟⠆⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠐⣇⣈⣁⣈⣈⣀⡼⣀⡶⡺⡎⠆⠁⠈⠈⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠑⣟⡲⣖⣀⣁⣉⣰⢾⠙⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠛⠘⠉⠙⠉⠋⠚⠑⠙⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠋⠙⠊⠂⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
"
 //17
};
    }
}
