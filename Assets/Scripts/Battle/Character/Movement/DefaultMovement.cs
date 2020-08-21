using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character.Movement
{
    public class DefaultMovement : MonoBehaviour, ICharacterMove
    {
        public void Move(CharacterAction characterAction)
        {
            Vector2 velocity = transform.position + transform.right * (int)(characterAction.MoveState) * characterAction.Stat.MoveSpeed;
            characterAction.Body.MovePosition(velocity);
        }
    }
}

