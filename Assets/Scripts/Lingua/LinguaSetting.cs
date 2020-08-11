using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lingua
{
    [CreateAssetMenu]
    public class LinguaSetting : ScriptableObject
    {
        [SerializeField]
        List<TextMeshProUGUI> textAssets;
    }
}

