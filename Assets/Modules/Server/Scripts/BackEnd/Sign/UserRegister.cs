using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lingua;
using TMPro;
using GameSystem;

namespace Server.BackEnd
{
    public class UserRegister : MonoBehaviour
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
        public List<InputBlock> inputBlocks = new List<InputBlock>(2);

        [Header("Lingua Key")]
        public string KEY_NULL;
        public string KEY_LEAST_CHARACTER;
        public string KEY_TRY_TO_REGIST;
        public string KEY_INVALID_INFORMATION;
        public string KEY_REGISTER_FAILED;

        event Action onEditID;
        event Action onEditPW;

        const int MIN_PASSWORD = 6;

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
                switch(loginState)
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
            // Check validation
            for (int i = 0; i < (int)RegisterField.COUNT; i++)
            {
                if (!inputBlocks[i].isValid)
                {
                    AlertMessage.Instance.SetActivePanel(true);
                    AlertMessage.Instance.SetMessage(Lingua.Lingua.GetString(KEY_INVALID_INFORMATION));
                    return;
                }
            }

            // Try to register
            BackEndServerManager.GetInstance().CustomSignIn(inputBlocks[(int)RegisterField.ID].field.text, inputBlocks[(int)RegisterField.PW].field.text, SubmitCallBack);
            // On loading panel
            LoadingMessage.Instance.SetActivePanel(true);
            LoadingMessage.Instance.SetMessage(Lingua.Lingua.GetString(KEY_TRY_TO_REGIST));
        }

        void Initialize()
        {
            for (int i = 0; i < inputBlocks.Count; i++)
            {
                inputBlocks[i].field.text = "";
                inputBlocks[i].stateText.text = "";
            }
            SetCallback();
        }

        void SetCallback()
        {
            onEditID += new Action(IDFieldCallback);
            onEditPW += new Action(PWFieldCallBack);

            // Event callback
            inputBlocks[(int)RegisterField.ID].field.onEndEdit.AddListener(delegate { onEditID(); });
            inputBlocks[(int)RegisterField.PW].field.onEndEdit.AddListener(delegate { onEditPW(); });
        }

        bool IsFieldEmpty(int index)
        {
            if (inputBlocks[index].field.text.Equals(""))
            {
                inputBlocks[index].stateText.text = Lingua.Lingua.GetString(KEY_NULL);
                inputBlocks[index].isValid = false;
                return true;
            }
            else
            {
                inputBlocks[index].stateText.text = "";
            }
            return false;
        }

        void IDFieldCallback()
        {
            if (IsFieldEmpty((int)RegisterField.ID))
            {
                inputBlocks[(int)RegisterField.ID].isValid = false;
                return;
            }
            else
            {
                inputBlocks[(int)RegisterField.ID].isValid = true;
            }
        }

        void PWFieldCallBack()
        {
            if (IsFieldEmpty((int)RegisterField.PW))
            {
                inputBlocks[(int)RegisterField.PW].isValid = false;
                return;
            }
            if (inputBlocks[(int)RegisterField.PW].field.text.Length < MIN_PASSWORD)
            {
                inputBlocks[(int)RegisterField.PW].stateText.text = Lingua.Lingua.GetString(KEY_LEAST_CHARACTER);
                inputBlocks[(int)RegisterField.PW].isValid = false;
            }
            else
            {
                inputBlocks[(int)RegisterField.PW].stateText.text = "";
                inputBlocks[(int)RegisterField.PW].isValid = true;
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
                loginMessage = Lingua.Lingua.GetString(KEY_REGISTER_FAILED);
            }
        }

        [Serializable]
        public class InputBlock
        {
            public TextMeshProUGUI stateText;
            public InputField field;
            public bool isValid;
        }
    }
}
