using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Battle/Item/ItemInfo")]
    public class ItemData : ScriptableObject
    {
        public enum ItemType
        {
            Consume,
            Active
        }
        [Header("Info")]
        public int index;
        public Sprite sprite;
        public ItemType itemType;

        [Header("Falling Setting")]
        public float appearPercentage;

        [Header("Flying Setting")]
        public float flyingSpeed;
        public float ccTime;
        public float ccPower;
        public bool isRotate;
    }
}

