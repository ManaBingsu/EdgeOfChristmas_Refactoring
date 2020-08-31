using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Server.BackEnd
{
    public class NicknameRegister : MonoBehaviour
    {
        /*
        event Action onEditNickname;


        public InputField nicknameField;
        public TextMeshProUGUI stateText;

        public string KEY_NULL;
        public string KEY_SPECIAL_CHARACTER;

        private void Awake()
        {
            Initialize();
        }
        void Initialize()
        {
            nicknameField.text = "";
            SetCallback();
        }

        void SetCallback()
        {
            onEditNickname += new Action(NicknameFieldCallBack);

            nicknameField.onEndEdit.AddListener(delegate { onEditNickname(); });
        }


        public void OnClickSubmit()
        {
            // Check validation

            // Try to register
            BackEndServerManager.GetInstance().RegistNickname(nicknameField.text, SubmitCallBack);
        }

        void SubmitCallBack(bool isSucceed, string msg)
        {

        }

        public bool IsFieldEmpty()
        {
            if (nicknameField.text.Equals(""))
            {
                stateText.text = Lingua.Lingua.GetString(KEY_NULL);
                return true;
            }
            else
            {
                stateText.text = "";
            }
            return false;
        }

        void NicknameFieldCallBack()
        {
            if (IsFieldEmpty())
            {
                return;
            }
            if (Regex.IsMatch(nicknameField.text, @"[^a-zA-Z0-9가-힣]") || string.IsNullOrWhiteSpace(nicknameField.text))
            {
                stateText.text = Lingua.Lingua.GetString(KEY_SPECIAL_CHARACTER);
            }
            else
            {
                stateText.text = "";
            }
        }
        */
    }
}

