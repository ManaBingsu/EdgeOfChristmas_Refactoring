using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;

namespace Server.BackEnd
{
    public class UserLogin : MonoBehaviour
    {
        enum RegisterField
        {
            ID,
            PW,
            COUNT
        }

        enum LoginState
        {
            Idle,
            Success,
            Fail,
            COUNT
        }
        [Header("Reference")]
        public List<InputField> inputFields = new List<InputField>(2);

        [Header("Lingua Key")]
        public string KEY_TRY_TO_LOGIN;
        public string KEY_BAD_ID;
        public string KEY_BAD_PASSWORD;
        public string KEY_BAD_LOGIN;

        LoginState loginState;
        string loginMessage;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (loginState > 0)
            {
                switch (loginState)
                {
                    case (LoginState.Success):
                        LoadingMessage.Instance.SetActivePanel(false);
                        SceneManager.Instance.LoadScene(SceneManager.Instance.lobbySceneName);
                        loginState = LoginState.Idle;
                        break;
                    case (LoginState.Fail):
                        LoadingMessage.Instance.SetActivePanel(false);
                        AlertMessage.Instance.SetActivePanel(true);
                        AlertMessage.Instance.SetMessage(loginMessage);
                        loginState = LoginState.Idle;
                        break;
                }
            }
        }
        public void OnClickSubmit()
        {
            // Try to register
            BackEndServerManager.GetInstance().CustomLogin(inputFields[(int)RegisterField.ID].text, inputFields[(int)RegisterField.PW].text, SubmitCallBack);
            // On loading panel
            LoadingMessage.Instance.SetActivePanel(true);
            LoadingMessage.Instance.SetMessage(Lingua.Lingua.GetString(KEY_TRY_TO_LOGIN));
        }

        void Initialize()
        {
            for (int i = 0; i < inputFields.Count; i++)
            {
                inputFields[i].text = "";
            }
        }

        void SubmitCallBack(bool isSucceed, string msg)
        {
            if (isSucceed)
            {
                loginState = LoginState.Success;
            }
            else
            {
                loginState = LoginState.Fail;
                if (msg.Contains("customId"))
                {
                    loginMessage = Lingua.Lingua.GetString(KEY_BAD_ID);
                }
                else if (msg.Contains("customPassword"))
                {
                    loginMessage = Lingua.Lingua.GetString(KEY_BAD_PASSWORD);
                }
                else
                {
                    loginMessage = Lingua.Lingua.GetString(KEY_BAD_LOGIN);
                }
            }
        }
    }
}

