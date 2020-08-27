using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    public class LoadingMessage : MonoBehaviour
    {
        public static LoadingMessage Instance;

        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private TextMeshProUGUI messageText;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this.gameObject);
        }

        public void SetActivePanel(bool isOn)
        {
            panel.SetActive(isOn);
        }

        public void SetMessage(string message)
        {
            messageText.text = message;
        }
    }
}

