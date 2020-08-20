using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private CharacterData characterData;

        CharacterAction characterAction;
        CharacterStat characterStat;
        private void Awake()
        {
            characterStat = new CharacterStat(characterData);
            characterAction = new CharacterAction(this);
        }
    }
}

