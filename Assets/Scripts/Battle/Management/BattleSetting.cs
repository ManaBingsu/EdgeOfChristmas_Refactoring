using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager : MonoBehaviour
    {
        public int TotalRound;
        public int TotalWin;
        public int RoundWinScore;

        public void CountWin(SessionId winner)
        {
            players[winner].WinCount++;
            if (players[winner].WinCount >= TotalWin)
            {
                FlowState = EFlowState.Result;
            }
        }
    }
}
