using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace sparta_9team_project
{

    public static class SoundManager
    {
        // 사용 예시
        //SoundManager.StopBGM();      // 기존 BGM 중지
        //SoundManager.PlayBattleBGM(); // 전투 BGM 시작
        //SoundManager.PlayBasicBGM(); // 기본 BGM 시작
        
        private static SoundPlayer player;

        public static void PlayBasicBGM()
        {
            player = new SoundPlayer("Resources/basic bgm.wav");
            player.PlayLooping();
        }

        public static void PlayBattleBGM()
        {
            player = new SoundPlayer("Resources/battle bgm.wav");
            player.PlayLooping();
        }

        public static void StopBGM()
        {
            if (player != null)
            {
                player.Stop();
            }
        }
    }

}
