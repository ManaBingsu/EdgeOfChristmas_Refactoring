using BackEnd;
using UnityEngine;

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
                // 구글 해시키 획득
                if (!Backend.Utils.GetGoogleHash().Equals(""))
                {

                }
            }
            // 실패 시
            else
            {

            }
        }
    }
}

