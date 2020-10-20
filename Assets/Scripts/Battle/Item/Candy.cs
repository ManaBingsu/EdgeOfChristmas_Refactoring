using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Candy : FlyingItem
    {

        protected override void Collided(Collider2D col)
        {
            /*
            if (col.CompareTag("Player"))
            {
                if (col.GetComponent<Player>().sessionId != OwnerId)
                {
                    Die();
                }
            }

            if (col.CompareTag("FlyingItem"))
            {
                if (col.GetComponent<FlyingItem>().OwnerId != OwnerId)
                {
                    Die();
                }
            }
            */
        }

        private void Update()
        {
            Move();
        }

        void OnEnable()
        {
            if (ItemManager.Instance != null && ItemManager.Instance.FlyingItemInfo != null)
            {
                Initialize(ItemManager.Instance.FlyingItemInfo);
                StartCoroutine(Disappear());
            }
        }

        protected override void Die()
        {
            throw new System.NotImplementedException();
        }

        protected override void Move()
        {
            base.Move();
        }

        protected override void ReturnToPool()
        {
            ItemManager.Instance.CandyPool.ReturnObject(this.gameObject);
        }
    }
}

