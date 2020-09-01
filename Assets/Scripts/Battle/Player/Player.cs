using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public bool IsWinner { get; set; }

        public int winCount;

        private Rigidbody2D body;
        public int xDir { get; set; }

        private bool isMove = false;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (isMove)
            {
                Move();
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public int GetItemIndex()
        {
            return 0;
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
            this.xDir = xDir;

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
            transform.position += Vector3.right * 2 * xDir * Time.deltaTime;
        }

        public void Jump()
        {
            body.AddForce(new Vector3(0, 5, 0), ForceMode2D.Impulse);
        }
    }
}

