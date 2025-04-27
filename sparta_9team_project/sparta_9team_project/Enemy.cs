using System;



namespace sparta_9team_project
{ 

    public enum Enemytype // 적 타입 enum형태로 표현
    {
        catling,      //새끼고양이
        chihuahua,   //치와와
        cat,         //고양이
        husky,       //허스키
        mortorcycle,  //오토바이
        bear, // 곰
        boar, // 멧돼지
        eagle, // 독수리
        finalboss
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
        
        public Enemyinfo(string _nm, int _level, int _attack, int _defense, int _hpoint, int _gld, string _atkname, string _enepic, int _mhp, Enemytype _enetyp)
        {
            nm = _nm;
            level = _level;
            attack = _attack;
            defense = _defense;
            hpoint = _hpoint;
            gld = _gld;
            atkname = _atkname;
            enepic = _enepic;
            mhp = _mhp;
            enetyp = _enetyp;
        }
    }


    public class Enemyinfos
    {
        public static Enemyinfo[] enemyinfos =  // 각 enmyinfo를 미리 배열에 저장
        {
            new Enemyinfo("새끼고양이", 1, 6, 3, 30, 4, "냥냥 펀치", Print.dogImage[10], 30, Enemytype.catling),
            new Enemyinfo("치와와", 2, 10, 5, 40, 6, "물어 뜯기", Print.dogImage[6], 40, Enemytype.chihuahua),
            new Enemyinfo("고양이", 3, 15, 7, 60, 8, "할퀴기", Print.dogImage[9], 60, Enemytype.cat),
            new Enemyinfo("허스키",   4, 20, 15,  90, 18, "얼음 으르렁", Print.dogImage[7],  90, Enemytype.husky),
            new Enemyinfo("오토바이", 5, 24, 10, 120, 22, "부릉부릉",    Print.dogImage[8], 120, Enemytype.mortorcycle),
            new Enemyinfo("곰",6,22,14,110,22,"으르렁",     Print.dogImage[13],110,Enemytype.bear),
            new Enemyinfo("멧돼지",7,24,12, 95,20,"돌진",       Print.dogImage[15], 95,Enemytype.boar),
            new Enemyinfo("독수리",8,26, 8, 80,25,"하늘 강타", Print.dogImage[14], 80,Enemytype.eagle),
            new Enemyinfo("동물병원의사",25,60,40,600,300,"사나운 주사기",Print.dogImage[16],600,Enemytype.finalboss)

        };
    }
    // 사용 방법 :  Enemyinfos.emeyinfos[i]로 접근하면 Enemy정보에 접근 가능(static이기 때문)

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
        public void GetDamage(int damage) // 몹이 데미지를 받았을때 함수
        {
            Hp -= damage;
            if (Hp < 0) { Hp = 0; }
        }
    }
    public class Enimies
    {
        private Enemy[] enemies;
        private int enemycount;

        public Enimies(int ecount, int rangestart, int rangeend) // enemycount : 적들의 수, rangestar : 랜덤 시작 , rangeend : rangeend
        {
            Random rand = new Random();
            enemycount = ecount;
            enemies = new Enemy[ecount]; // 적들의 수를 받아서 enemy 배열을 생성.
            for (int i = 0; i < ecount; i++)
            {
                int t = rand.Next(rangestart, rangeend);
                enemies[i] = new Enemy((Enemytype)t);
            }

        }

        public bool EnemygetDamage(int enemynum, int damage) // enemynum번호의 몹이 데미지를 받는 동작을 하는 함수
        {
            if (enemynum <= 0 || enemynum >= enemycount) return false;
            if (enemies[enemynum].Hp == 0) return false;
            enemies[enemynum-1].GetDamage(damage);
            return true;
            // 만약 잘못된 enemynum이나 이미 죽은 몹이면 false를 반환하여 잘못된 입력임을 알려줌
            // 입력이 제대로 되었다면 체력 깎는 GetDamage함수를 실행시키고 true를 반환하여 성공을 알려줌.
        }
        
        public bool GetEnemyInfo(int input, ref Enemy Ei)
        {
            if(input <= 0 || input >= enemycount) {  return false; }
            Ei = enemies[input-1];
            return true;
        }
        // input이 잘못된 입력이면 false를 반환하고 Ei에 enemy 정보를 입력하지 않음
        // 맞는 입력이라면 true를 반환하고 Ei에 enemy 정보를 입력


    }

}

// 사용 예시 :
// 던전에서 적들 3명을 생성하려면 : Enimies dungeonmob = new Enimies(3);
//                                -> 3명의 몹이 랜덤으로 정해지면서 Enimies에 저장됨
// 던전에서 적들 중 1번 몹에게 10의 데미지를 주고 싶다면 :
//                               dungeonmob.EnemygetDamage(1, 10);
//                                -> 1번 몹이 10 데미지를 받는데, 만약 1번몹이 이미 죽었다면 false를 반환
//                                   맞는 몹 선택이라면 데미지를 받게하고 true를 반환
//                                   이 true/false 반환으로 맞는 선택을했는지 알 수 있음 -> 활용 가능
// 던전에서 1번 적의 현재 정보를 확인하고 싶다면 :
//                               Enemy enemy1;
//                               dungeonmob.GetEnemyInfo(1, enemy1);
//                               1번 몹의 정보가 enemy1에 들어가게 됨 그리고 true 반환
//                               잘못된 번호의 입력이라면 false 반환
// 추가 예정...
