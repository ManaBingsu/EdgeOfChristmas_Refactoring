using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Battle
{
    public class BattleUI : MonoBehaviour
    {
        const int startTime = 3;

        [SerializeField]
        GuidePanel guidePanel;

        private void Start()
        {
            SetStateEvent();
        }

        private void SetStateEvent()
        {
            BattleManager.Instance.StateEvents[(int)BattleManager.EFlowState.Start] += OnStartUIEvent;
            BattleManager.Instance.StateEvents[(int)BattleManager.EFlowState.Progress] += OnProgressUIEvent;
        }

        private void OnStartUIEvent()
        {
            guidePanel.panel.SetActive(true);
            StartCoroutine(DisplayTimer());
        }

        private IEnumerator DisplayTimer()
        {
            int time = startTime;
            WaitForSeconds waitTime = new WaitForSeconds(1.0f);
            while (time > 0)
            {
                guidePanel.text.text = time == 0 ? "" : time.ToString();
                time--;
                yield return waitTime;
            }
        }

        private void OnProgressUIEvent()
        {
            guidePanel.panel.SetActive(false);
        }

    }

    [Serializable]
    public class GuidePanel
    {
        public GameObject panel;
        public TextMeshProUGUI text;
    }
}

