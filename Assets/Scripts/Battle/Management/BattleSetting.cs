using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager : MonoBehaviour
    {
        public int totalRound;
        public int totalWin;

        public void CountWin(SessionId winner)
        {
            players[winner].WinCount++;
            if (players[winner].WinCount >= totalWin)
            {
                FlowState = EFlowState.Result;
            }
        }
    }
}
