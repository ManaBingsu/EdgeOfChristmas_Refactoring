using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lingua
{
    public class LinguaText : MonoBehaviour
    {
        private TextMeshProUGUI textObject;
        [SerializeField]
        private string key;

        private void Awake()
        {
            textObject = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateText();
        }

        private void OnEnable()
        {
            UpdateText();
        }

        void UpdateText()
        {
            textObject.text = Lingua.GetString(key);
        }
    }
}

