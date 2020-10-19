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

        public ItemData ItemData { get; set; }

        private float speed;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            Move();
        }

        public void Initialize(FallingItemInfo fallingItemInfo)
        {
            ItemData = ItemManager.Instance.itemDatas[fallingItemInfo.ItemIndex];

            spriteRenderer.sprite = ItemData.sprite;
            transform.position = new Vector3(fallingItemInfo.ItemXPos, ItemManager.Instance.yPos, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, fallingItemInfo.ItemRotate));
            speed = fallingItemInfo.ItemSpeed;
        }

        public void Move()
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        void OnEnable()
        {
            if (ItemManager.Instance != null && ItemManager.Instance.FallingItemInfo != null)
            {
                Initialize(ItemManager.Instance.FallingItemInfo);
                StartCoroutine(test());
            }

        }

        IEnumerator test()
        {
            yield return new WaitForSeconds(3f);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            ItemManager.Instance.Pool.ReturnObject(this.gameObject);
        }
    }
}
