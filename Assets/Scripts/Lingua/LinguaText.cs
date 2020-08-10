using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lingua
{
    public class LinguaText : MonoBehaviour
    {
        private TextMeshPro textObject;
        string[] strings;

        private void Awake()
        {
            textObject = GetComponent<TextMeshPro>();
        }

        private void OnEnable()
        {

        }

        void UpdateText()
        {

        }
    }
}

