using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FallingItem : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        public ItemData itemInfo;

        private float speed;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            Move();
        }

        public void Initialize(ItemData itemInfo, float xPos, float speed, float rotate)
        {
            this.itemInfo = itemInfo;

            spriteRenderer.sprite = itemInfo.sprite;
            transform.position = new Vector3(xPos, ItemManager.Instance.yPos, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotate));
            this.speed = speed;
        }

        public void Move()
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        void OnEnable()
        {
            StartCoroutine(test());
        }

        IEnumerator test()
        {
            yield return new WaitForSeconds(3f);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            ItemManager.Instance.pool.ReturnObject(this);
        }
    }
}
