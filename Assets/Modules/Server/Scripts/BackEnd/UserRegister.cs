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
            PW
        }

        public List<InputBlock> inputBlocks = new List<InputBlock>(2);

        public string KEY_NULL;
        public string KEY_LEAST_CHARACTER;

        event Action onEditID;
        event Action onEditPW;

        const int MIN_PASSWORD = 6;

        private void Awake()
        {
            Initialize();
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

        public void OnClickSubmit()
        {
            // Check validation

            // Try to register
            BackEndServerManager.GetInstance().CustomSignIn(inputBlocks[(int)RegisterField.ID].field.text, inputBlocks[(int)RegisterField.PW].field.text, SubmitCallBack);
            // On loading panel
            LoadingMessage.Instance.SetActivePanel(true);
        }

        void IDFieldCallback()
        {
            if (IsFieldEmpty((int)RegisterField.ID))
            {
                return;
            }
        }

        void PWFieldCallBack()
        {
            if (IsFieldEmpty((int)RegisterField.PW))
            {
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
            }
        }

        void SubmitCallBack(bool isSucceed, string msg)
        {
            if (isSucceed)
            {
                LoadingMessage.Instance.SetActivePanel(false);
            }
            else
            {
                AlertMessage.Instance.SetActivePanel(true);
                AlertMessage.Instance.SetMessage(msg);
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
