using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    public class CharacterStat : MonoBehaviour
    {
        [SerializeField]
        private CharacterData data;
        #region Stat
        public float MoveSpeed
        {
            get;
            set;
        }
        #endregion

        void Awake()
        {
            MoveSpeed = data.moveSpeed;
        }
    }
}

