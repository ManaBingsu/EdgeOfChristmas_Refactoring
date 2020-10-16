using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Snowball : FlyingItem
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Collided(col);
        }

        protected override void Collided(Collider2D col)
        {
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
        }

        protected override void Die()
        {
            throw new System.NotImplementedException();
        }

        protected override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}

