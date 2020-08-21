using Battle.Character.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class CharacterAction : MonoBehaviour
    {
        public enum EMoveState
        {
            Left = -1,
            Zero = 0,
            Right = 1
        }
        private EMoveState moveState;
        public EMoveState MoveState
        {
            get => moveState;
            set
            {
                if (value == moveState)
                    return;
                moveState = value;
            }
        }

        ICharacterMove movement;
        public Rigidbody2D Body { get; private set; }
        public CharacterStat Stat { get; private set; }

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Stat = GetComponent<CharacterStat>();
            movement = GetComponent<DefaultMovement>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            movement.Move(this);
        }

        public void SetMoveState(EMoveState moveState)
        {
            MoveState = moveState;
        }
    }
}

