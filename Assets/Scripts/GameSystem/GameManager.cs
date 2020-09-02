using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }

        private void Initialize()
        {
            // 60프레임 고정
            Application.targetFrameRate = 60;
            // 게임중 슬립모드 해제
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}

