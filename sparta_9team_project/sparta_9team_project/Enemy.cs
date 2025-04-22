using System;

namespace sparta_9team_project
{
    public static string enemypicture[] = new string[3]; //enemy asciiart를 넣어야함..(아직 안넣음)



    public enum enemytype
    {
        chihuahua,   //치와와
        husky,       //허스키
        mortorcycle  //오토바이

        // 더 추가가능...
    }
    public class Enemy
    {
        // character class에서 공격력, 레벨등등의 정보를 상속받음
        private string attackname { get; set; } // 공격 이름(ex, 물어뜯기...)
        private enemytype enetyp { get; set; }
        private string enemypic;
        private int maxhp;

        Enemy(int level, int attack, int defense, int hpoint, int gld, string atkname, enemytype type)
        {
            lvl = level;
            atk = attack;
            def = defense;
            hp = hpoint;
            maxhp = hpoint;
            gold = gld;
            attackname = atkname;
            enetype = type;                     // 적 종류 체크
            enemypic = enemypicture[(int)type]; // 적 모습 체크
        }

        public displayenemypicture(int x, int y) // 적의 모습을 화면에 띄우는 함수
        {
            PrintAsciiAt(enemypic, x, y);
        }

        public displayenemyinfo() // 적의 정보를 화면에 띄우는 함수
        {

        }
    }
}
