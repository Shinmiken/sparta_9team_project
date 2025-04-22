using System;



namespace sparta_9team_project
{ 

    public enum Enemytype // 적 타입 enum형태로 표현
    {
        chihuahua,   //치와와
        cat,         //고양이
        husky,       //허스키
        mortorcycle  //오토바이
        // 더 추가가능...
    }


    public struct Enemyinfo // 적 정보를 모아놓은 구조체
    {
        public string nm;
        public int level;
        public int attack;
        public int defense;
        public int hpoint;
        public int gld;
        public string atkname;
        public string enepic; // 적 그림
        public int mhp; // 최대 hp
        public Enemytype enetyp; // 적 타입
    }


    public class Enemyinfos
    {
        public static Enemyinfo[] enemyinfos = new Enemyinfo[4]; // 각 enmyinfo를 미리 배열에 저장
        // 0, 1, 2에 각각 enemy 정보 저장...
    }

    public class Enemy : Character
    {
        // character class에서 공격력, 레벨등등의 정보를 상속받음
        private string attackname; // 공격 이름(ex, 물어뜯기...)
        private string enemypic;
        private int maxhp;
        Enemytype enetype;
        public Enemy(Enemytype type) : base(Enemyinfos.enemyinfos[(int)type].nm, Enemyinfos.enemyinfos[(int)type].level, Enemyinfos.enemyinfos[(int)type].attack, Enemyinfos.enemyinfos[(int)type].defense, Enemyinfos.enemyinfos[(int)type].hpoint, Enemyinfos.enemyinfos[(int)type].gld) // 각 멤버변수 type에 맞게 초기화
        {
            maxhp = Enemyinfos.enemyinfos[(int)type].mhp;
            attackname = Enemyinfos.enemyinfos[(int)type].atkname;
            enetype = Enemyinfos.enemyinfos[(int)type].enetyp;   // 적 종류 체크
            enemypic = Enemyinfos.enemyinfos[(int)type].enepic;  // 적 모습 체크
        }
    }
    public class Enimies
    {
        private Enemy[] enimies;
        private int enemycount;

        public Enimies(int ecount) // enemycount : 적들의 수
        {
            Random rand = new Random();
            enemycount = ecount;
            enimies = new Enemy[ecount]; // 적들의 수를 받아서 enemy 배열을 생성.
            for(int i = 0; i < ecount; i++)
            {
                int t = rand.Next(0, 4);
                enimies[i] = new Enemy((Enemytype)t);
            }

        }
        public void displayenemypicture(int x, int y) // 적의 모습을 화면에 띄우는 함수
        {
            
        }

        public void displayenemyinfo() // 적의 정보를 화면에 띄우는 함수
        {

        }
        public void GetDamage(int a) // 몹이 데미지를 받았을때 함수
        {

        }
    }
}
