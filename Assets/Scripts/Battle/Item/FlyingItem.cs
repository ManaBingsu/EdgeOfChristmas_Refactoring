using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public abstract class FlyingItem : MonoBehaviour
    {
        protected ItemData itemInfo;

        public SessionId OwnerId { get; set; }

        public bool IsFlying { get; set; }

        protected int xDir;

        protected float speed;

        protected bool isRotate;

        public void Initialize(SessionId sessionId, ItemData itemInfo, Vector3 pos)
        {
            IsFlying = true;

            OwnerId = sessionId;
            this.itemInfo = itemInfo;
            transform.position = pos;

            xDir = BattleManager.Instance.players[sessionId].XDir;
            speed = itemInfo.flyingSpeed;
            isRotate = itemInfo.isRotate;
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            IsFlying = false;
            Collided(col);
        }

        protected abstract void Collided(Collider2D col);

        protected abstract void Die();

        protected virtual void Move()
        {
            
        }
    }
}

