using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    public class CharacterAction
    {
        public enum EDirection
        {
            Left = -1,
            Zero = 0,
            Right = 1
        }
        private EDirection direction;
        public EDirection Direction
        {
            get => direction;
            set
            {
                if (value == direction)
                    return;
                direction = value;
            }
        }

        CharacterController controller;

        ICharacterMove movement;
        public CharacterStat Stat { get; private set; }
        public Rigidbody2D Body { get; private set; }

        public CharacterAction(CharacterController controller)
        {
            this.controller = controller;
        }

        public void Move(EDirection direction)
        {
            Direction = direction;
            movement.Move(this);
        }

        public void StopMove()
        {
            Direction = EDirection.Zero;
        }
    }
}

