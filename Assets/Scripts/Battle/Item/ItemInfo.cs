using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Battle/Item/ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        public int index;
        public Sprite sprite;
        public float appearPercentage;
    }
}

