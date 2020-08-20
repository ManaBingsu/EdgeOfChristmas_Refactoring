using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character.Movement
{
    public class DefaultMovement : MonoBehaviour, ICharacterMove
    {
        public void Move(CharacterAction characterAction)
        {
            Vector2 velocity = Vector2.right * (int)(characterAction.Direction) * characterAction.Stat.MoveSpeed * Time.deltaTime; 
            characterAction.Body.MovePosition(velocity);
        }
    }
}

