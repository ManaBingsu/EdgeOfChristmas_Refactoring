using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class UserInfoUI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI nickNameText;

        private void Start()
        {
            nickNameText.text = BackEndServerManager.GetInstance().myNickName;
        }
    }
}

