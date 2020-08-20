using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    public class CharacterStat
    {
        #region Stat
        public float MoveSpeed
        {
            get;
            set;
        }
        #endregion

        CharacterController controller;

        public CharacterStat(CharacterData characterStat)
        {
            MoveSpeed = characterStat.moveSpeed;
        }
    }
}

