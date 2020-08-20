using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    [CreateAssetMenu(fileName = "CharcterStatData", menuName = "Battle/Character/StatData")]
    public class CharacterData : ScriptableObject
    {
        #region Stat
        public float moveSpeed;
        #endregion
    }
}

