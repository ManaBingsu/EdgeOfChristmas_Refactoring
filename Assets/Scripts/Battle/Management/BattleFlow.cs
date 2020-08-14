using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditorInternal;
using UnityEngine;

namespace Battle.Management
{
    [Serializable]
    public class BattleFlow
    {
        public enum EFlowState
        {
            Idle,
            Ready,
            Progress,
            Pause,
            Result,
            COUNT
        }

        private EFlowState flowState;
        public EFlowState FlowState
        {
            get => flowState;
            set
            {
                flowState = value;
                StateEvents[(int)flowState]();
            }
        }

        public Action[] StateEvents { get; set; }

        public BattleFlow()
        {
            InitField();
            InitState();
        }

        public void InitState()
        {
            FlowState = EFlowState.Ready;
        }

        void InitField()
        {
            StateEvents = new Action[(int)EFlowState.COUNT];
            for (int i = 0; i < StateEvents.Length; i++)
            {
                StateEvents[i] = new Action(() => { });
            }
        }
    }
}

