using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Battle
{
    [Serializable]
    public partial class BattleManager : MonoBehaviour
    {
        public enum EFlowState
        {
            Idle,
            Start,
            Progress,
            RoundEnd,
            Result,
            COUNT
        }

        private EFlowState flowState;
        public EFlowState FlowState
        {
            get => flowState;
            set
            {
                if (value == flowState)
                    return;
                flowState = value;
                StateEvents[(int)flowState]();
            }
        }

        public Action[] StateEvents { get; set; }

        void InitStateEvents()
        {
            StateEvents = new Action[(int)EFlowState.COUNT];
            for (int i = 0; i < StateEvents.Length; i++)
            {
                StateEvents[i] = new Action(delegate { });
            }
        }
    }
}

