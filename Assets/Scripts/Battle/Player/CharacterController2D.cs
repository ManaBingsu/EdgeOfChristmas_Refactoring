using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField]
        protected CharacterControllerData data;

        [SerializeField]
        private Transform groundCheck;

        [SerializeField]
        private float groundCheckRadius;

        [SerializeField]
        public LayerMask groundMask;

        protected Rigidbody2D body;

        protected bool isGrounded;

        protected bool isKnockBack;

        protected bool isMove;

        protected float currentMoveSpeed;

        public float CurrentMoveSpeed
        {
            get => currentMoveSpeed;
            set
            {
                currentMoveSpeed = value;
                if (currentMoveSpeed < 0)
                {
                    currentMoveSpeed = 0;
                }
                else if (currentMoveSpeed > data.maxMoveSpeed)
                {
                    currentMoveSpeed = data.maxMoveSpeed;
                }
            }
        }

        private int goalDirection;
        public int GoalDirection
        {
            get => goalDirection;
            set
            {
                goalDirection = value;
                if (goalDirection != 0)
                {
                    LookDiretion = goalDirection;
                }
            }
        }

        public int LookDiretion { get; set; }

        protected float moveSpeed;
        public float MoveSpeed
        {
            get => moveSpeed;
            set
            {
                moveSpeed = value;
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {

        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {

        }

        protected virtual void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            GoalDirection = 0;
            LookDiretion = 1;
        }

        protected virtual void Update()
        {
            if (isMove)
            {
                Move();
            }
            else
            {
                NotMove();
            }
            Movement();
        }

        protected virtual void FixedUpdate()
        {
            CheckGround();
        }

        private void CheckGround()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        }

        public void Move()
        {
            CurrentMoveSpeed = data.maxMoveSpeed;
        }

        public void Jump()
        {
            if (!isGrounded)
                return;

            body.AddForce(new Vector3(0, 10, 0), ForceMode2D.Impulse);
        }

        public void Movement()
        {
            transform.position += Vector3.right * CurrentMoveSpeed * GoalDirection * Time.deltaTime;
        }

        public void NotMove()
        {
            GoalDirection = 0;
        }

        protected virtual IEnumerator KnockBack(int xDir, float ccPower)
        {
            isKnockBack = true;

            body.velocity = new Vector2(0f, body.velocity.y);
            body.AddForce((Vector2.right * xDir + Vector2.up).normalized * ccPower, ForceMode2D.Impulse);
            float time = 0f;
            float ccTime = ccPower / 20;
            while (time < ccTime)
            {
                time += Time.deltaTime;
                yield return null;
            }
            isKnockBack = false;
            yield return null;
        }
    }
}

