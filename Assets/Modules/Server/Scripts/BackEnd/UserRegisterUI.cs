using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;
using Lingua;
using TMPro;
using System.Text.RegularExpressions;
using UnityEditor;

public class UserRegisterUI : MonoBehaviour
{
    enum RegisterField
    {
        ID,
        Nickname,
        PW
    }

    public List<InputBlock> inputBlocks = new List<InputBlock>(3);

    public string KEY_NULL;
    public string KEY_SPECIAL_CHARACTER;
    public string KEY_LEAST_CHARACTER;

    const int MIN_PASSWORD = 6; 

    private void Awake()
    {
        Initialize();
        SetCallback();
    }

    void SetCallback()
    {
        inputBlocks[(int)RegisterField.ID].field.onEndEdit.AddListener(delegate { OnFieldEmpty((int)RegisterField.ID); });
        inputBlocks[(int)RegisterField.Nickname].field.onEndEdit.AddListener(delegate { OnFieldEmpty((int)RegisterField.Nickname); });
        inputBlocks[(int)RegisterField.PW].field.onEndEdit.AddListener(delegate { OnFieldEmpty((int)RegisterField.PW); });

        inputBlocks[(int)RegisterField.Nickname].field.onEndEdit.AddListener(delegate { NicknameFieldCallBack(); });
        inputBlocks[(int)RegisterField.PW].field.onEndEdit.AddListener(delegate { PWFieldCallBack(); });
    }

    void Initialize()
    {
        for (int i = 0; i < inputBlocks.Count; i++)
        {
            inputBlocks[i].field.text = "";
            inputBlocks[i].stateText.text = "";
        }
    }

    public void OnClickSubmit()
    {
        //if (IDField.)
        //BackEndServerManager.GetInstance().CustomSignIn(IDField.text, PWField.text, CustomSignInCallBack);
    }

    void CustomSignInCallBack(bool isSuccess, string msg)
    {
        //stateText.text = $"{isSuccess} msg";
    }

    public void OnFieldEmpty(int index)
    {
        if (inputBlocks[index].field.text.Equals(""))
        {
            inputBlocks[index].stateText.text = Lingua.Lingua.GetString(KEY_NULL);
            inputBlocks[index].isValid = false;
        }
        else
        {
            inputBlocks[index].stateText.text = "";
        }
    }


    void NicknameFieldCallBack()
    {
        if (Regex.IsMatch(inputBlocks[(int)RegisterField.Nickname].field.text, @"[^a-zA-Z0-9가-힣]") || string.IsNullOrWhiteSpace(inputBlocks[(int)RegisterField.Nickname].field.text))
        {
            inputBlocks[(int)RegisterField.Nickname].stateText.text = Lingua.Lingua.GetString(KEY_SPECIAL_CHARACTER);
            inputBlocks[(int)RegisterField.Nickname].isValid = false;
        }
        else
        {
            inputBlocks[(int)RegisterField.Nickname].stateText.text = "";
        }
    }

    void PWFieldCallBack()
    {
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

    bool IsValidUserInformation()
    {
        bool flag = false;
        /*for (int i = 0; i < Inputs.Count; i++)
        {
            
        }*/
        // Email validation

        // Nickname validation


        return flag;
    }

    [Serializable]
    public class InputBlock
    {
        public TextMeshProUGUI stateText;
        public InputField field;
        public bool isValid;
    }
}
