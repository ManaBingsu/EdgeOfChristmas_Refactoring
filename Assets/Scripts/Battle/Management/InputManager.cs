﻿using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public Player myPlayer;

        bool isMove = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this.gameObject);
        }

        private void Start()
        {
            VirtualStick.Instance.xDirChangeEvent += InputMove;
            myPlayer = BattleManager.Instance.players[BattleManager.Instance.myPlayerIndex];
        }
        private void Update()
        {
            InputControl();
        }

        private void InputControl()
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
            if (BattleManager.Instance.FlowState != BattleManager.EFlowState.Progress)
                return;

            int keyCode = 0;
            keyCode |= KeyEventCode.MOVE;
            Vector3 moveVector = new Vector3(xDir, myPlayer.CurrentMoveSpeed, 0);

            KeyMessage msg = new KeyMessage(keyCode, moveVector);

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
            if (BattleManager.Instance.FlowState != BattleManager.EFlowState.Progress)
                return;

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

        private void InputUseItem()
        {
            int keyCode = 0;
            keyCode |= KeyEventCode.USEITEM;

            KeyMessage msg;
            msg = new KeyMessage(keyCode, Vector3.zero);
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

