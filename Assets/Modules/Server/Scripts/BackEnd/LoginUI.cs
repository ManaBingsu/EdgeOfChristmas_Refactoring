using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public InputField IDField;
    public InputField PWField;

    public Text stateText;

    public void OnClickSubmit()
    {
        BackEndServerManager.GetInstance().CustomSignIn(IDField.text, PWField.text, CustomSignInCallBack);
    }

    void CustomSignInCallBack(bool isSuccess, string msg)
    {
        stateText.text = $"{isSuccess} msg";
    }
}
