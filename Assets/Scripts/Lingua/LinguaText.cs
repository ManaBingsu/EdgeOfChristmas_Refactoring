using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lingua
{
    public class LinguaText : MonoBehaviour
    {
        private TextMeshProUGUI textMeshProObject;
        private Text textObject;
        [SerializeField]
        private string key;

        private void Awake()
        {
            textMeshProObject = GetComponent<TextMeshProUGUI>();
            if (textMeshProObject == null)
                textObject = GetComponent<Text>();
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
            if (textMeshProObject != null)
                textMeshProObject.text = Lingua.GetString(key);
            else
                textObject.text = Lingua.GetString(key);
        }
    }
}

