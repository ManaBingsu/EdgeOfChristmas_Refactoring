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

            }
        }

        private void Move()
        {
            PlayerMoveMessage msg;

            msg = new KeyMessage(keyCode, aimPos);

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

