using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Management
{
    public class InputManager : MonoBehaviour
    {
        bool isMove = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                InputMove(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                InputMove(-1);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                InputStop();
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                InputStop();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                InputJump();
            }
        }
        private void InputMove(int xDir)
        {
            int keyCode = 0;
            keyCode |= KeyEventCode.MOVE;
            Vector3 moveVector = new Vector3(xDir, 0, 0);

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

        private void InputJump()
        {
            int keyCode = 0;
            keyCode |= KeyEventCode.JUMP;
            Vector3 moveVector = new Vector3(0, 0, 0);

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

