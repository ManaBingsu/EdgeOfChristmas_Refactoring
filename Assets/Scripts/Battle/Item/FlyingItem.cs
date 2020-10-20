using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class FlyingItem : MonoBehaviour
    {
        protected ItemData itemData;

        public SessionId OwnerId { get; set; }

        public bool IsFlying { get; set; }

        protected int xDir;

        protected float speed;

        protected bool isRotate;

        public void Initialize(FlyingItemInfo flyingItemInfo)
        {
            IsFlying = true;

            OwnerId = flyingItemInfo.SessionId;
            this.itemData = flyingItemInfo.ItemData;
            transform.position = flyingItemInfo.Pos;

            xDir = BattleManager.Instance.players[OwnerId].LookDiretion;
            speed = itemData.flyingSpeed;
            isRotate = itemData.isRotate;
        }

        protected abstract void Collided(Collider2D col);

        protected abstract void Die();

        protected virtual void Move()
        {
            transform.position += new Vector3(xDir * speed * Time.deltaTime, 0);
        }

        protected IEnumerator Disappear()
        {
            yield return new WaitForSeconds(3f);
            ReturnToPool();
        }

        protected abstract void ReturnToPool();

        public void CollidedWithPlayer()
        {
            ItemManager.Instance.FallingItemPool.ReturnObject(this.gameObject);
        }
    }
}

