using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    [RequireComponent(typeof(CharacterAction))]
    [RequireComponent(typeof(CharacterStat))]
    public class CharacterController : MonoBehaviour
    {
        private CharacterAction characterAction;

        private void Awake()
        {
            characterAction = GetComponent<CharacterAction>();
        }

        public void Move(CharacterAction.EMoveState moveState)
        {
            characterAction.SetMoveState(moveState);
        }
    }
}

