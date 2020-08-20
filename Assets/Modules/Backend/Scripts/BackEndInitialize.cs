using BackEnd;
using UnityEngine;
using Debug = Helper.Debug;

namespace Server.BackEnd
{
    public class BackEndInitialize : MonoBehaviour
    {
        private void Awake()
        {
            Backend.Initialize(HandleBackendCallBack);
        }

        void HandleBackendCallBack()
        {
            if (Backend.IsInitialized)
            {
                Debug.Log("뒤끝 SDK 초기화 완료");
                // 구글 해시키 획득
                if (!Backend.Utils.GetGoogleHash().Equals(""))
                {
                    Debug.Log($"구글 해시 {Backend.Utils.GetGoogleHash()}");
                }

                // 서버 시간 획득
                Debug.Log(Backend.Utils.GetServerTime());
            }
            // 실패 시
            else
            {
                Debug.LogError("뒤끝 SDK 초기화 실패");
            }
        }
    }
}

