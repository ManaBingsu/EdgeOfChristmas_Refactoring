using UnityEngine;

namespace Battle
{
    [CreateAssetMenu(fileName = "CharacterControllerData", menuName = "Battle/Character/CharacterControllerData")]
    public class CharacterControllerData : ScriptableObject
    {
        public float maxMoveSpeed;
        public float acceleration;
    }
}

