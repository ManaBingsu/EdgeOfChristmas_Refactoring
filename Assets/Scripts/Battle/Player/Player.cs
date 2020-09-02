using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        #region Reference
        private Rigidbody2D body;

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
        public bool IsWinner { get; set; }
        public int WinCount
        {
            get;
            set;
        }
        #endregion

        public int XDir { get; set; }

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
            body.AddForce(new Vector3(0, 10, 0), ForceMode2D.Impulse);
        }
    }
}