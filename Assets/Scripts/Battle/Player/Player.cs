using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Player : MonoBehaviour
    {
        private bool isMove = false;

        public Vector3 moveVector { get; private set; }

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

        public void SetPosition(float x, float y, float z)
        {
            Vector3 pos = new Vector3(x, y, z);
            SetPosition(pos);
        }

        public void SetPosition(Vector3 pos)
        {
            gameObject.transform.position = pos;
        }

        public void SetMoveVector(Vector3 vector)
        {
            moveVector = vector;

            if (vector == Vector3.zero)
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
            transform.position += moveVector * Time.deltaTime;
        }
    }
}

