﻿using BackEnd.Tcp;
using JetBrains.Annotations;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

namespace Battle
{
    public class Player : CharacterController2D
    {
        #region Reference
        [SerializeField]
        GameObject marker;
        #endregion

        #region Game system
        private bool isMyPlayer;
        public bool IsMyPlayer
        {
            get => isMyPlayer;
            set
            {
                isMyPlayer = value;
                marker.SetActive(isMyPlayer);
            }
        }

        public SessionId sessionId;

        public bool IsWinner { get; set; }
        public int WinCount
        {
            get;
            set;
        }
        #endregion

        #region Public info
        public int XDir { get; set; }

        private int itemIndex;
        public int ItemIndex
        {
            get => itemIndex;
            set
            {
                if (value == itemIndex)
                {
                    ItemNum++;
                }
                else
                {
                    itemIndex = value;
                    ItemNum = 1;
                }

            }
        }
        private int itemNum;
        public int ItemNum
        {
            get => itemNum;
            set
            {
                if (value > ItemManager.Instance.maxItemNum)
                    return;
                itemNum = value;
            }
        }
        #endregion

        private bool isMove = false;

        public bool ableToMove = false;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (isMove)
            {
                Move();
            }
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);

            if (col.gameObject.CompareTag("FlyingItem"))
            {
                FlyingItem item = col.gameObject.GetComponent<FlyingItem>();
                if (item.OwnerId != sessionId)
                {
                    GetDamage(itemIndex);
                }

            }

            if (col.gameObject.CompareTag("FallingItem"))
            {
                FallingItem item = col.gameObject.GetComponent<FallingItem>();
                CollideWithFallingItem(item.itemInfo.index);
            }

        }


        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public int GetItemIndex()
        {
            return ItemIndex;
        }

        public void SetPosition(float x, float y, float z)
        {
            Vector3 pos = new Vector3(x, y, z);
            SetPosition(pos);
        }

        public void SetPosition(Vector3 pos)
        {
            gameObject.transform.position = pos;
        }

        public void SetMoveVector(int xDir)
        {
            this.XDir = xDir;

            if (xDir == 0)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
        }

        public void Move()
        {
            transform.position += Vector3.right * 2 * XDir * Time.deltaTime;
        }

        public void Jump()
        {
            if (!isGrounded)
                return;

            body.AddForce(new Vector3(0, 10, 0), ForceMode2D.Impulse);
        }

        public void UseItem()
        {
            ItemManager.Instance.ThrowItem(sessionId, ItemIndex, transform.position);
        }

        public void CollideWithFallingItem(int itemIndex)
        {
            if (BattleManager.Instance.FlowState != BattleManager.EFlowState.Progress)
                return;

            PlayerGetItemMessage msg;
            msg = new PlayerGetItemMessage(sessionId, itemIndex);
            if (BackEndMatchManager.GetInstance().IsHost())
            {
                BattleManager.Instance.ProcessGetItemEvent(msg);
            }
            else
            {
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerGetItemMessage>(msg);
            }
        }

        public void GetItem(int itemIndex)
        {
            if (ItemManager.Instance.itemInfos[itemIndex].itemType == ItemData.ItemType.Consume)
            {
                GetConsumeItem(itemIndex);
            }
            else if (ItemManager.Instance.itemInfos[itemIndex].itemType == ItemData.ItemType.Active)
            {
                ItemIndex = itemIndex;
            }
        }

        private void GetConsumeItem(int itemIndex)
        {
            switch(itemIndex)
            {
                case (int)ItemManager.Item.Gift:
                    // 점수 얻기
                    break;

                case (int)ItemManager.Item.GoldenGift:
                    break;
            }
        }

        public void GetDamage(int itemIndex)
        {
            switch (itemIndex)
            {
                case (int)ItemManager.Item.Snowball:
                    // 점수 얻기
                    break;

                case (int)ItemManager.Item.Candy:
                    break;
                default:
                    break;
            }
        }
    }
}