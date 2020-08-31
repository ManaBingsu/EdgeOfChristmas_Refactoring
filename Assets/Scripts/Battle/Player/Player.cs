using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Player : MonoBehaviour
    {
        private bool isMove = false;

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

        public Vector3 moveVector;

        public void SetMoveVector(Vector3 vector)
        {
            //moveVector = vector;

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
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        }
    }
}

