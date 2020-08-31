using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Management
{
    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                InputMove();
            }

            if (Input.GetKey(KeyCode.Z))
            {
                InputStop();
            }
        }
        private void InputMove()
        {
            int keyCode = 0;
            keyCode |= KeyEventCode.MOVE;
            Vector3 moveVector = new Vector3(1, 0, 0);

            KeyMessage msg;
            msg = new KeyMessage(keyCode, moveVector);
            if (BackEndMatchManager.GetInstance().IsHost())
            {
                BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
            }
            else
            {
                BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
            }
        }

        private void InputStop()
        {
            int keyCode = 0;
            keyCode |= KeyEventCode.MOVE;
            Vector3 moveVector = new Vector3(0, 0, 0);

            Debug.Log("haha");

            KeyMessage msg;
            msg = new KeyMessage(keyCode, moveVector);
            if (BackEndMatchManager.GetInstance().IsHost())
            {
                BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
            }
            else
            {
                BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
            }
        }
    }
}

